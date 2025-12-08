using BookMotelsDomain.Entities;
using BookMotelsDomain.Interfaces.Base;

namespace BookMotelsDomain.Interfaces
{
    public interface ISuiteRepository : ICrudRepository<long, SuiteEntity>
    {
        Task<IEnumerable<SuiteEntity>> FindAllAvailable(long motelId, string? name, DateTime? checkIn,
            DateTime? checkOut);

    }
}
