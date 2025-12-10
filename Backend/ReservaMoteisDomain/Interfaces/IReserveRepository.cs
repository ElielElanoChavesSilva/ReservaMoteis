using BookMotelsDomain.Entities;
using BookMotelsDomain.Interfaces.Base;

namespace BookMotelsDomain.Interfaces
{
    public interface IReserveRepository : ICrudRepository<long, ReserveEntity>
    {
        Task<bool> HasConflictingReservation(long suiteId, DateTime checkIn, DateTime checkOut);
        Task<IEnumerable<ReserveEntity>> FindAllByUserAsync(Guid userId);
        Task<IEnumerable<BillingReportDTO>> FindBillingReport(long? motelId, int? year, int? month);

    }
}
