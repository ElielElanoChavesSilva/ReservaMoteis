using BookMotelsDomain.Entities;
using BookMotelsDomain.Interfaces;
using BookMotelsInfra.Context;
using BookMotelsInfra.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace BookMotelsInfra.Repositories
{
    public class SuiteRepository : CrudRepository<long, SuiteEntity>, ISuiteRepository
    {
        public SuiteRepository(MainContext context) : base(context)
        {
        }

        public async Task<IEnumerable<SuiteEntity>> FindAllAvailable(long motelId, string? name, DateTime? checkIn, DateTime? checkOut)
        {
            var query = _context.Suites
                .Where(s => s.MotelId == motelId)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(s => s.Name.Contains(name));

            if (checkIn.HasValue && checkOut.HasValue)
            {
                query = query.Where(s => !_context.Reserves.Any(r =>
                    r.SuiteId == s.Id &&
                    r.CheckIn < checkOut.Value &&
                    r.CheckOut > checkIn.Value
                ));
            }

            return await query.ToListAsync();
        }
    }
}
