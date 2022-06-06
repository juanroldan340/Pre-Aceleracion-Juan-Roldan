using DisneyAPI.Models;
using DisneyWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DisneyAPI.Data
{
    public class DisneyDbContext : DbContext
    {
        public DisneyDbContext(DbContextOptions<DisneyDbContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Genre>().HasData(new List<Genre>() 
            { 
                new Genre { GenreId = 1, Name = "Aventura", MovieOrSerie = null, Image = null },
                new Genre { GenreId = 2, Name = "Romántica", MovieOrSerie = null, Image = null },
                new Genre { GenreId = 3, Name = "Acción", MovieOrSerie = null, Image = null },
                new Genre { GenreId = 4, Name = "Ciencia Ficción", MovieOrSerie = null, Image = null },
                new Genre { GenreId = 5, Name = "Comedia", MovieOrSerie = null, Image = null },
                new Genre { GenreId = 6, Name = "Terror", MovieOrSerie = null, Image = null },
            });
            modelBuilder.Entity<CharacterMovieOrSerie>().HasKey(cmos => new { cmos.CharactersId, cmos.MoviesOrSeriesId });

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<CharacterMovieOrSerie> CharactersMoviesOrSeries { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<MovieOrSerie> MoviesOrSeries { get; set; }
        public DbSet<Genre> Genres { get; set; }
    }
}
