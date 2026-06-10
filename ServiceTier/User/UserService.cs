using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RepositoryTier.API_Configurations;
using RepositoryTier.User.DTOs;
using RepositoryTier.User.DTOs.Authentication;
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
        protected readonly IUserRepository _repo;
        protected readonly JWTOptions _jwtConfigs;
        protected readonly IConfiguration _configs;
        public UserService(
            IUserRepository repo,
            IOptions<JWTOptions> jwtOptions,
            IConfiguration configs)
            : base(repo)
        {
            _jwtConfigs = jwtOptions.Value;
            _repo = repo;
            _configs = configs;
            if (_jwtConfigs == null)
                throw new Exception("JWT options is not configured");
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

            if(user.IsDeleted)
                return new LoginResult(enLoginStatus.Deleted);

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
                new Claim(ClaimTypes.Role,user.Role.ToString())
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

            if (user.IsDeleted)
                return new RefreshResult(enRefreshStatus.Deleted);

            bool isUserRefreshToken = BCrypt.Net.BCrypt
                .Verify(request.RefreshToken.Trim(), user.RefreshTokenHash);
            if (!isUserRefreshToken ||
                user.RefreshTokenExpiresAt < DateTime.UtcNow ||
                user.RefreshTokenRevokedAt != null)
                return new RefreshResult(enRefreshStatus.InvalidPassword);
            // B) refresh token is valid - generate new tokens
            //1.access token
            var Claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Role,user.Role.ToString())
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

            var user = await _repo.FindByIdAsync(userId);
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
    }
}