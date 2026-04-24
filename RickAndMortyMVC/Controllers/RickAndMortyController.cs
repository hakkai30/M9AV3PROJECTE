using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RickAndMortyMVC.Data;
using RickAndMortyMVC.Models;
using RickAndMortyMVC.Models.ViewModels;
using RickAndMortyMVC.Services;

namespace RickAndMortyMVC.Controllers
{
    /// <summary>
    /// Controlador principal que gestiona totes les rutes relacionades
    /// amb personatges i episodis de Rick & Morty.
    ///
    /// RUTES disponibles:
    ///   GET  /RickAndMorty/Characters          → Llistat de personatges (paginat + filtres)
    ///   GET  /RickAndMorty/Character/{id}      → Detall d'un personatge
    ///   GET  /RickAndMorty/Episodes            → Llistat d'episodis
    ///   GET  /RickAndMorty/Episode/{id}        → Detall d'un episodi
    ///   GET  /RickAndMorty/Favorites           → Llista de favorits (BBDD)
    ///   POST /RickAndMorty/AddFavorite         → Afegir a favorits (BBDD)
    ///   POST /RickAndMorty/RemoveFavorite/{id} → Eliminar de favorits (BBDD)
    /// </summary>
    public class RickAndMortyController : Controller
    {
        private readonly IRickAndMortyService _apiService;
        private readonly AppDbContext _db;
        private readonly ILogger<RickAndMortyController> _logger;

        public RickAndMortyController(
            IRickAndMortyService apiService,
            AppDbContext db,
            ILogger<RickAndMortyController> logger)
        {
            _apiService = apiService;
            _db = db;
            _logger = logger;
        }

        // ══════════════════════════════════════════════════════════════════
        // PERSONATGES
        // ══════════════════════════════════════════════════════════════════

        /// <summary>
        /// GET /RickAndMorty/Characters?page=1&name=Rick&status=Alive&...
        /// Mostra el llistat paginat de personatges amb filtres opcionals.
        /// </summary>
        public async Task<IActionResult> Characters(
            int page = 1,
            string? name = null,
            string? status = null,
            string? species = null,
            string? gender = null)
        {
            var apiResponse = await _apiService.GetCharactersAsync(page, name, status, species, gender);

            // Obtenir els IDs de favorits de la BBDD per marcar els personatges
            var favoriteIds = _db.FavoriteCharacters
                .Select(f => f.CharacterId)
                .ToHashSet();

            var viewModel = new CharacterListViewModel
            {
                Characters    = apiResponse.Results,
                CurrentPage   = page,
                TotalPages    = apiResponse.Info?.Pages ?? 1,
                TotalCount    = apiResponse.Info?.Count ?? 0,
                SearchName    = name,
                FilterStatus  = status,
                FilterSpecies  = species,
                FilterGender  = gender,
                FavoriteIds   = favoriteIds
            };

            return View(viewModel);
        }

        /// <summary>
        /// GET /RickAndMorty/Character/{id}
        /// Mostra el detall d'un personatge i els seus episodis.
        /// </summary>
        public async Task<IActionResult> Character(int id)
        {
            var character = await _apiService.GetCharacterByIdAsync(id);
            if (character == null)
            {
                _logger.LogWarning("Personatge {Id} no trobat", id);
                return NotFound();
            }

            // Obtenir els episodis del personatge (màxim 10 per no sobrecarregar)
            var episodeUrls = character.Episode.Take(10);
            var episodes = await _apiService.GetEpisodesByUrlsAsync(episodeUrls);

            var isFavorite = await _db.FavoriteCharacters
                .AnyAsync(f => f.CharacterId == id);

            var viewModel = new CharacterDetailViewModel
            {
                Character  = character,
                Episodes   = episodes,
                IsFavorite = isFavorite
            };

            return View(viewModel);
        }

        // ══════════════════════════════════════════════════════════════════
        // EPISODIS
        // ══════════════════════════════════════════════════════════════════

        /// <summary>
        /// GET /RickAndMorty/Episodes?page=1
        /// Mostra el llistat paginat d'episodis agrupats per temporada.
        /// </summary>
        public async Task<IActionResult> Episodes(int page = 1)
        {
            var apiResponse = await _apiService.GetEpisodesAsync(page);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages  = apiResponse.Info?.Pages ?? 1;
            ViewBag.TotalCount  = apiResponse.Info?.Count ?? 0;

            // Agrupar per temporada per mostrar-los ordenats
            var grouped = apiResponse.Results
                .GroupBy(e => e.SeasonNumber)
                .OrderBy(g => g.Key)
                .ToDictionary(g => g.Key, g => g.ToList());

            return View(grouped);
        }

        /// <summary>
        /// GET /RickAndMorty/Episode/{id}
        /// Mostra el detall d'un episodi i els primers personatges.
        /// </summary>
        public async Task<IActionResult> Episode(int id)
        {
            var episode = await _apiService.GetEpisodeByIdAsync(id);
            if (episode == null) return NotFound();

            // Obtenir els primers 12 personatges de l'episodi
            var characterUrls = episode.Characters.Take(12);
            var characters = await _apiService.GetEpisodesByUrlsAsync(characterUrls);

            // Obtenir personatges reals (no episodis) - fem servir el servei adequat
            var charIds = episode.Characters
                .Take(12)
                .Select(url => url.Split('/').LastOrDefault())
                .Where(id => int.TryParse(id, out _))
                .Select(int.Parse!)
                .ToList();

            var episodeCharacters = await _apiService.GetCharactersByIdsAsync(charIds);

            ViewBag.Episode = episode;
            return View(episodeCharacters);
        }

        // ══════════════════════════════════════════════════════════════════
        // FAVORITS (BBDD SQLite)
        // ══════════════════════════════════════════════════════════════════

        /// <summary>
        /// GET /RickAndMorty/Favorites
        /// Mostra la llista de personatges favorits guardats a la BBDD.
        /// </summary>
        public async Task<IActionResult> Favorites()
        {
            // Consulta a la BBDD SQLite mitjançant Entity Framework
            var favorites = await _db.FavoriteCharacters
                .OrderByDescending(f => f.AddedAt)
                .ToListAsync();

            return View(favorites);
        }

        /// <summary>
        /// POST /RickAndMorty/AddFavorite
        /// Afegeix un personatge als favorits a la BBDD SQLite.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddFavorite(int characterId, string? returnUrl = null)
        {
            // Comprovar si ja existeix (evitar duplicats per la restricció UNIQUE)
            var exists = await _db.FavoriteCharacters
                .AnyAsync(f => f.CharacterId == characterId);

            if (!exists)
            {
                // Obtenir les dades actualitzades de l'API
                var character = await _apiService.GetCharacterByIdAsync(characterId);
                if (character != null)
                {
                    var favorite = FavoriteCharacter.FromCharacter(character);
                    _db.FavoriteCharacters.Add(favorite);
                    await _db.SaveChangesAsync();
                    TempData["SuccessMessage"] = $"'{character.Name}' afegit als favorits!";
                }
            }
            else
            {
                TempData["InfoMessage"] = "Aquest personatge ja és als teus favorits.";
            }

            // Redirigir de tornada a la pàgina anterior
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction(nameof(Favorites));
        }

        /// <summary>
        /// POST /RickAndMorty/RemoveFavorite/{id}
        /// Elimina un personatge dels favorits de la BBDD.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFavorite(int id, string? returnUrl = null)
        {
            var favorite = await _db.FavoriteCharacters
                .FirstOrDefaultAsync(f => f.CharacterId == id);

            if (favorite != null)
            {
                _db.FavoriteCharacters.Remove(favorite);
                await _db.SaveChangesAsync();
                TempData["SuccessMessage"] = $"'{favorite.Name}' eliminat dels favorits.";
            }

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction(nameof(Favorites));
        }
    }
}
