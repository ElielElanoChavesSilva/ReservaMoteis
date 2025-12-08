using BookMotelsDomain.Entities;
using BookMotelsDomain.Interfaces;
using BookMotelsInfra.Context;
using BookMotelsInfra.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace BookMotelsInfra.Repositories
{
    public class ReserveRepository : CrudRepository<long, ReserveEntity>, IReserveRepository
    {
        public ReserveRepository(MainContext context) : base(context)
        {
        }

        public async Task<bool> HasConflictingReservation(long suiteId, DateTime checkIn, DateTime checkOut)
        {
            return await _context.Reserves
                .AnyAsync(r => r.SuiteId == suiteId &&
                               r.CheckIn <= checkOut && r.CheckOut >= checkIn);
        }

        public async Task<IEnumerable<ReserveEntity>> FindAllByUserAsync(Guid userId)
        {
            return await _context.Reserves
                .Where(r => r.UserId == userId).ToListAsync();
        }
    }
}
