using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace RepositoryTier.User.Repositories
{
    public interface IUserRepository: IRepository<Entities.User>
    {
        Task<Entities.User?> GetByEmailAsync(string email);
    }
}
