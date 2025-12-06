namespace ReservaMoteisDomain.Interfaces.Base
{
    public interface ICrudRepository<TId, TEntity> : IReadRepository<TId, TEntity>
    {
        Task<TId> Add(TEntity entity);
        Task Update(TEntity entity);
        Task Delete(TId id);
    }
}
