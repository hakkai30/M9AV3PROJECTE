using System.ComponentModel.DataAnnotations;

namespace RickAndMortyMVC.Models
{
    /// <summary>
    /// Model per a la taula de personatges favorits a la BBDD SQLite.
    /// </summary>
    public class FavoriteCharacter
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CharacterId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Status { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Species { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Gender { get; set; } = string.Empty;

        [MaxLength(500)]
        public string ImageUrl { get; set; } = string.Empty;

        [MaxLength(200)]
        public string OriginName { get; set; } = string.Empty;

        public DateTime AddedAt { get; set; } = DateTime.UtcNow;

        [MaxLength(500)]
        public string? Notes { get; set; }

        // Fàbrica: crea un FavoriteCharacter a partir d'un Character de l'API
        public static FavoriteCharacter FromCharacter(Character c) => new()
        {
            CharacterId = c.Id,
            Name        = c.Name,
            Status      = c.Status,
            Species     = c.Species,
            Gender      = c.Gender,
            ImageUrl    = c.Image,
            OriginName  = c.Origin?.Name ?? "Unknown",
            AddedAt     = DateTime.UtcNow
        };

        // Classe CSS per al badge d'estat
        public string StatusCssClass => Status.ToLower() switch
        {
            "alive" => "status-alive",
            "dead"  => "status-dead",
            _       => "status-unknown"
        };

        // Text alternatiu per a la imatge (accessibilitat WAVE)
        public string ImageAltText => $"Imatge del personatge favorit {Name}, {Species}, estat: {Status}";
    }
}