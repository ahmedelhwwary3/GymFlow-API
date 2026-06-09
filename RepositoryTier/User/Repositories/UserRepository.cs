using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using models = RepositoryTier.Models;

namespace RepositoryTier.User.Repositories
{
    public class UserRepository : Repository<models.User>, IUserRepository
    {
        public UserRepository(GymManagementDbContext context) : base(context) { }

        public async Task<models.User?> GetByEmailAsync(string email)
        {
            var user= await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
            return user;
        }
    }
}
