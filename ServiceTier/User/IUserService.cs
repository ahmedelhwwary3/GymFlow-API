using RepositoryTier.User.DTOs;
using RepositoryTier.User.DTOs.Authentication;
using RepositoryTier.User.Enums;
using RepositoryTier.User.Results; 

namespace ServiceTier.User
{
    public interface IUserService: IService<RepositoryTier.Entities.User>
    {
        Task<LoginResult> LoginAsync(LoginRequest request);
        Task<RefreshResult> RefreshAsync(RefreshRequest request);
        Task<bool> LogoutAsync(LogoutRequest request);
        Task<enChangePasswordStatus> ChangePasswordAsync(int userId,ChangePasswordRequest request);
    }
}
