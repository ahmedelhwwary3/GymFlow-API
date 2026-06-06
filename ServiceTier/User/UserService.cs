using RepositoryTier.Data.Repositories;
using RepositoryTier.Data.Repositories.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceTier.User
{
    public class UserService : Service<RepositoryTier.Models.User>, IUserService
    {
        public UserService(IUserRepository repo) : base(repo) { }
    }
}
