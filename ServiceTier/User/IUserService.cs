using RepositoryTier.DTOs.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using models = RepositoryTier.Models;
namespace ServiceTier
{
    public interface IUserService: IService<models.User>
    {
        Task<LoginResponse?> LoginAsync(LoginRequest request);
    }
}
