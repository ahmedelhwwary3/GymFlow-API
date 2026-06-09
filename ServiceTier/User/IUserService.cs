using RepositoryTier.User.DTOs.Authentication;
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
        Task<TokenResponse?> LoginAsync(LoginRequest request);
        Task<TokenResponse?> RefreshAsync(RefreshRequest request);
        Task<bool> LogoutAsync(LogoutRequest request);
    }
}
