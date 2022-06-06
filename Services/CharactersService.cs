using DisneyAPI.Models;
using DisneyAPI.Repositories;
using DisneyAPI.ViewModels;
using DisneyWebAPI.Models;

namespace DisneyAPI.Services
{
    public interface ICharactersService : IGenericService<Character>
    {
        Task<Character> GetById(int Id);
        Task<List<Character>> GetByName(string name);
        Task<List<Character>> GetByAgeOrWeight(int age, double weight);
        Task<List<Character>> GetByMovie(int movieId);
        Task<List<GetCharacters>> Get();
        Task<Character?> Add(AddCharacter character);
        Task<Character?> Update(UpdateCharacter character);
    }

    public class CharactersService : GenericService<Character>, ICharactersService
    {
        private readonly ICharactersRepository _repository;
        public CharactersService(ICharactersRepository repository) : base(repository)
        {
            _repository = repository;
        }

        public async Task<List<GetCharacters>> Get()
        {
            var characters = await _repository.Get();
            var model = new List<GetCharacters>();

            foreach (var c in characters)
            {
                model.Add(new GetCharacters
                {
                    Image = c.Image,
                    Name = c.Name
                });
            }

            return model;
        }

        public async Task<Character?> Add(AddCharacter character)
        {
            if (await _repository.ExistsWithName(character.Name))
                throw new Exception("The character is already exists.");

            var characterModel = new Character
            {
                CharacterId = null,
                Name = character.Name,
                Image = character.Image,
                Age = character.Age,
                Weight = character.Weight,
                History = character.History
            };

            var result = await _repository.Add(characterModel);

            return result;
        }

        public async Task<Character> GetById(int Id)
        {
            var entity = await _repository.GetById(Id);
            return entity;
        }

        public async Task<List<Character>> GetByAgeOrWeight(int age, double weight) => await _repository.GetByAgeOrWeight(age, weight);

        public async Task<List<Character>> GetByMovie(int movieId) => await _repository.GetByMovie(movieId);

        public async Task<List<Character>> GetByName(string name) => await _repository.GetByName(name);

        public async Task<Character?> Update(UpdateCharacter character)
        {
            var model = await GetById(character.CharacterId);

            model.Name = character.Name;
            model.Image = character.Image;
            model.Age = character.Age;
            model.Weight = character.Weight;
            model.History = character.History;
            model.MovieOrSerie = GetList(character.CharacterId, character.MovieOrSerieId);

            var result = await _repository.Update(character);

            return result;
        }

        private static List<CharacterMovieOrSerie> GetList(int character, int movieId)
        {
            var list = new List<CharacterMovieOrSerie>();

            list.Add(new CharacterMovieOrSerie { CharactersId = character, MoviesOrSeriesId = movieId });

            return list;
        }
    }
}

