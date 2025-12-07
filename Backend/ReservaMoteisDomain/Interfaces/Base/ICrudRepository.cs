namespace BookMotelsDomain.Interfaces.Base
{
    public interface ICrudRepository<TId, TEntity> : IReadRepository<TId, TEntity> where TEntity : class
    {
        Task<bool> Exist(TId id);
        Task<TEntity> Add(TEntity entity);
        Task<TEntity> Update(TEntity entity);
        Task Delete(TEntity entity);
    }
}
