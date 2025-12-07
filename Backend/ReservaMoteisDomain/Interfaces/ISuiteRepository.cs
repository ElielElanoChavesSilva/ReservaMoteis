using BookMotelsDomain.Entities;
using BookMotelsDomain.Interfaces.Base;

namespace BookMotelsDomain.Interfaces
{
    public interface ISuiteRepository : ICrudRepository<long, SuiteEntity>
    {
    }
}
