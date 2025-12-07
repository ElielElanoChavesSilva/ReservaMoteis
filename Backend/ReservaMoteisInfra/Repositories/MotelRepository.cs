using BookMotelsDomain.Entities;
using BookMotelsDomain.Interfaces;
using BookMotelsInfra.Context;
using BookMotelsInfra.Repositories.Base;

namespace BookMotelsInfra.Repositories
{
    public class MotelRepository : CrudRepository<long, MotelEntity>, IMotelRepository
    {
        public MotelRepository(MainContext context) : base(context)
        {
        }
    }
}
