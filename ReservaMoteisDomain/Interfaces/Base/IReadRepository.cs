namespace ReservaMoteisDomain.Interfaces.Base
{
    public interface IReadRepository<TId, TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> FindAll();
        Task<TEntity> FindById(TId id);
    }
}
