 
using RepositoryTier.DTOs.Users;
using RepositoryTier.Enums.Users;
using RepositoryTier.User.DTOs.Authentication;  
using models = RepositoryTier.Models;

namespace ServiceTier
{
    public interface IUserService: IService<models.User>
    {
        Task<TokenResponse?> LoginAsync(LoginRequest request);
        Task<TokenResponse?> RefreshAsync(RefreshRequest request);
        Task<bool> LogoutAsync(LogoutRequest request);
        Task<enChangePasswordResult> ChangePasswordAsync(ChangePasswordRequest request);
    }
}
