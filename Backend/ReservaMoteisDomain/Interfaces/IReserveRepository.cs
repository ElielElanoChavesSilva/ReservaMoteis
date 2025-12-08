using BookMotelsDomain.Entities;
using BookMotelsDomain.Interfaces.Base;

namespace BookMotelsDomain.Interfaces
{
    public interface IReserveRepository : ICrudRepository<long, ReserveEntity>
    {
        Task<bool> HasConflictingReservation(long suiteId, DateTime checkIn, DateTime checkOut);
    }
}
