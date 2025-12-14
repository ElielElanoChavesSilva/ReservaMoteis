using BookMotelsDomain.Entities;
using BookMotelsDomain.Interfaces;
using BookMotelsDomain.Projections;
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

        public async Task<IEnumerable<GetReserveProjection>> FindAllByUser(Guid userId)
        {
            var query = await _context.Reserves
                .Include(s => s.Suite)
                .ThenInclude(m => m.Motel).
                 Include(x => x.User)
                .Where(r => r.UserId == userId).ToListAsync();


            return query.Select(x => new GetReserveProjection
            {
                Id = x.Id,
                UserName = x.User.Name,
                MotelName = x.Suite.Motel!.Name,
                SuiteId = x.Suite.Id,
                CheckIn = x.CheckIn,
                CheckOut = x.CheckOut,
                SuiteName = x.Suite.Name
            });
        }

        public Task<GetReserveProjection> FindByIdProjection(long id)
        {
            {
                var query = _context.Reserves
                    .Include(s => s.Suite)
                    .ThenInclude(m => m.Motel)
                    .Include(x => x.User)
                    .Single(r => r.Id == id);

                return Task.FromResult(new GetReserveProjection
                {
                    Id = query.Id,
                    UserName = query.User.Name,
                    MotelName = query.Suite.Motel!.Name,
                    SuiteId = query.Suite.Id,
                    CheckIn = query.CheckIn,
                    CheckOut = query.CheckOut,
                    SuiteName = query.Suite.Name
                });
            }
        }

        public async Task<IEnumerable<GetReserveProjection>> FindAllProjection()
        {
            var query = _context.Reserves
                .Include(s => s.Suite)
                .ThenInclude(m => m.Motel).
                Include(x => x.User).AsQueryable();

            return await query.Select(x => new GetReserveProjection
            {
                Id = x.Id,
                UserName = x.User.Name,
                MotelName = x.Suite.Motel!.Name,
                SuiteId = x.Suite.Id,
                CheckIn = x.CheckIn,
                CheckOut = x.CheckOut,
                SuiteName = x.Suite.Name
            }).ToListAsync();
        }

        public async Task<IEnumerable<BillingReportProjection>> FindBillingReport(long? motelId, int? year, int? month)
        {
            var query = _context.Reserves
                .Include(r => r.Suite)
                .ThenInclude(s => s.Motel)
                .AsQueryable();

            if (motelId.HasValue)
                query = query.Where(r => r.Suite.MotelId == motelId.Value);

            if (year.HasValue)
                query = query.Where(r => r.CheckOut.Year == year.Value);

            if (month.HasValue)
                query = query.Where(r => r.CheckOut.Month == month.Value);

            var report = await query
                .GroupBy(r => new
                {
                    r.Suite.MotelId,
                    MotelName = r.Suite.Motel!.Name,
                    r.CheckOut.Year,
                    r.CheckOut.Month
                })
                .Select(g => new BillingReportProjection(
                    g.Key.MotelId,
                    g.Key.MotelName,
                    g.Key.Year,
                    g.Key.Month,
                    (decimal)g.Sum(r => (double)r.TotalPrice)))
                .ToListAsync();

            return report;
        }
    }
}
