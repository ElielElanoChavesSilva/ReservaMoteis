using BookMotelsDomain.Entities;
using BookMotelsDomain.Interfaces;
using BookMotelsInfra.Context;
using BookMotelsInfra.Repositories.Base;

namespace BookMotelsInfra.Repositories
{
    public class UserRepository : CrudRepository<Guid, UserEntity>, IUserRepository
    {
        public UserRepository(MainContext context) : base(context)
        {
        }
    }
}
