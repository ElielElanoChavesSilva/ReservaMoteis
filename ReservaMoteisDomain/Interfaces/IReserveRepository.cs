using BookMotelsDomain.Entities;
using BookMotelsDomain.Interfaces.Base;

namespace BookMotelsDomain.Interfaces
{
    public interface IReserveRepository : ICrudRepository<long, ReserveEntity>
    {
    }
}
