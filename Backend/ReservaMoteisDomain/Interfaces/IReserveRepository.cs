using BookMotelsDomain.Entities;
using BookMotelsDomain.Interfaces.Base;
using BookMotelsDomain.Models;

namespace BookMotelsDomain.Interfaces
{
    public interface IReserveRepository : ICrudRepository<long, ReserveEntity>
    {
        Task<bool> HasConflictingReservation(long suiteId, DateTime checkIn, DateTime checkOut);
        Task<IEnumerable<ReserveEntity>> FindAllByUserAsync(Guid userId);
        Task<IEnumerable<BillingReportModel>> FindBillingReport(long? motelId, int? year, int? month);

    }
}
