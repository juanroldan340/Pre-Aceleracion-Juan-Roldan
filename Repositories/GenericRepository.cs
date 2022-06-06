using DisneyAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace DisneyAPI.Repositories
{
    public interface IGenericRepository<TEntity>
    {
        Task<bool> Delete(int Id);
        Task<List<TEntity>> Get();
    }
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly DisneyDbContext _context;

        public GenericRepository(DisneyDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Delete(int Id)
        {
            var entity = await _context.Set<TEntity>().FindAsync(Id);

            _context.Set<TEntity>().Remove(entity);

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<TEntity>> Get()
        {
            var entities = await _context.Set<TEntity>().ToListAsync();
            return entities;
        }
    }

}
