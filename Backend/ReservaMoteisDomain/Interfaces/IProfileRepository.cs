using BookMotelsDomain.Entities;
using BookMotelsDomain.Interfaces.Base;

namespace BookMotelsDomain.Interfaces
{
    public interface IProfileRepository : ICrudRepository<int, ProfileEntity>
    {
    }
}
