using Newtonsoft.Json;

namespace RickAndMortyMVC.Models
{
    /// <summary>
    /// Model que representa un personatge de l'API de Rick & Morty.
    /// Els atributs [JsonProperty] mapegen els camps JSON als atributs C#.
    /// </summary>
    public class Character
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>Estat: Alive, Dead o unknown</summary>
        [JsonProperty("status")]
        public string Status { get; set; } = string.Empty;

        [JsonProperty("species")]
        public string Species { get; set; } = string.Empty;

        [JsonProperty("type")]
        public string Type { get; set; } = string.Empty;

        /// <summary>Gènere: Female, Male, Genderless o unknown</summary>
        [JsonProperty("gender")]
        public string Gender { get; set; } = string.Empty;

        [JsonProperty("origin")]
        public Location? Origin { get; set; }

        [JsonProperty("location")]
        public Location? Location { get; set; }

        /// <summary>URL de la imatge del personatge (s'usa com a alt text per accessibilitat)</summary>
        [JsonProperty("image")]
        public string Image { get; set; } = string.Empty;

        [JsonProperty("episode")]
        public List<string> Episode { get; set; } = new();

        [JsonProperty("url")]
        public string Url { get; set; } = string.Empty;

        [JsonProperty("created")]
        public DateTime Created { get; set; }

        // ── Propietats derivades (no vénen de l'API) ──────────────────────
        /// <summary>Classe CSS per al badge d'estat (verd/vermell/gris)</summary>
        public string StatusCssClass => Status.ToLower() switch
        {
            "alive" => "status-alive",
            "dead"  => "status-dead",
            _       => "status-unknown"
        };

        /// <summary>Text alternatiu descriptiu per a la imatge (WAVE accessibility)</summary>
        public string ImageAltText => $"Imatge del personatge {Name}, {Species} {Gender}, estat: {Status}";
    }

    /// <summary>Ubicació (origen o localització actual) d'un personatge.</summary>
    public class Location
    {
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("url")]
        public string Url { get; set; } = string.Empty;
    }
}
