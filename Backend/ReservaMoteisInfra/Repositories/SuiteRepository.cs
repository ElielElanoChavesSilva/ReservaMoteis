using BookMotelsDomain.Entities;
using BookMotelsDomain.Interfaces;
using BookMotelsInfra.Context;
using BookMotelsInfra.Repositories.Base;

namespace BookMotelsInfra.Repositories
{
    public class SuiteRepository : CrudRepository<long, SuiteEntity>, ISuiteRepository
    {
        public SuiteRepository(MainContext context) : base(context)
        {
        }
    }
}
