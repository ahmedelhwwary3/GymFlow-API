using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using models = RepositoryTier.Models;

namespace RepositoryTier.Data.Repositories.User
{
    public interface IUserRepository: IRepository<models.User>
    {
        Task<models.User?> GetByEmailAsync(string email);
    }
}
