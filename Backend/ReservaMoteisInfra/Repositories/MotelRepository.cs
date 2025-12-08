using BookMotelsDomain.DTOs;
using BookMotelsDomain.Entities;
using BookMotelsDomain.Interfaces;
using BookMotelsInfra.Context;
using BookMotelsInfra.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace BookMotelsInfra.Repositories
{
    public class MotelRepository : CrudRepository<long, MotelEntity>, IMotelRepository
    {
        public MotelRepository(MainContext context) : base(context)
        {
        }

        public async Task<IEnumerable<BillingReportDTO>> FindBillingReport(long? motelId, int? year, int? month)
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
                    r.Suite.Motel.Id,
                    r.Suite.Motel.Name,
                    Year = r.CheckOut.Year,
                    Month = r.CheckOut.Month
                })
                .Select(g => new BillingReportDTO
                {
                    MotelId = g.Key.Id,
                    MotelName = g.Key.Name,
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    TotalRevenue = (decimal)g.Sum(r => (double)r.Suite.PricePerPeriod)
                })
                .ToListAsync();

            return report;
        }
    }
}
