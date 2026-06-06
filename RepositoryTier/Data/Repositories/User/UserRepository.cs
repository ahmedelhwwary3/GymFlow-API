using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.Data.Repositories.User
{
    internal class UserRepository : Repository<GymManagementAPI.Models.User>, IUserRepository
    {
        public UserRepository(GymManagementDbContext context) : base(context) { }
    }
}
