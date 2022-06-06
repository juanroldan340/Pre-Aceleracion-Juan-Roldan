using DisneyAPI.Data;
using DisneyAPI.Models;
using DisneyAPI.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DisneyAPI.Repositories
{
    public interface IMoviesOrSeriesRepository : IGenericRepository<MovieOrSerie>
    {
        Task<MovieOrSerie> Update(UpdateMovieOrSerie movieOrSerie);
        Task<MovieOrSerie> Add(MovieOrSerie movieOrSerie);
        Task<MovieOrSerie> GetById(int Id);
        Task<List<MovieOrSerie>> GetOrdered(string order);
        Task<List<MovieOrSerie>> GetByGenre(int genreId);
        Task<List<MovieOrSerie>> GetByTitle(string title);
        Task<bool> ExistsWithTitle(string movieOrSerieTitle);
    }
    public class MoviesOrSeriesRepository : GenericRepository<MovieOrSerie>, IMoviesOrSeriesRepository
    {
        private readonly DisneyDbContext _context;
        public MoviesOrSeriesRepository(DisneyDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> ExistsWithTitle(string title)
        {
            var list = await Get();

            bool Exists = false;

            foreach (var c in list)
            {
                if (c.Title == title)
                    Exists = true;
            }

            return Exists;
        }

        public async Task<MovieOrSerie> GetById(int Id)
        {
            var entity = await _context.MoviesOrSeries.Where(m => m.MovieOrSerieId == Id).Include(c => c.Character).FirstOrDefaultAsync();
            //await _context.MoviesOrSeries.Include(m => m.Character.Select(c => c.CharacterId)).FirstOrDefaultAsync();
            return entity;
        }
        public async Task<List<MovieOrSerie>> GetByGenre(int genreId)
        {
            try
            {
                var moviesOrSeries = await _context.MoviesOrSeries.Where(m => m.GenreId.Equals(genreId)).ToListAsync();
                return moviesOrSeries;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<MovieOrSerie>> GetByTitle(string title)
        {
            try
            {
                var moviesOrSeries = await _context.MoviesOrSeries.Where(m => m.Title.Contains(title)).ToListAsync();
                return moviesOrSeries;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<MovieOrSerie>> GetOrdered(string order)
        {
            try
            {
                var moviesOrSeries = new List<MovieOrSerie>();
                if (order == "ASC")
                    moviesOrSeries = await (from c in _context.MoviesOrSeries
                                    orderby c.Title ascending
                                    select c).ToListAsync();
                if (order == "DESC")
                    moviesOrSeries = await (from c in _context.MoviesOrSeries
                                    orderby c.Title descending
                                    select c).ToListAsync();

                return moviesOrSeries;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<MovieOrSerie> Add(MovieOrSerie movieOrSerie)
        {
            try
            {
                EntityEntry<MovieOrSerie> entity = await _context.MoviesOrSeries.AddAsync(movieOrSerie);

                await _context.SaveChangesAsync();

                return await GetById(entity.Entity.MovieOrSerieId ?? throw new Exception("there was an error saving movie or serie"));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<MovieOrSerie> Update(UpdateMovieOrSerie movieOrSerie)
        {
            var entity = await _context.MoviesOrSeries.Where(m => m.MovieOrSerieId == movieOrSerie.MovieOrSerieId).Include(m => m.Character).FirstOrDefaultAsync();

            _context.MoviesOrSeries.Update(entity);
            try
            {
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
