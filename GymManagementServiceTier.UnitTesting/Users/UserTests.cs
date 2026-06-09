using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using NUnit;
using NUnit.Framework;
using RepositoryTier.User.Repositories;
using ServiceTier.Configurations;
using RepositoryTier.Models;
using ServiceTier.User;
using System.Security.Cryptography;
using RepositoryTier.User.DTOs;
using RepositoryTier.User.Enums;

namespace GymManagementServiceTier.UnitTesting.Users
{
    [TestFixture]
    public class UserTests
    {
        private UserService _userService;
        private Mock<IUserRepository> _repoMock; 

        [OneTimeSetUp]
        public void SetUp() 
        {
            IConfiguration configs = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["GYM_SECRET_KEY"] = "8QmX4vN2AsL9rT7yHcD5w" +
                    "aaF2sHgE6uB3nYkR8qVxM2tP4iC7oW9lA5dSfG1hJ6z"
                })
                .Build();

            IOptions<JWTOptions> options = Options.Create(new JWTOptions()
            {
                Issuer = "Ahmed-Elhwwary",
                Audience = "Gym-Users",
                ExpirationInMinutes = 15,
                RefreshTokenExpirationInDays = 7
            });

            _repoMock=new Mock<IUserRepository>();

            _userService = new UserService(
                _repoMock.Object,
                options, 
                configs);
        }

        [OneTimeTearDown] 
        public void TearDown() { }

        protected void SetUpSaveChangesAsync(int affetedRows)
        {
            _repoMock.Setup(r => r.SaveChangesAsync())
                .ReturnsAsync(affetedRows);
        }

        protected void VerifySaveChangesAsync(int affetedRows)
        {
            _repoMock.Verify(r => r.SaveChangesAsync(),
                Times.Exactly(affetedRows));
        }

        protected string GenerateRefreshToken()
        {
            byte[] array = new byte[64];
           var rng= RandomNumberGenerator.Create();
            rng.GetBytes(array);
            return Convert.ToBase64String(array);
        }

        [Test]
        [Category("ChangePassword")]
        public async Task ChangePasswordAsync_ValidData_ReturnsSucceeded()
        {
            string refreshToken= GenerateRefreshToken();
            //1.Arrange
            _repoMock.Setup(r => r.FindByIdAsync(1))
                .ReturnsAsync(new User()
            {
                Id = 1,
                Email = "koko@yahoo.com",
                CreatedAt = DateTime.UtcNow,
                DeletedAt = null,
                UpdatedAt = null,
                IsActive = true,
                Gender = 1,
                FullName = "Ahmed Elhwwary",
                RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(5),
                RefreshTokenRevokedAt = null,
                RefreshTokenHash = BCrypt.Net.BCrypt.HashPassword(refreshToken),
                Role = 1,
                DateOfBirth = new DateOnly(1990, 1, 1),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("11112222"),
                IsDeleted = false,
                Phone = "1234567890"
            }); 
            var request = new ChangePasswordRequest()
            {
                CurrentPassword= "11112222",
                NewPassword="33334444",
                ConfirmPassword= "33334444"
            }; 
            SetUpSaveChangesAsync(1);

            //2.Act
            enChangePasswordStatus result = await _userService
                .ChangePasswordAsync(1, request);

            //3.Assert
            Assert.That(result, Is.EqualTo(enChangePasswordStatus.Succeeded));
            VerifySaveChangesAsync(1);
        }

        [Test]
        [Category("ChangePassword")]
        public async Task ChangePasswordAsync_OldPassword_ReturnsOldPassword()
        {
            string refreshToken = GenerateRefreshToken();
            //1.Arrange
            _repoMock.Setup(r => r.FindByIdAsync(1))
                .ReturnsAsync(new User()
                {
                    Id = 1,
                    Email = "koko@yahoo.com",
                    CreatedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    UpdatedAt = null,
                    IsActive = true,
                    Gender = 1,
                    FullName = "Ahmed Elhwwary",
                    RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(5),
                    RefreshTokenRevokedAt = null,
                    RefreshTokenHash = BCrypt.Net.BCrypt.HashPassword(refreshToken),
                    Role = 1,
                    DateOfBirth = new DateOnly(1990, 1, 1),
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("11112222"),
                    IsDeleted = false,
                    Phone = "1234567890"
                });
            var request = new ChangePasswordRequest()
            {
                CurrentPassword = "11112222",
                NewPassword = "11112222",
                ConfirmPassword = "11112222"
            };
            SetUpSaveChangesAsync(0);

            //2.Act
            enChangePasswordStatus result = await _userService
                .ChangePasswordAsync(1, request);

            //3.Assert
            Assert.That(result, Is.EqualTo(enChangePasswordStatus.OldPassword));
            VerifySaveChangesAsync(0);
        }
         
        [Test]
        [Category("ChangePassword")]
        public async Task ChangePasswordAsync_InvalidConfirmPassword_ReturnsInvalidConfirmPassword()
        {
            string refreshToken = GenerateRefreshToken();
            //1.Arrange
            _repoMock.Setup(r => r.FindByIdAsync(1))
                .ReturnsAsync(new User()
                {
                    Id = 1,
                    Email = "koko@yahoo.com",
                    CreatedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    UpdatedAt = null,
                    IsActive = true,
                    Gender = 1,
                    FullName = "Ahmed Elhwwary",
                    RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(5),
                    RefreshTokenRevokedAt = null,
                    RefreshTokenHash = BCrypt.Net.BCrypt.HashPassword(refreshToken),
                    Role = 1,
                    DateOfBirth = new DateOnly(1990, 1, 1),
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("11112222"),
                    IsDeleted = false,
                    Phone = "1234567890"
                });
            var request = new ChangePasswordRequest()
            {
                CurrentPassword = "11112222",
                NewPassword = "33334444",
                ConfirmPassword = "33334554"
            };
            SetUpSaveChangesAsync(0);

            //2.Act
            enChangePasswordStatus result = await _userService
                .ChangePasswordAsync(1, request);

            //3.Assert
            Assert.That(result, Is.EqualTo(enChangePasswordStatus.InvalidConfirmPassword));
            VerifySaveChangesAsync(0);
        }

        [Test]
        [Category("ChangePassword")]
        public async Task ChangePasswordAsync_UserNotFound_ReturnsUserNotFound()
        {
            string refreshToken = GenerateRefreshToken();
            //1.Arrange
            _repoMock.Setup(r => r.FindByIdAsync(1))
                .ReturnsAsync(new User()
                {
                    Id = 1,
                    Email = "koko@yahoo.com",
                    CreatedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    UpdatedAt = null,
                    IsActive = true,
                    Gender = 1,
                    FullName = "Ahmed Elhwwary",
                    RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(5),
                    RefreshTokenRevokedAt = null,
                    RefreshTokenHash = BCrypt.Net.BCrypt.HashPassword(refreshToken),
                    Role = 1,
                    DateOfBirth = new DateOnly(1990, 1, 1),
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("11112222"),
                    IsDeleted = false,
                    Phone = "1234567890"
                });
            var request = new ChangePasswordRequest()
            {
                CurrentPassword = "11112222",
                NewPassword = "33334444",
                ConfirmPassword = "33334444"
            };
            SetUpSaveChangesAsync(0);

            //2.Act
            enChangePasswordStatus result = await _userService
                .ChangePasswordAsync(5, request);

            //3.Assert
            Assert.That(result, Is.EqualTo(enChangePasswordStatus.UserNotFound));
            VerifySaveChangesAsync(0);
        }

        [Test]
        [Category("ChangePassword")]
        public async Task ChangePasswordAsync_InvalidCurrentPassword_ReturnsInvalidCurrentPassword()
        {
            string refreshToken = GenerateRefreshToken();
            //1.Arrange
            _repoMock.Setup(r => r.FindByIdAsync(1))
                .ReturnsAsync(new User()
                {
                    Id = 1,
                    Email = "koko@yahoo.com",
                    CreatedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    UpdatedAt = null,
                    IsActive = true,
                    Gender = 1,
                    FullName = "Ahmed Elhwwary",
                    RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(5),
                    RefreshTokenRevokedAt = null,
                    RefreshTokenHash = BCrypt.Net.BCrypt.HashPassword(refreshToken),
                    Role = 1,
                    DateOfBirth = new DateOnly(1990, 1, 1),
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("11112222"),
                    IsDeleted = false,
                    Phone = "1234567890"
                });
            var request = new ChangePasswordRequest()
            {
                CurrentPassword = "00002222",
                NewPassword = "33334444",
                ConfirmPassword = "33334444"
            };
            SetUpSaveChangesAsync(0);

            //2.Act
            enChangePasswordStatus result = await _userService
                .ChangePasswordAsync(1, request);

            //3.Assert
            Assert.That(result, Is.EqualTo(enChangePasswordStatus.InvalidCurrentPassword));
            VerifySaveChangesAsync(0);
        }

    }
}
