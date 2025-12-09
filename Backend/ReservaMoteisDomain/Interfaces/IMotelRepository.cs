using BookMotelsDomain.Entities;
using BookMotelsDomain.Interfaces.Base;

namespace BookMotelsDomain.Interfaces
{
    public interface IMotelRepository : ICrudRepository<long, MotelEntity>
    {
    }
}
