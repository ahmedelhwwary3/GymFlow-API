using RepositoryTier.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceTier.User
{
    public class UserService : Service<RepositoryTier.Models.User>, IUserService
    {
        public UserService(IRepository<RepositoryTier.Models.User> repo) : base(repo) { }
    }
}
