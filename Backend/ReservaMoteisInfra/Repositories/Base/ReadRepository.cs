using BookMotelsDomain.Interfaces.Base;
using BookMotelsInfra.Context;
using Microsoft.EntityFrameworkCore;

namespace BookMotelsInfra.Repositories.Base
{
    public class ReadRepository<TId, TEntity> : IReadRepository<TId, TEntity> where TEntity : class
    {
        public readonly MainContext _context;

        public ReadRepository(MainContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TEntity>> FindAll()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity?> FindById(TId id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }
    }
}
