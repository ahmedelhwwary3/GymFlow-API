using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using RepositoryTier.Models;
using RepositoryTier.User.DTOs.Authentication;
using RepositoryTier.User.Repositories;
using ServiceTier;
using ServiceTier.Configurations;
using ServiceTier.User;
using System.Security.Cryptography; 

namespace GymManagementServiceTier.UnitTesting.Users
{
    [TestFixture]
    public class AuthenticationTests
    {
        private UserService _userService; // No specefic type here , we can use real object directly instead of interface
        // We used interfaces because CTOR requires it  
        private Mock<IUserRepository> _repoMock; 

        [OneTimeSetUp]
        protected void SetUpOneTime()
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
        protected void TearDownOneTime() { }
        
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

        [Test]
        [Category("Login")]
        public async Task TestLoginAsync_ValidCredentials_ReturnsTokenResponse()
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
            SetUpSaveChangesAsync(1);

            //2.Act
            var response =await _userService.LoginAsync(new LoginRequest()
            {
                Email= "koko@yahoo.com",
                Password = "11112222"
            });
            //3.Assert
            Assert.That(response,Is.Not.Null);
            VerifySaveChangesAsync(1);
        }

        [Test]
        [Category("Login")]
        public async Task TestLoginAsync_InvalidEmail_ReturnsNull()
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
            SetUpSaveChangesAsync(0);
            //2.Act
            var response = await _userService.LoginAsync(new LoginRequest()
            {
                Email = "koko@yahoo.com",
                Password = "11112222"
            });
            //3.Assert
            Assert.That(response, Is.Null);
            VerifySaveChangesAsync(0);
        }

        [Test]
        [Category("Login")]
        public async Task TestLoginAsync_InvalidPassword_ReturnsNull()
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
            SetUpSaveChangesAsync(0);
            //2.Act
            var response = await _userService.LoginAsync(new LoginRequest()
            {
                Email = "koko@yahoo.com",
                Password = "11112222"
            });
            //3.Assert
            Assert.That(response, Is.Null);
            VerifySaveChangesAsync(0);
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
            SetUpSaveChangesAsync(0);
            //2.Act
            var response = await _userService.LoginAsync(new LoginRequest()
            {
                Email = "koko@yahoo.com",
                Password = "11112222"
            });
            //3.Assert
            Assert.That(response, Is.Null);
            VerifySaveChangesAsync(0);
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
            SetUpSaveChangesAsync(0);
            //2.Act
            var response = await _userService.LoginAsync(new LoginRequest()
            {
                Email = "koko@yahoo.com",
                Password = "11112222"
            });
            //3.Assert
            Assert.That(response, Is.Null);
            VerifySaveChangesAsync(0);
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
            SetUpSaveChangesAsync(0);
            //2.Act
            var response = await _userService.LoginAsync(new LoginRequest()
            {
                Email = "",
                Password = "11112222"
            });
            //3.Assert
            Assert.That(response, Is.Null);
            VerifySaveChangesAsync(0);
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
            SetUpSaveChangesAsync(0);
            //2.Act
            var response = await _userService.LoginAsync(new LoginRequest()
            {
                Email = "koko@yahoo.com",
                Password = ""
            });
            //3.Assert
            Assert.That(response, Is.Null);
            VerifySaveChangesAsync(0);
        }

        //=======================

        private string GenerateRefreshToken()
        {
            var bytes = new byte[64];
            var rng = RandomNumberGenerator.Create(); // crypto object
            rng.GetBytes(bytes);//Fill array  random bytes
            return Convert.ToBase64String(bytes);
        }
        [Test]
        [Category("Refresh")]
        public async Task TestRefreshAsync_ValidTokenAndEmail_ReturnsTokenResponse()
        {
            string refreshToken = GenerateRefreshToken();
            if (string.IsNullOrEmpty(refreshToken))
                return;
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
                   RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(5),
                   RefreshTokenRevokedAt = null,
                   RefreshTokenHash = BCrypt.Net.BCrypt.HashPassword(refreshToken),
                   Role = 1,
                   DateOfBirth = new DateOnly(1990, 1, 1),
                   PasswordHash = BCrypt.Net.BCrypt.HashPassword("11112222"),
                   IsDeleted = false,
                   Phone = "1234567890"
               });
            SetUpSaveChangesAsync(1);
            //2.Act
            var response =await _userService.RefreshAsync(new RefreshRequest()
            {
                Email= "koko@yahoo.com",
                RefreshToken= refreshToken
            });

            //3.Assert
            Assert.That(response, Is.Not.Null);
            VerifySaveChangesAsync(1);
        }

        [Test]
        [Category("Refresh")]
        public async Task TestRefreshAsync_InvalidToken_ReturnsNull()
        {
            string refreshToken = GenerateRefreshToken();
            if (string.IsNullOrEmpty(refreshToken))
                return;

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
                   RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(5),
                   RefreshTokenRevokedAt = null,
                   RefreshTokenHash = BCrypt.Net.BCrypt.HashPassword(refreshToken),
                   Role = 1,
                   DateOfBirth = new DateOnly(1990, 1, 1),
                   PasswordHash = BCrypt.Net.BCrypt.HashPassword("11112222"),
                   IsDeleted = false,
                   Phone = "1234567890"
               });
            SetUpSaveChangesAsync(0);
            //2.Act
            var response = await _userService.RefreshAsync(new RefreshRequest()
            {
                Email = "koko@yahoo.com",
                RefreshToken = "Invalid Token" 
            });

            //3.Assert
            Assert.That(response, Is.Null);
            VerifySaveChangesAsync(0);
        }

        [Test]
        [Category("Refresh")]
        public async Task TestRefreshAsync_InvalidEmail_ReturnsNull()
        {
            string refreshToken = GenerateRefreshToken();
            if (string.IsNullOrEmpty(refreshToken))
                return;

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
                   RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(5),
                   RefreshTokenRevokedAt = null,
                   RefreshTokenHash = BCrypt.Net.BCrypt.HashPassword(refreshToken),
                   Role = 1,
                   DateOfBirth = new DateOnly(1990, 1, 1),
                   PasswordHash = BCrypt.Net.BCrypt.HashPassword("11112222"),
                   IsDeleted = false,
                   Phone = "1234567890"
               });
            SetUpSaveChangesAsync(0);
            //2.Act
            var response =await _userService.RefreshAsync(new RefreshRequest()
            {
                Email = "medo@yahoo.com",
                RefreshToken = refreshToken
            });

            //3.Assert
            Assert.That(response, Is.Null);
            VerifySaveChangesAsync(0);
        }

        [Test]
        [Category("Refresh")]
        public async Task TestRefreshAsync_EmptyEmail_ReturnsNull()
        {
            string refreshToken = GenerateRefreshToken();
            if (string.IsNullOrEmpty(refreshToken))
                return;

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
                   RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(5),
                   RefreshTokenRevokedAt = null,
                   RefreshTokenHash = BCrypt.Net.BCrypt.HashPassword(refreshToken),
                   Role = 1,
                   DateOfBirth = new DateOnly(1990, 1, 1),
                   PasswordHash = BCrypt.Net.BCrypt.HashPassword("11112222"),
                   IsDeleted = false,
                   Phone = "1234567890"
               });
            SetUpSaveChangesAsync(0);
            //2.Act
            var response =await _userService.RefreshAsync(new RefreshRequest()
            {
                Email = string.Empty,
                RefreshToken = refreshToken
            });

            //3.Assert
            Assert.That(response, Is.Null);
            VerifySaveChangesAsync(0);
        }

        [Test]
        [Category("Refresh")]
        public async Task TestRefreshAsync_EmptyToken_ReturnsNull()
        {
            string refreshToken = GenerateRefreshToken();
            if (string.IsNullOrEmpty(refreshToken))
                return;

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
                   RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(5),
                   RefreshTokenRevokedAt = null,
                   RefreshTokenHash = BCrypt.Net.BCrypt.HashPassword(refreshToken),
                   Role = 1,
                   DateOfBirth = new DateOnly(1990, 1, 1),
                   PasswordHash = BCrypt.Net.BCrypt.HashPassword("11112222"),
                   IsDeleted = false,
                   Phone = "1234567890"
               });
            SetUpSaveChangesAsync(0);
            //2.Act
            var response =await _userService.RefreshAsync(new RefreshRequest()
            {
                Email = "medo@yahoo.com",
                RefreshToken = string.Empty
            });

            //3.Assert
            Assert.That(response, Is.Null);
            VerifySaveChangesAsync(0);
        }

        [Test]
        [Category("Refresh")]
        public async Task TestRefreshAsync_Expired_ReturnsNull()
        {
            string refreshToken = GenerateRefreshToken();
            if (string.IsNullOrEmpty(refreshToken))
                return;

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
                   RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(-1),
                   RefreshTokenRevokedAt = null,
                   RefreshTokenHash = BCrypt.Net.BCrypt.HashPassword(refreshToken),
                   Role = 1,
                   DateOfBirth = new DateOnly(1990, 1, 1),
                   PasswordHash = BCrypt.Net.BCrypt.HashPassword("11112222"),
                   IsDeleted = false,
                   Phone = "1234567890"
               });
            SetUpSaveChangesAsync(0);
            //2.Act
            var response = await _userService.RefreshAsync(new RefreshRequest()
            {
                Email = "medo@yahoo.com",
                RefreshToken = refreshToken
            });

            //3.Assert
            Assert.That(response, Is.Null);
            VerifySaveChangesAsync(0);
        }

        [Test]
        [Category("Refresh")]
        public async Task TestRefreshAsync_Revoked_ReturnsNull()
        {
            string refreshToken = GenerateRefreshToken();
            if (string.IsNullOrEmpty(refreshToken))
                return;

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
                   RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(5),
                   RefreshTokenRevokedAt = DateTime.UtcNow.AddDays(-1),
                   RefreshTokenHash = BCrypt.Net.BCrypt.HashPassword(refreshToken),
                   Role = 1,
                   DateOfBirth = new DateOnly(1990, 1, 1),
                   PasswordHash = BCrypt.Net.BCrypt.HashPassword("11112222"),
                   IsDeleted = false,
                   Phone = "1234567890"
               });
            SetUpSaveChangesAsync(0);
            //2.Act
            var response = await _userService.RefreshAsync(new RefreshRequest()
            {
                Email = "medo@yahoo.com",
                RefreshToken = refreshToken
            });

            //3.Assert
            Assert.That(response, Is.Null);
            VerifySaveChangesAsync(0);
        }

        //=======================

        [Test]
        [Category("Logout")]
        public async Task TestLogoutAsync_ValidTokenAndEmail_ReturnsTrue()
        {
            string refreshToken = GenerateRefreshToken();
            if (string.IsNullOrEmpty(refreshToken))
                return;
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
                   RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(5),
                   RefreshTokenRevokedAt = null,
                   RefreshTokenHash = BCrypt.Net.BCrypt.HashPassword(refreshToken),
                   Role = 1,
                   DateOfBirth = new DateOnly(1990, 1, 1),
                   PasswordHash = BCrypt.Net.BCrypt.HashPassword("11112222"),
                   IsDeleted = false,
                   Phone = "1234567890"
               });
            SetUpSaveChangesAsync(1);
            //2.Act
            bool succeed = await _userService.LogoutAsync(new LogoutRequest()
            {
                Email = "koko@yahoo.com",
                RefreshToken = refreshToken
            });

            //3.Assert
            Assert.That(succeed, Is.True);
            VerifySaveChangesAsync(1);
        }

        [Test]
        [Category("Logout")]
        public async Task TestLogoutAsync_InvalidToken_False()
        {
            string refreshToken = GenerateRefreshToken();
            if (string.IsNullOrEmpty(refreshToken))
                return;

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
                   RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(5),
                   RefreshTokenRevokedAt = null,
                   RefreshTokenHash = BCrypt.Net.BCrypt.HashPassword(refreshToken),
                   Role = 1,
                   DateOfBirth = new DateOnly(1990, 1, 1),
                   PasswordHash = BCrypt.Net.BCrypt.HashPassword("11112222"),
                   IsDeleted = false,
                   Phone = "1234567890"
               });
            SetUpSaveChangesAsync(0);
            //2.Act
            bool succeed = await _userService.LogoutAsync(new LogoutRequest()
            {
                Email = "koko@yahoo.com",
                RefreshToken = "Invalid Token"
            });

            //3.Assert
            Assert.That(succeed, Is.False);
            VerifySaveChangesAsync(0);
        }

        [Test]
        [Category("Logout")]
        public async Task TestLogoutAsync_InvalidEmail_False()
        {
            string refreshToken = GenerateRefreshToken();
            if (string.IsNullOrEmpty(refreshToken))
                return;

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
                   RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(5),
                   RefreshTokenRevokedAt = null,
                   RefreshTokenHash = BCrypt.Net.BCrypt.HashPassword(refreshToken),
                   Role = 1,
                   DateOfBirth = new DateOnly(1990, 1, 1),
                   PasswordHash = BCrypt.Net.BCrypt.HashPassword("11112222"),
                   IsDeleted = false,
                   Phone = "1234567890"
               });
            SetUpSaveChangesAsync(0);
            //2.Act
            bool succeed = await _userService.LogoutAsync(new LogoutRequest()
            {
                Email = "medo@yahoo.com",
                RefreshToken = refreshToken
            });

            //3.Assert
            Assert.That(succeed, Is.False);
            VerifySaveChangesAsync(0);
        }

        [Test]
        [Category("Logout")]
        public async Task TestLogoutAsync_EmptyEmail_False()
        {
            string refreshToken = GenerateRefreshToken();
            if (string.IsNullOrEmpty(refreshToken))
                return;

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
                   RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(5),
                   RefreshTokenRevokedAt = null,
                   RefreshTokenHash = BCrypt.Net.BCrypt.HashPassword(refreshToken),
                   Role = 1,
                   DateOfBirth = new DateOnly(1990, 1, 1),
                   PasswordHash = BCrypt.Net.BCrypt.HashPassword("11112222"),
                   IsDeleted = false,
                   Phone = "1234567890"
               });
            SetUpSaveChangesAsync(0);
            //2.Act
            bool succeed = await _userService.LogoutAsync(new LogoutRequest()
            {
                Email = string.Empty,
                RefreshToken = refreshToken
            });

            //3.Assert
            Assert.That(succeed, Is.False);
            VerifySaveChangesAsync(0);
        }

        [Test]
        [Category("Logout")]
        public async Task TestLogoutAsync_EmptyToken_False()
        {
            string refreshToken = GenerateRefreshToken();
            if (string.IsNullOrEmpty(refreshToken))
                return;

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
                   RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(5),
                   RefreshTokenRevokedAt = null,
                   RefreshTokenHash = BCrypt.Net.BCrypt.HashPassword(refreshToken),
                   Role = 1,
                   DateOfBirth = new DateOnly(1990, 1, 1),
                   PasswordHash = BCrypt.Net.BCrypt.HashPassword("11112222"),
                   IsDeleted = false,
                   Phone = "1234567890"
               });
            SetUpSaveChangesAsync(0);
            //2.Act
            bool succeed = await _userService.LogoutAsync(new LogoutRequest()
            {
                Email = "medo@yahoo.com",
                RefreshToken = string.Empty
            });

            //3.Assert
            Assert.That(succeed, Is.False);
            VerifySaveChangesAsync(0);
        }

        [Test]
        [Category("Logout")]
        public async Task TestLogoutAsync_Expired_False()
        {
            string refreshToken = GenerateRefreshToken();
            if (string.IsNullOrEmpty(refreshToken))
                return;

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
                   RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(-1),
                   RefreshTokenRevokedAt = null,
                   RefreshTokenHash = BCrypt.Net.BCrypt.HashPassword(refreshToken),
                   Role = 1,
                   DateOfBirth = new DateOnly(1990, 1, 1),
                   PasswordHash = BCrypt.Net.BCrypt.HashPassword("11112222"),
                   IsDeleted = false,
                   Phone = "1234567890"
               });
            SetUpSaveChangesAsync(0);
            //2.Act
            bool succeed = await _userService.LogoutAsync(new LogoutRequest()
            {
                Email = "medo@yahoo.com",
                RefreshToken = refreshToken
            });

            //3.Assert
            Assert.That(succeed, Is.False);
            VerifySaveChangesAsync(0);
        }

        [Test]
        [Category("Logout")]
        public async Task TestLogoutAsync_Revoked_False()
        {
            string refreshToken = GenerateRefreshToken();
            if (string.IsNullOrEmpty(refreshToken))
                return;

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
                   RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(5),
                   RefreshTokenRevokedAt = DateTime.UtcNow.AddDays(-1),
                   RefreshTokenHash = BCrypt.Net.BCrypt.HashPassword(refreshToken),
                   Role = 1,
                   DateOfBirth = new DateOnly(1990, 1, 1),
                   PasswordHash = BCrypt.Net.BCrypt.HashPassword("11112222"),
                   IsDeleted = false,
                   Phone = "1234567890"
               });
            SetUpSaveChangesAsync(0);
            //2.Act
            bool succeed = await _userService.LogoutAsync(new LogoutRequest()
            {
                Email = "medo@yahoo.com",
                RefreshToken = refreshToken
            });

            //3.Assert
            Assert.That(succeed, Is.False);
            VerifySaveChangesAsync(0);
        }




    }
}
