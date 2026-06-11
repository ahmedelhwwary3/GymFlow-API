using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace RepositoryTier.User.Repositories
{
    public class UserRepository : Repository<Entities.User>, IUserRepository
    {
        public UserRepository(GymManagementDbContext context) : base(context) { }

        public async Task<Entities.User?> GetByEmailAsync(string email)
        {
            var user = await _context.Users
                .SingleOrDefaultAsync(u => u.Email == email); 
            return user;
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<bool> ExistsByPhoneAsync(string phone)
        {
            return await _context.Users.AnyAsync(u => u.Phone == phone);
        }
    }
}
