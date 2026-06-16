using RepositoryTier.Member.DTOs;
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
        Task<bool> ExistsByPhoneAsync(string phone);
        Task<bool> ExistsByEmailAsync(string phone);
        Task<Boolean> IsUniquePhoneAsync(string phone, int userId = 0);
        Task<Boolean> IsUniqueEmailAsync(string email, int userId = 0);
        Task<RegisterMemberResult> RegitserMemberAsync(RegisterMemberRequest request);
        Task<RegisterCoachResult> RegitserCoachAsync(RegisterCoachRequest request);
        Task<RegisterAdminResult> RegitserAdminAsync(RegisterAdminRequest request);
        Task<GetUserByIdResponse?> GetUserByIdAsync(int Id);
        Task<enUpdateUserStatus> UpdateUserByIdAsync(int Id,UpdateUserRequest request);

    }
}
