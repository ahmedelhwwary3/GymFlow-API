using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace RepositoryTier.User.Repositories
{
    public interface IUserRepository: IRepository<Entities.User>
    {
        Task<Entities.User?> GetByEmailAsync(string email,bool isTracked=true); 
        Task<Entities.User?> GetByPhoneAsync(string phone, bool isTracked=true);
        Task<int?> GetIdByIdentifierAsync(string identifier);
        Task<bool> ExistsByPhoneAsync(string phone);
        Task<bool> ExistsByEmailAsync(string email);
    }
}
