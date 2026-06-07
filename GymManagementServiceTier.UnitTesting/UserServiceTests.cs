using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using RepositoryTier.Data.Repositories.User;
using ServiceTier.Configurations;
using Microsoft.Extensions.Configuration;
using ServiceTier.User;
using RepositoryTier.DTOs.Authentication;
using BCrypt.Net;
using RepositoryTier.Models;

namespace GymManagementServiceTier.UnitTesting
{
    [TestFixture]
    public class UserServiceTests
    {
        private UserService _userService; // No specefic type here , we can use real object directly instead of interface
        // We used interfaces because CTOR requires it  
        private Mock<IUserRepository> _repoMock; 

        [OneTimeSetUp]
        public void SetUpOneTime()
        {
            // 1.Arrange

            // We can create a real copy of JWTOptions since it's a simple class,
            // but we will mock the repository and configuration (only interfaces
            var jwtOptions = Options.Create(
                 new JWTOptions
                 {
                     Issuer = "Ahmed-Elhwwary",
                     Audience = "Gym-Users",
                     ExpirationInMinutes = 15,
                     RefreshTokenExpirationInDays = 7
                 }); 

            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["GYM_SECRET_KEY"] = "8QmX4vN2AsL9rT7yHcD5w" +
                    "aaF2sHgE6uB3nYkR8qVxM2tP4iC7oW9lA5dSfG1hJ6z"
                })
                .Build();

            _repoMock=new Mock<IUserRepository>();

            _userService = new UserService(
                _repoMock.Object,
                jwtOptions,
                config);
              
        }
        [OneTimeTearDown]
        public void TearDownOneTime() { }

        [Test]
        [Category("Login")]
        public async Task TestLoginAsync_WithValidCredentials_ReturnsTokenResponse()
        {
            // 1.Arrange
            _repoMock
               .Setup(r => r.GetByEmailAsync("koko@yahoo.com"))
               .ReturnsAsync(new User
               {
                   Id = 1,
                   Email = "koko@yahoo.com",
                   CreatedAt = DateTime.UtcNow,
                   DeletedAt = null,
                   UpdatedAt = null,
                   IsActive = true,
                   Gender = 1,
                   FullName = "Ahmed Elhwwary",
                   RefreshTokenExpiresAt = null,
                   RefreshTokenRevokedAt = null,
                   RefreshTokenHash = null,
                   Role = 1,
                   DateOfBirth = new DateOnly(1990, 1, 1),
                   PasswordHash = BCrypt.Net.BCrypt.HashPassword("11112222"),
                   IsDeleted = false,
                   Phone = "1234567890"
               });
            //2.Act
            var response =await _userService.LoginAsync(new LoginRequest()
            {
                Email= "koko@yahoo.com",
                Password = "11112222"
            });
            //3.Assert
            Assert.That(response,Is.Not.Null);
        }

        [Test]
        [Category("Login")]
        public async Task TestLoginAsync_WithInvalidEmail_ReturnsNull()
        {
            // 1.Arrange
            _repoMock
               .Setup(r => r.GetByEmailAsync("medo@yahoo.com"))
               .ReturnsAsync(new User
               {
                   Id = 1,
                   Email = "koko@yahoo.com",
                   CreatedAt = DateTime.UtcNow,
                   DeletedAt = null,
                   UpdatedAt = null,
                   IsActive = true,
                   Gender = 1,
                   FullName = "Ahmed Elhwwary",
                   RefreshTokenExpiresAt = null,
                   RefreshTokenRevokedAt = null,
                   RefreshTokenHash = null,
                   Role = 1,
                   DateOfBirth = new DateOnly(1990, 1, 1),
                   PasswordHash = BCrypt.Net.BCrypt.HashPassword("11112222"),
                   IsDeleted = false,
                   Phone = "1234567890"
               });
            //2.Act
            var response = await _userService.LoginAsync(new LoginRequest()
            {
                Email = "koko@yahoo.com",
                Password = "11112222"
            });
            //3.Assert
            Assert.That(response, Is.Null);
        }

        [Test]
        [Category("Login")]
        public async Task TestLoginAsync_WithInvalidPassword_ReturnsNull()
        {
            // 1.Arrange
            _repoMock
               .Setup(r => r.GetByEmailAsync("koko@yahoo.com"))
               .ReturnsAsync(new User
               {
                   Id = 1,
                   Email = "koko@yahoo.com",
                   CreatedAt = DateTime.UtcNow,
                   DeletedAt = null,
                   UpdatedAt = null,
                   IsActive = true,
                   Gender = 1,
                   FullName = "Ahmed Elhwwary",
                   RefreshTokenExpiresAt = null,
                   RefreshTokenRevokedAt = null,
                   RefreshTokenHash = null,
                   Role = 1,
                   DateOfBirth = new DateOnly(1990, 1, 1),
                   PasswordHash = BCrypt.Net.BCrypt.HashPassword("wrongPassword"),
                   IsDeleted = false,
                   Phone = "1234567890"
               });
            //2.Act
            var response = await _userService.LoginAsync(new LoginRequest()
            {
                Email = "koko@yahoo.com",
                Password = "11112222"
            });
            //3.Assert
            Assert.That(response, Is.Null);
        }

        [Test]
        [Category("Login")]
        public async Task TestLoginAsync_InactiveUser_ReturnsNull()
        {
            // 1.Arrange
            _repoMock
               .Setup(r => r.GetByEmailAsync("koko@yahoo.com"))
               .ReturnsAsync(new User
               {
                   Id = 1,
                   Email = "koko@yahoo.com",
                   CreatedAt = DateTime.UtcNow,
                   DeletedAt = null,
                   UpdatedAt = null,
                   IsActive = false,
                   Gender = 1,
                   FullName = "Ahmed Elhwwary",
                   RefreshTokenExpiresAt = null,
                   RefreshTokenRevokedAt = null,
                   RefreshTokenHash = null,
                   Role = 1,
                   DateOfBirth = new DateOnly(1990, 1, 1),
                   PasswordHash = BCrypt.Net.BCrypt.HashPassword("11112222"),
                   IsDeleted = false,
                   Phone = "1234567890"
               });
            //2.Act
            var response = await _userService.LoginAsync(new LoginRequest()
            {
                Email = "koko@yahoo.com",
                Password = "11112222"
            });
            //3.Assert
            Assert.That(response, Is.Null);
        }

        [Test]
        [Category("Login")]
        public async Task TestLoginAsync_DeletedUser_ReturnsNull()
        {
            // 1.Arrange
            _repoMock
               .Setup(r => r.GetByEmailAsync("koko@yahoo.com"))
               .ReturnsAsync(new User
               {
                   Id = 1,
                   Email = "koko@yahoo.com",
                   CreatedAt = DateTime.UtcNow,
                   DeletedAt = DateTime.UtcNow.AddDays(-1),
                   UpdatedAt = null,
                   IsActive = true,
                   Gender = 1,
                   FullName = "Ahmed Elhwwary",
                   RefreshTokenExpiresAt = null,
                   RefreshTokenRevokedAt = null,
                   RefreshTokenHash = null,
                   Role = 1,
                   DateOfBirth = new DateOnly(1990, 1, 1),
                   PasswordHash = BCrypt.Net.BCrypt.HashPassword("11112222"),
                   IsDeleted = true,
                   Phone = "1234567890"
               });
            //2.Act
            var response = await _userService.LoginAsync(new LoginRequest()
            {
                Email = "koko@yahoo.com",
                Password = "11112222"
            });
            //3.Assert
            Assert.That(response, Is.Null);
        }
         
        [Test]
        [Category("Login")]
        public async Task TestLoginAsync_EmptyEmail_ReturnsNull()
        {
            // 1.Arrange
            _repoMock
               .Setup(r => r.GetByEmailAsync("koko@yahoo.com"))
               .ReturnsAsync(new User
               {
                   Id = 1,
                   Email = "koko@yahoo.com",
                   CreatedAt = DateTime.UtcNow,
                   DeletedAt = DateTime.UtcNow.AddDays(-1),
                   UpdatedAt = null,
                   IsActive = true,
                   Gender = 1,
                   FullName = "Ahmed Elhwwary",
                   RefreshTokenExpiresAt = null,
                   RefreshTokenRevokedAt = null,
                   RefreshTokenHash = null,
                   Role = 1,
                   DateOfBirth = new DateOnly(1990, 1, 1),
                   PasswordHash = BCrypt.Net.BCrypt.HashPassword("11112222"),
                   IsDeleted = true,
                   Phone = "1234567890"
               });
            //2.Act
            var response = await _userService.LoginAsync(new LoginRequest()
            {
                Email = "",
                Password = "11112222"
            });
            //3.Assert
            Assert.That(response, Is.Null);
        }

        [Test]
        [Category("Login")]
        public async Task TestLoginAsync_EmptyPassword_ReturnsNull()
        {
            // 1.Arrange
            _repoMock
               .Setup(r => r.GetByEmailAsync("koko@yahoo.com"))
               .ReturnsAsync(new User
               {
                   Id = 1,
                   Email = "koko@yahoo.com",
                   CreatedAt = DateTime.UtcNow,
                   DeletedAt = DateTime.UtcNow.AddDays(-1),
                   UpdatedAt = null,
                   IsActive = true,
                   Gender = 1,
                   FullName = "Ahmed Elhwwary",
                   RefreshTokenExpiresAt = null,
                   RefreshTokenRevokedAt = null,
                   RefreshTokenHash = null,
                   Role = 1,
                   DateOfBirth = new DateOnly(1990, 1, 1),
                   PasswordHash = BCrypt.Net.BCrypt.HashPassword("11112222"),
                   IsDeleted = true,
                   Phone = "1234567890"
               });
            //2.Act
            var response = await _userService.LoginAsync(new LoginRequest()
            {
                Email = "koko@yahoo.com",
                Password = ""
            });
            //3.Assert
            Assert.That(response, Is.Null);
        }

        [Test]
        [Category("Refresh")]
        public void TestRefreshAsync_WithValidTokenAndEmail_ReturnsTokenResponse()
        {

        }

        [Test]
        [Category("Logout")]
        public void TestLogoutAsync_WithValidTokenAndEmail_ReturnsOkWithMessage()
        {

        }




    }
}
