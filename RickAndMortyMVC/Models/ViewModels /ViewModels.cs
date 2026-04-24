namespace RickAndMortyMVC.Models.ViewModels
{
    /// <summary>
    /// ViewModel per a la vista de llistat de personatges.
    /// </summary>
    public class CharacterListViewModel
    {
        public List<Character> Characters { get; set; } = new();
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; } = 1;
        public int TotalCount { get; set; }
        public string? SearchName { get; set; }
        public string? FilterStatus { get; set; }
        public string? FilterSpecies { get; set; }
        public string? FilterGender { get; set; }
        public HashSet<int> FavoriteIds { get; set; } = new();
    }

    /// <summary>
    /// ViewModel per a la vista de detall d'un personatge.
    /// </summary>
    public class CharacterDetailViewModel
    {
        public Character? Character { get; set; }
        public List<Episode> Episodes { get; set; } = new();
        public bool IsFavorite { get; set; }
    }

    /// <summary>
    /// ViewModel per a la pàgina d'inici.
    /// </summary>
    public class HomeViewModel
    {
        public List<Character> FeaturedCharacters { get; set; } = new();
        public List<Episode> LatestEpisodes { get; set; } = new();
        public int TotalCharacters { get; set; }
        public int TotalEpisodes { get; set; }
        public int TotalFavorites { get; set; }
    }
}