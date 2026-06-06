using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.Data.Repositories.User
{
    public class UserRepository : Repository<RepositoryTier.Models.User>, IUserRepository
    {
        public UserRepository(GymManagementDbContext context) : base(context) { }
    }
}
