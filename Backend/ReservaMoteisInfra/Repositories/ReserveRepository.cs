using BookMotelsDomain.Entities;
using BookMotelsDomain.Interfaces;
using BookMotelsDomain.Models;
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

        public async Task<IEnumerable<BillingReportModel>> FindBillingReport(long? motelId, int? year, int? month)
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
                    MotelId = r.Suite.MotelId,
                    MotelName = r.Suite.Motel.Name,
                    Year = r.CheckOut.Year,
                    Month = r.CheckOut.Month
                })
                .Select(g => new BillingReportModel
                {
                    MotelId = g.Key.MotelId,
                    MotelName = g.Key.MotelName,
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    TotalRevenue = (decimal)g.Sum(r => (double)r.TotalPrice)
                })
                .ToListAsync();

            return report;
        }
    }
}
