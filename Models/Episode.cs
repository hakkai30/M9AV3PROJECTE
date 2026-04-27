using Newtonsoft.Json;

namespace RickAndMortyMVC.Models
{
    /// <summary>
    /// Model que representa un episodi de Rick & Morty.
    /// </summary>
    public class Episode
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("air_date")]
        public string AirDate { get; set; } = string.Empty;

        [JsonProperty("episode")]
        public string EpisodeCode { get; set; } = string.Empty;

        [JsonProperty("characters")]
        public List<string> Characters { get; set; } = new();

        [JsonProperty("url")]
        public string Url { get; set; } = string.Empty;

        [JsonProperty("created")]
        public DateTime Created { get; set; }

        // Extreu el número de temporada del codi (S01E01 → 1)
        public int SeasonNumber
        {
            get
            {
                if (string.IsNullOrEmpty(EpisodeCode) || EpisodeCode.Length < 3) return 0;
                return int.TryParse(EpisodeCode[1..3], out int s) ? s : 0;
            }
        }

        // Extreu el número d'episodi del codi (S01E01 → 1)
        public int EpisodeNumber
        {
            get
            {
                if (string.IsNullOrEmpty(EpisodeCode) || EpisodeCode.Length < 6) return 0;
                return int.TryParse(EpisodeCode[4..6], out int e) ? e : 0;
            }
        }
    }
}