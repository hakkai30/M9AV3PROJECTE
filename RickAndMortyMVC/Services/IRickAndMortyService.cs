using RickAndMortyMVC.Models;

namespace RickAndMortyMVC.Services
{
    /// <summary>
    /// Interfície del servei que abstreu les crides a l'API de Rick & Morty.
    /// Segueix el principi de Inversió de Dependències (POO - SOLID).
    /// </summary>
    public interface IRickAndMortyService
    {
        /// <summary>Obté una pàgina de personatges amb filtres opcionals.</summary>
        Task<ApiResponse<Character>> GetCharactersAsync(
            int page = 1,
            string? name = null,
            string? status = null,
            string? species = null,
            string? gender = null);

        /// <summary>Obté el detall d'un personatge pel seu ID.</summary>
        Task<Character?> GetCharacterByIdAsync(int id);

        /// <summary>Obté múltiples personatges pels seus IDs.</summary>
        Task<List<Character>> GetCharactersByIdsAsync(IEnumerable<int> ids);

        /// <summary>Obté una pàgina d'episodis.</summary>
        Task<ApiResponse<Episode>> GetEpisodesAsync(int page = 1);

        /// <summary>Obté el detall d'un episodi pel seu ID.</summary>
        Task<Episode?> GetEpisodeByIdAsync(int id);

        /// <summary>Obté múltiples episodis pels seus URLs.</summary>
        Task<List<Episode>> GetEpisodesByUrlsAsync(IEnumerable<string> urls);
    }
}
