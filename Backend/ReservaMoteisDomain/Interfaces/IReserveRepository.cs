using BookMotelsDomain.Entities;
using BookMotelsDomain.Interfaces.Base;
using BookMotelsDomain.Projections;

namespace BookMotelsDomain.Interfaces
{
    public interface IReserveRepository : ICrudRepository<long, ReserveEntity>
    {
        Task<bool> HasConflictingReservation(long suiteId, DateTime checkIn, DateTime checkOut);
        Task<IEnumerable<GetReserveProjection>> FindAllByUser(Guid userId);
        Task<IEnumerable<GetReserveProjection>> FindAllProjection();
        Task<GetReserveProjection> FindByIdProjection(long id);
        Task<IEnumerable<BillingReportProjection>> FindBillingReport(long? motelId, int? year, int? month);
    }
}
