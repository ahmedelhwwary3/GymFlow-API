using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RepositoryTier.API_Configurations;
using RepositoryTier.Authentication;
using RepositoryTier.Coach.Enums;
using RepositoryTier.Coach.Repositories;
using RepositoryTier.User.DTOs; 
using RepositoryTier.User.Enums;
using RepositoryTier.User.Repositories;
using RepositoryTier.User.Results; 
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks; 
namespace ServiceTier.User
{
    public class UserService : Service<RepositoryTier.Entities.User>, IUserService
    {
        private readonly IUserRepository _repo;
        protected readonly JWTOptions _jwtConfigs;
        protected readonly IConfiguration _configs;
        private readonly ICoachRepository _coachRepo;
        public UserService(
            IUserRepository repo,
            ICoachRepository coachRepo,
            IOptions<JWTOptions> jwtOptions,
            IConfiguration configs)
            : base(repo)
        {
            _jwtConfigs = jwtOptions.Value;
            _repo = repo;
            _configs = configs;
            _coachRepo= coachRepo;
            if (_jwtConfigs == null)
                throw new Exception("JWT options is not configured");
        }

        public async Task<Boolean> IsUniquePhoneAsync(string phone, int userId = 0)
        {
            if (userId > 0) //Update
            {
                string? userPhone = await _repo.GetPhoneByIdAsync(userId);
                if (string.IsNullOrEmpty(phone.Trim()))
                    return false;//NotFound (Error)
                //Same user
                if (userPhone == phone.Trim())
                    return true;
            }
            //Add
            return !await _repo.ExistsByPhoneAsync(phone.Trim());
        }

        public async Task<Boolean> IsUniqueEmailAsync(string email, int userId = 0)
        {
            if (userId > 0) //Update
            {
                string? userEmail = await _repo.GetEmailByIdAsync(userId);
                if (string.IsNullOrEmpty(userEmail))
                    return false;

                //Same user
                if (userEmail == email.Trim())
                    return true;
            }
            //Add
            return !await _repo.ExistsByEmailAsync(email.Trim());
        }

        protected string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            // create cryptographic object to generate secure random numbers
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes); // fill the byte array with random bytes
                return Convert.ToBase64String(randomBytes);
            }
        }
        public async Task<LoginResult> LoginAsync(LoginRequest request)
        {
            // A) validate login
            var user = await _repo.GetByEmailAsync(request.Email.Trim());
            if (user == null)
                return new LoginResult(enLoginStatus.UserNotFound);

            if (!user.IsActive)
                return new LoginResult(enLoginStatus.Inactive); 

            bool isValidPassword = BCrypt.Net.BCrypt
                .Verify(request.Password.Trim(), user.PasswordHash);
            if (!isValidPassword)
                return new LoginResult(enLoginStatus.InvalidPassword);
            // B) login is valid - generate tokens
            // 1.access token
            var Claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Role,$"{(int)user.Role}")
            };
            string key = _configs["GYM_SECRET_KEY"] ??
                throw new Exception("GYM_SECRET key is not configured");

            var creds = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256);

            var accessToken = new JwtSecurityToken(
                claims: Claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtConfigs.ExpirationInMinutes),
                audience: _jwtConfigs.Audience,
                issuer: _jwtConfigs.Issuer,
                signingCredentials: creds);
            // 2.save new refresh token and revoke the old one
            var refreshToken = GenerateRefreshToken();
            user.RefreshTokenRevokedAt = null;
            user.RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(_jwtConfigs.RefreshTokenExpirationInDays);
            user.RefreshTokenHash = BCrypt.Net.BCrypt.HashPassword(refreshToken);// logout  
            int affectedRows= await _repo.SaveChangesAsync();

            var tokenResonse= new TokenResponse()
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
                RefreshToken = refreshToken
            };
            return new LoginResult(enLoginStatus.Succeeded,tokenResonse);
        }

        public async Task<RefreshResult> RefreshAsync(RefreshRequest request)
        {
            // A) validate refresh token
            var user = await _repo.GetByEmailAsync(request.Email.Trim());
            if (user == null)
                return new RefreshResult(enRefreshStatus.UserNotFound);

            if (!user.IsActive) 
                return new RefreshResult(enRefreshStatus.Inactive); 

            bool isUserRefreshToken = BCrypt.Net.BCrypt
                .Verify(request.RefreshToken.Trim(), user.RefreshTokenHash);
            if (!isUserRefreshToken ||
                user.RefreshTokenExpiresAt < DateTime.UtcNow ||
                user.RefreshTokenRevokedAt != null)
                return new RefreshResult(enRefreshStatus.InvalidToken);
            // B) refresh token is valid - generate new tokens
            //1.access token
            var Claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Role,$"{(int)user.Role}")
            };
            string key = _configs["GYM_SECRET_KEY"] ??
                throw new Exception("GYM_SECRET key is not configured");

            var creds = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256);
            var newAccessToken = new JwtSecurityToken(
                claims: Claims,
                issuer: _jwtConfigs.Issuer,
                audience: _jwtConfigs.Audience,
                expires: DateTime.UtcNow.AddMinutes(_jwtConfigs.ExpirationInMinutes),
                signingCredentials: creds
                );
            //2.save new refresh token and revoke the old one
            var newRefreshToken = GenerateRefreshToken(); 
            user.RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(_jwtConfigs.RefreshTokenExpirationInDays);
            user.RefreshTokenHash = BCrypt.Net.BCrypt.HashPassword(newRefreshToken);
            user.RefreshTokenRevokedAt = null;
            int affectedRows = await _repo.SaveChangesAsync();

            var tokenResponse= new TokenResponse()
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                RefreshToken = newRefreshToken
            };
            return new RefreshResult(enRefreshStatus.Succeeded,tokenResponse);
        }

        public async Task<bool> LogoutAsync(LogoutRequest request)
        {
            // A) validate refresh token
            var user = await _repo.GetByEmailAsync(request.Email);
            if(user == null)
                return false;

            if(user.IsDeleted || !user.IsActive)
                return false;

            bool isUserRefreshToken = BCrypt.Net.BCrypt
                .Verify(request.RefreshToken, user.RefreshTokenHash);
            if(!isUserRefreshToken || user.RefreshTokenExpiresAt < DateTime.UtcNow ||
                user.RefreshTokenRevokedAt != null)
                return false;

            // B) refresh token is valid - revoke it
            user.RefreshTokenRevokedAt = DateTime.UtcNow;
            user.RefreshTokenExpiresAt = null;
            int affectedRows = await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<enChangePasswordStatus>
            ChangePasswordAsync(int userId,ChangePasswordRequest request)
        {
            if (request.ConfirmPassword.Trim() != request.NewPassword.Trim())
                return enChangePasswordStatus.InvalidConfirmPassword;

            var user = await _repo.FindAsync(userId);
            if (user == null)
                return enChangePasswordStatus.UserNotFound; 

            bool isValidPassword = BCrypt.Net.BCrypt
                .Verify(request.CurrentPassword.Trim(), user.PasswordHash);

            if (!isValidPassword)
                return enChangePasswordStatus.InvalidCurrentPassword;

            if (request.NewPassword.Trim() == request.CurrentPassword.Trim())
                return enChangePasswordStatus.OldPassword;

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword.Trim());
            user.RefreshTokenHash = null;
            user.RefreshTokenRevokedAt = DateTime.UtcNow; 
            int affectedRows = await _repo.SaveChangesAsync();

            return enChangePasswordStatus.Succeeded;
        }

        public async Task<bool> ExistsByPhoneAsync(string phone)
        {
            return await _repo.ExistsByPhoneAsync(phone);
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _repo.ExistsByEmailAsync(email);
        }

        public async Task<RegisterCoachResult> 
            RegitserCoachAsync(RegisterCoachRequest request)
        {
            bool isUniqueEmail = await IsUniqueEmailAsync(request.Email);
            bool isUniquePhone = await IsUniquePhoneAsync(request.Phone);

            if (!isUniqueEmail)
                return new RegisterCoachResult(enRegisterCoachStatus.NotUniqueEmail);

            if (!isUniquePhone)
                return new RegisterCoachResult(enRegisterCoachStatus.NotUniqueEmail);

            var newCoach = new RepositoryTier.Entities.Coach()
            {
                CreatedAt = DateTime.UtcNow,
                DateOfBirth = request.DateOfBirth,
                Email = request.Email.Trim(),
                FullName = request.FullName.Trim(),
                Gender = request.Gender,
                HireDate = DateOnly.FromDateTime(DateTime.UtcNow),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Phone = request.Phone.Trim(),
                Role = enUserRole.Coach,
                Salary = request.Salary,
                Specialization = request.Specialization
            };

            await _repo.AddAsync(newCoach);
            int affectedRows = await _repo.SaveChangesAsync();

            var response = new RegisterCoachResponse()
            {
                HireDate = newCoach.HireDate,
                Salary = newCoach.Salary,
                Id = newCoach.Id,
                Specialization = newCoach.Specialization
            };
            return new RegisterCoachResult(enRegisterCoachStatus.Succeeded, response);
        }

        public async Task<RegisterMemberResult>
            RegitserMemberAsync(RegisterMemberRequest request)
        {
            //1. Unique Email & Phone
            bool isUniqueEmail = await IsUniqueEmailAsync(request.Email);
            bool isUniquePhone = await IsUniquePhoneAsync(request.Phone);

            if (!isUniqueEmail)
                return new RegisterMemberResult(enRegisterMemberStatus.NotUniqueEmail);

            if (!isUniquePhone)
                return new RegisterMemberResult(enRegisterMemberStatus.NotUniquePhone);

            //2.coach exists and Active
            bool coachExists = await _coachRepo.ExistsAsync(request.CoachId);
            if (!coachExists)
                return new RegisterMemberResult(enRegisterMemberStatus.CoachNotExists);

            bool isActiveCoach = await _coachRepo.IsActiveByIdAsync(request.CoachId);
            if (!isActiveCoach)
                return new RegisterMemberResult(enRegisterMemberStatus.CoachInactive);

            var newMember = new RepositoryTier.Entities.Member()
            {
                IsActive = true,
                Address = request.Address.Trim(),
                CreatedAt = DateTime.UtcNow,
                CoachId = request.CoachId,
                DateOfBirth = request.DateOfBirth,
                Email = request.Email.Trim(),
                FitnessGoal = request.FitnessGoal,
                FullName = request.FullName.Trim(),
                Gender = request.Gender,
                Height = request.Height,
                Role = enUserRole.Member,
                Phone = request.Phone.Trim(),
                IsDeleted = false,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };
            //3.Add and save
            await _repo.AddAsync(newMember);
            int affectedRows = await _repo.SaveChangesAsync();
            return new RegisterMemberResult(enRegisterMemberStatus.Succeeded, newMember.Id);
        }

        public async Task<RegisterAdminResult>
            RegitserAdminAsync(RegisterAdminRequest request)
        {
            //1. Unique Email & Phone
            bool isUniqueEmail = await IsUniqueEmailAsync(request.Email); 
            bool isUniquePhone = await IsUniquePhoneAsync(request.Phone);

            if (!isUniqueEmail)
                return new RegisterAdminResult(enRegisterAdminStatus.NotUniqueEmail);

            if (!isUniquePhone)
                return new RegisterAdminResult(enRegisterAdminStatus.NotUniquePhone); 

            var newMember = new RepositoryTier.Entities.User()
            {
                IsActive = true ,
                CreatedAt = DateTime.UtcNow,  
                Email = request.Email.Trim(),  
                Role = enUserRole.Admin,
                Phone = request.Phone.Trim(),
                IsDeleted = false,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password), 
                FullName= request.FullName.Trim(), 
                Gender=request.Gender
            };
            //2.Add and save
            await _repo.AddAsync(newMember);
            int affectedRows = await _repo.SaveChangesAsync();
            return new RegisterAdminResult(enRegisterAdminStatus.Succeeded, newMember.Id);
        }

        public async Task<GetUserByIdResponse?> GetUserByIdAsync(int Id)
        {
            return await _repo.GetUserByIdAsync(Id);
        }

        public async Task<enUpdateUserStatus> UpdateUserByIdAsync(int Id, UpdateUserRequest request)
        {
            //1.Unique Email & Phone
            bool isUniqueEmail = await IsUniqueEmailAsync(request.Email, Id);
            bool isUniquePhone = await IsUniquePhoneAsync(request.Phone, Id);

            if (!isUniqueEmail)
                return enUpdateUserStatus.NotUniqueEmail;

            if (!isUniquePhone)
                return enUpdateUserStatus.NotUniquePhone;// 

            //2.Load then Update strategy for efCore Tracking
            var user = await _repo.FindAsync(Id);
            if (user == null)
                return enUpdateUserStatus.UserNotFound;  

            user.Phone = request.Phone.Trim();
            user.Email = request.Email.Trim();
            user.FullName = request.FullName.Trim();
            user.DateOfBirth = request.DateOfBirth;
            user.Gender = request.Gender;
            user.Role = request.Role;
            user.IsActive = request.IsActive;
             
            EntityState state = _repo.GetEntityState(user);
            if (state == EntityState.Unchanged)
                return enUpdateUserStatus.DataNotChanged;
            user.UpdatedAt = DateTime.UtcNow;

            //3.Save
            int affectedRows = await _repo.SaveChangesAsync();
            return enUpdateUserStatus.Succeeded;
        }
    }
}