using BookMotelsDomain.Entities;
using BookMotelsDomain.Interfaces;
using BookMotelsInfra.Context;
using BookMotelsInfra.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace BookMotelsInfra.Repositories
{
    public class UserRepository : CrudRepository<Guid, UserEntity>, IUserRepository
    {
        public UserRepository(MainContext context) : base(context)
        {
        }

        public async Task<UserEntity?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .Include(x => x.Profile)
                .FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
