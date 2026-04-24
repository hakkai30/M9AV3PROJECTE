using Microsoft.AspNetCore.Mvc;
using RickAndMortyMVC.Data;
using RickAndMortyMVC.Models.ViewModels;
using RickAndMortyMVC.Services;

namespace RickAndMortyMVC.Controllers
{
    /// <summary>
    /// Controlador de la pàgina d'inici.
    /// Ruta: /  o  /Home/Index
    /// </summary>
    public class HomeController : Controller
    {
        private readonly IRickAndMortyService _apiService;
        private readonly AppDbContext _db;

        public HomeController(IRickAndMortyService apiService, AppDbContext db)
        {
            _apiService = apiService;
            _db = db;
        }

        // GET: /
        public async Task<IActionResult> Index()
        {
            // Carreguem dades en paral·lel per millorar el rendiment
            var charactersTask = _apiService.GetCharactersAsync(page: 1);
            var episodesTask   = _apiService.GetEpisodesAsync(page: 1);

            await Task.WhenAll(charactersTask, episodesTask);

            var charactersResponse = await charactersTask;
            var episodesResponse   = await episodesTask;

            var viewModel = new HomeViewModel
            {
                FeaturedCharacters = charactersResponse.Results.Take(6).ToList(),
                LatestEpisodes     = episodesResponse.Results.Take(5).ToList(),
                TotalCharacters    = charactersResponse.Info?.Count ?? 0,
                TotalEpisodes      = episodesResponse.Info?.Count ?? 0,
                TotalFavorites     = _db.FavoriteCharacters.Count()
            };

            return View(viewModel);
        }

        // GET: /Home/Error
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}