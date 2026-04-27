using Newtonsoft.Json;
using RickAndMortyMVC.Models;

namespace RickAndMortyMVC.Services
{
    /// <summary>
    /// Implementació del servei que consumeix l'API REST de Rick & Morty.
    /// Fa ús de Newtonsoft.Json per a la deserialització (requeriment obligatori).
    /// </summary>
    public class RickAndMortyService : IRickAndMortyService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<RickAndMortyService> _logger;
        private const string BaseUrl = "https://rickandmortyapi.com/api";

        private static readonly JsonSerializerSettings _jsonSettings = new()
        {
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore
        };

        public RickAndMortyService(HttpClient httpClient, ILogger<RickAndMortyService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<ApiResponse<Character>> GetCharactersAsync(
            int page = 1,
            string? name = null,
            string? status = null,
            string? species = null,
            string? gender = null)
        {
            var query = new List<string> { $"page={page}" };
            if (!string.IsNullOrWhiteSpace(name))    query.Add($"name={Uri.EscapeDataString(name)}");
            if (!string.IsNullOrWhiteSpace(status))  query.Add($"status={Uri.EscapeDataString(status)}");
            if (!string.IsNullOrWhiteSpace(species)) query.Add($"species={Uri.EscapeDataString(species)}");
            if (!string.IsNullOrWhiteSpace(gender))  query.Add($"gender={Uri.EscapeDataString(gender)}");

            var url = $"{BaseUrl}/character?{string.Join("&", query)}";
            return await GetDeserializedAsync<ApiResponse<Character>>(url)
                   ?? new ApiResponse<Character>();
        }

        public async Task<Character?> GetCharacterByIdAsync(int id)
        {
            return await GetDeserializedAsync<Character>($"{BaseUrl}/character/{id}");
        }

        public async Task<List<Character>> GetCharactersByIdsAsync(IEnumerable<int> ids)
        {
            var idList = string.Join(",", ids);
            if (string.IsNullOrEmpty(idList)) return new();

            var result = await GetDeserializedAsync<List<Character>>($"{BaseUrl}/character/[{idList}]");
            return result ?? new();
        }

        public async Task<ApiResponse<Episode>> GetEpisodesAsync(int page = 1)
        {
            return await GetDeserializedAsync<ApiResponse<Episode>>($"{BaseUrl}/episode?page={page}")
                   ?? new ApiResponse<Episode>();
        }

        public async Task<Episode?> GetEpisodeByIdAsync(int id)
        {
            return await GetDeserializedAsync<Episode>($"{BaseUrl}/episode/{id}");
        }

        public async Task<List<Episode>> GetEpisodesByUrlsAsync(IEnumerable<string> urls)
        {
            var ids = urls
                .Select(url => url.Split('/').LastOrDefault())
                .Where(id => int.TryParse(id, out _))
                .Select(int.Parse!)
                .Distinct()
                .Take(20)
                .ToList();

            if (!ids.Any()) return new();

            var idList = string.Join(",", ids);
            var result = await GetDeserializedAsync<List<Episode>>($"{BaseUrl}/episode/[{idList}]");
            return result ?? new();
        }

        private async Task<T?> GetDeserializedAsync<T>(string url)
        {
            try
            {
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("API retornà {StatusCode} per a {Url}", response.StatusCode, url);
                    return default;
                }

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(json, _jsonSettings);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error de xarxa al cridar {Url}", url);
                return default;
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Error de deserialització JSON per a {Url}", url);
                return default;
            }
        }
    }
}