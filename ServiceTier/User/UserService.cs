using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using RepositoryTier.Data.Repositories;
using RepositoryTier.Data.Repositories.User;
using RepositoryTier.DTOs.Authentication; 
using ServiceTier.Configurations;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using models = RepositoryTier.Models;

namespace ServiceTier.User
{
    public class UserService : Service<models.User>, IUserService
    {
        private readonly IUserRepository _repo;
        private readonly JWTOptions _jwt;
        private readonly IConfiguration _configs;
        public UserService(
            IUserRepository repo,
            IOptions<JWTOptions> jwtOptions,
            IConfiguration configs)
            : base(repo)
        {
            _jwt = jwtOptions.Value;
            _repo = repo;
            _configs = configs;
        }

        private string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            // create cryptographic object to generate secure random numbers
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes); // fill the byte array with random bytes
                return Convert.ToBase64String(randomBytes);
            }
        }
        public async Task<TokenResponse?> LoginAsync(LoginRequest request)
        {
            // A) validate login
            var user = await _repo.GetByEmailAsync(request.Email);
            if (user == null)
                return null;

            bool isValidPassword = BCrypt.Net.BCrypt
                .Verify("password", user.PasswordHash);
            if (!isValidPassword)
                return null;
            // B) login is valid - generate tokens
            // 1.access token
            var Claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Role,user.Role.ToString())
            };
            string key = _configs["JWT_SECRET_KEY"] ??
                throw new Exception("JWT_SECRET key is not configured");

            var creds = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256);

            var accessToken = new JwtSecurityToken(
                claims: Claims,
                expires: DateTime.UtcNow.AddMinutes(_jwt.ExpirationInMinutes),
                audience: _jwt.Audience,
                issuer: _jwt.Issuer,
                signingCredentials: creds);
            // 2.save new refresh token and revoke the old one
            var refreshToken = GenerateRefreshToken();
            user.RefreshTokenRevokedAt = null;
            user.RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(_jwt.RefreshTokenExpirationInDays);
            user.RefreshTokenHash = BCrypt.Net.BCrypt.HashPassword(refreshToken);// logout  
            int affectedRows= await _repo.SaveChangesAsync();

            return new TokenResponse()
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
                RefreshToken = refreshToken
            };
        }

        public async Task<TokenResponse?> RefreshAsync(RefreshRequest request)
        {
            // A) validate refresh token
            var user = await _repo.GetByEmailAsync(request.Email);
            if (user == null)
                return null;

            bool isUserRefreshToken = BCrypt.Net.BCrypt
                .Verify(request.RefreshToken, user.RefreshTokenHash);
            if (!isUserRefreshToken ||
                user.RefreshTokenExpiresAt > DateTime.UtcNow ||
                user.RefreshTokenRevokedAt != null)
                return null;
            // B) refresh token is valid - generate new tokens
            //1.access token
            var Claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Role,user.Role.ToString())
            };
            string key = _configs["JWT_SECRET_KEY"] ??
                throw new Exception("JWT_SECRET key is not configured");

            var creds = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256);
            var newAccessToken = new JwtSecurityToken(
                claims: Claims,
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                expires: DateTime.UtcNow.AddMinutes(_jwt.ExpirationInMinutes),
                signingCredentials: creds
                );
            //2.save new refresh token and revoke the old one
            var newRefreshToken = GenerateRefreshToken(); 
            user.RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(_jwt.RefreshTokenExpirationInDays);
            user.RefreshTokenHash = BCrypt.Net.BCrypt.HashPassword(newRefreshToken);
            user.RefreshTokenRevokedAt = null;
            int affectedRows = await _repo.SaveChangesAsync();

            return new TokenResponse()
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                RefreshToken = newRefreshToken
            };
        }

        public async Task<bool> LogoutAsync(LogoutRequest request)
        {
            // A) validate refresh token
            var user = await _repo.GetByEmailAsync(request.Email);
            if(user == null)
                return false;

            bool isUserRefreshToken = BCrypt.Net.BCrypt
                .Verify(request.RefreshToken, user.RefreshTokenHash);
            if(!isUserRefreshToken || user.RefreshTokenExpiresAt > DateTime.UtcNow ||
                user.RefreshTokenRevokedAt != null)
                return false;
            // B) refresh token is valid - revoke it
            user.RefreshTokenRevokedAt = DateTime.UtcNow;
            user.RefreshTokenExpiresAt = null;
            int affectedRows = await _repo.SaveChangesAsync();
            return true;
        }
    }
}