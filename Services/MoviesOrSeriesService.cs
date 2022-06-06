using DisneyAPI.Models;
using DisneyAPI.Repositories;
using DisneyAPI.ViewModels;

namespace DisneyAPI.Services
{
    public interface IMoviesOrSeriesService : IGenericService<MovieOrSerie>
    {
        Task<MovieOrSerie> Update(UpdateMovieOrSerie movieOrSerie);
        Task<MovieOrSerie> GetById(int Id);
        Task<List<MovieOrSerie>> GetOrdered(string order);
        Task<List<MovieOrSerie>> GetByGenre(int genreId);
        Task<List<MovieOrSerie>> GetByTitle(string title);
        Task<List<GetMoviesOrSeries>> Get();
        Task<MovieOrSerie> Add(MovieOrSerie movieOrSerie);
    }

    public class MoviesOrSeriesService : GenericService<MovieOrSerie>, IMoviesOrSeriesService
    {
        private readonly IMoviesOrSeriesRepository _repository;

        public MoviesOrSeriesService(IMoviesOrSeriesRepository repository) : base(repository)
        {
            _repository = repository;
        }

        public async Task<MovieOrSerie> Add(MovieOrSerie movieOrSerie)
        {
            if (await _repository.ExistsWithTitle(movieOrSerie.Title))
                throw new Exception("The movie or serie already exists.");

            var result = await _repository.Add(movieOrSerie);

            return result;
        }


        public async Task<MovieOrSerie> Update(UpdateMovieOrSerie entity)
        {
            var result = await _repository.Update(entity);

            return result;
        }

        public async Task<List<GetMoviesOrSeries>> Get()
        {
            var moviesOrSeries = await _repository.Get();

            var model = new List<GetMoviesOrSeries>();

            foreach (var m in moviesOrSeries)
            {
                model.Add(new GetMoviesOrSeries
                {
                    Image = m.Image,
                    Title = m.Title,
                    CreationDate = m.CreationDate
                });
            }

            return model;
        }

        public async Task<MovieOrSerie> GetById(int Id)
        {
            var entity = await _repository.GetById(Id);
            return entity;
        }

        public async Task<List<MovieOrSerie>> GetOrdered(string order) => await _repository.GetOrdered(order);

        public async Task<List<MovieOrSerie>> GetByGenre(int genreId) => await _repository.GetByGenre(genreId);

        public async Task<List<MovieOrSerie>> GetByTitle(string title) => await _repository.GetByTitle(title);
    }
}
