using DisneyAPI.Data;
using DisneyAPI.Models;
using DisneyAPI.ViewModels;
using DisneyWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DisneyAPI.Repositories
{
    public interface ICharactersRepository : IGenericRepository<Character>
    {
        Task<Character> Add(Character character);
        Task<Character> Update(UpdateCharacter character);
        Task<Character> GetById(int Id);
        Task<List<Character>> GetByName(string name);
        Task<List<Character>> GetByAgeOrWeight(int age, double weight);
        Task<List<Character>> GetByMovie(int movieId);
        Task<bool> ExistsWithName(string characterName);
    }
    public class CharactersRepository : GenericRepository<Character>, ICharactersRepository
    {
        private readonly DisneyDbContext _context;
        public CharactersRepository(DisneyDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> ExistsWithName(string name)
        {
            var list = await Get();

            bool Exists = false;

            foreach (var c in list)
            {
                if (c.Name == name)
                    Exists = true;
            }

            return Exists;
        }

        public async Task<Character> GetById(int Id)
        {
            var character = await _context.Characters.Where(c => c.CharacterId == Id).Include(m => m.MovieOrSerie).FirstOrDefaultAsync();
            return character;
        }

        public async Task<List<Character>> GetByAgeOrWeight(int age, double weight)
        {
            try
            {
                var characters = await _context.Characters.Where(c => c.Age == age || c.Weight == weight).ToListAsync();
                return characters;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Character>> GetByMovie(int movieId)
        {
            try
            {
                var character = await _context.Characters.Include(m => m.MovieOrSerie.Where(m => m.MoviesOrSeriesId == movieId)).ToListAsync();
                return character;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Character>> GetByName(string name)
        {
            try
            {
                var characters = await _context.Characters.Where(c => c.Name.StartsWith(name)).ToListAsync();
                return characters;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Character> Add(Character character)
        {
            var list = new List<CharacterMovieOrSerie>();
            
            try
            {
                var lastChar = await _context.Characters.OrderBy(c => c.CharacterId).LastOrDefaultAsync();

                foreach (var m in character.MovieOrSerie)
                    list.Add(new CharacterMovieOrSerie { CharactersId = lastChar.CharacterId+1, MoviesOrSeriesId = m.MoviesOrSeriesId });

                character.MovieOrSerie = list;

                EntityEntry<Character> entity = await _context.Characters.AddAsync(character);
                await _context.SaveChangesAsync();

                return await GetById(entity.Entity.CharacterId ?? throw new Exception("there was an error saving character"));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Character> Update(UpdateCharacter character)
        {
            try
            {
                var model = await _context.Characters.Where(c => c.CharacterId == character.CharacterId)
                                                     .FirstOrDefaultAsync();

                model.Image = character.Image;
                model.Name = character.Name;
                model.Age = character.Age;
                model.Weight = character.Weight;
                model.History = character.History;

                _context.Characters.Update(model);

                await _context.SaveChangesAsync();

                return model;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

    }
}
