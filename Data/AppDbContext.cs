using Microsoft.EntityFrameworkCore;
using RickAndMortyMVC.Models;

namespace RickAndMortyMVC.Data
{
    /// <summary>
    /// Context de la base de dades SQLite.
    /// Hereta de DbContext (Entity Framework Core) i exposa
    /// la taula de personatges favorits.
    /// </summary>
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        /// <summary>Taula de personatges favorits.</summary>
        public DbSet<FavoriteCharacter> FavoriteCharacters { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Índex únic: un personatge de l'API no es pot afegir dues vegades
            modelBuilder.Entity<FavoriteCharacter>()
                .HasIndex(f => f.CharacterId)
                .IsUnique();

            // Configuració de la taula
            modelBuilder.Entity<FavoriteCharacter>()
                .ToTable("FavoriteCharacters");
        }
    }
}