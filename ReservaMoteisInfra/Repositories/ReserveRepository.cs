using BookMotelsDomain.Entities;
using BookMotelsDomain.Interfaces;
using BookMotelsInfra.Repositories.Base;

namespace BookMotelsInfra.Repositories
{
    public class ReserveRepository : CrudRepository<long, ReserveEntity>, IReserveRepository
    {
    }
}
