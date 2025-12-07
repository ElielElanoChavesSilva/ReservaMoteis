using BookMotelsDomain.Interfaces.Base;

namespace BookMotelsInfra.Repositories.Base
{
    public class CrudRepository<TId, TEntity> : ReadRepository<TId, TEntity>, ICrudRepository<TId, TEntity> where TEntity : class
    {
        public CrudRepository()
        {

        }
        public Task<TId> Add(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task Update(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(TId id)
        {
            throw new NotImplementedException();
        }
    }
}
