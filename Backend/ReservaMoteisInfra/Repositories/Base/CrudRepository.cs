using BookMotelsDomain.Interfaces.Base;
using BookMotelsInfra.Context;
using Microsoft.EntityFrameworkCore;

namespace BookMotelsInfra.Repositories.Base
{
    public class CrudRepository<TId, TEntity> : ReadRepository<TId, TEntity>, ICrudRepository<TId, TEntity> where TEntity : class
    {
        protected CrudRepository(MainContext context) : base(context)
        {
        }

        public async Task<bool> Exist(TId id)
        {
            var entity = await _context.Set<TEntity>().FindAsync(id);
            return entity != null;
        }

        public virtual async Task<TEntity> Add(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public virtual async Task<TEntity> Update(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return entity;
        }

        public virtual async Task Delete(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
