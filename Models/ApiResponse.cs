using Newtonsoft.Json;

namespace RickAndMortyMVC.Models
{
    /// <summary>
    /// Resposta paginada de l'API de Rick & Morty.
    /// Newtonsoft.Json gestiona la deserialització automàtica.
    /// </summary>
    public class ApiResponse<T>
    {
        [JsonProperty("info")]
        public PageInfo? Info { get; set; }

        [JsonProperty("results")]
        public List<T> Results { get; set; } = new();
    }

    /// <summary>
    /// Informació de paginació retornada per l'API.
    /// </summary>
    public class PageInfo
    {
        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("pages")]
        public int Pages { get; set; }

        [JsonProperty("next")]
        public string? Next { get; set; }

        [JsonProperty("prev")]
        public string? Prev { get; set; }
    }
}