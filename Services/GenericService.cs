using DisneyAPI.Repositories;

namespace DisneyAPI.Services
{
    public interface IGenericService <TEntity> where TEntity : class
    {
        Task<bool> Delete(int Id);
    }
    public class GenericService<TEntity> : IGenericService<TEntity> where TEntity : class
    {
        private readonly IGenericRepository<TEntity> _repository; 
        public GenericService(IGenericRepository<TEntity> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Delete(int Id)
        { 
            if(!await _repository.Delete(Id))
                return false;

            return true;
        }


    }
}
