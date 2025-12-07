namespace BookMotelsDomain.Interfaces.Base
{
    public interface ICrudRepository<TId, TEntity> : IReadRepository<TId, TEntity> where TEntity : class
    {
        Task<TId> Add(TEntity entity);
        Task Update(TEntity entity);
        Task Delete(TId id);
    }
}
