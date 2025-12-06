using ReservaMoteisDomain.Interfaces.Base;

namespace ReservaMoteisInfra.Repositories.Base
{
    public abstract class ReadRepository<TId, TEntity> : IReadRepository<TId, TEntity> where TEntity : class
    {
        public Task<IEnumerable<TEntity>> FindAll()
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> FindById(TId id)
        {
            throw new NotImplementedException();
        }
    }
}
