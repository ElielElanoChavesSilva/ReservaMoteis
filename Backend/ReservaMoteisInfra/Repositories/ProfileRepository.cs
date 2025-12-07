using BookMotelsDomain.Entities;
using BookMotelsDomain.Interfaces;
using BookMotelsInfra.Context;
using BookMotelsInfra.Repositories.Base;

namespace BookMotelsInfra.Repositories
{
    public class ProfileRepository : CrudRepository<int, ProfileEntity>, IProfileRepository
    {
        public ProfileRepository(MainContext context) : base(context)
        {
        }
    }
}
