using BookMotelsDomain.Entities;
using BookMotelsDomain.Interfaces.Base;

namespace BookMotelsDomain.Interfaces
{
    public interface IUserRepository : ICrudRepository<long, UserEntity>
    {
    }
}
