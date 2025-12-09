using BookMotelsApplication.DTOs.Auth;
using BookMotelsApplication.Interfaces;
using BookMotelsApplication.Services;
using BookMotelsDomain.Entities;
using BookMotelsDomain.Exceptions;
using BookMotelsDomain.Interfaces;
using Moq;
using Xunit;

namespace BookMotelsTest.Services
{
    public class AuthenticationServiceTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IJwtConfiguration> _mockJwtConfiguration;
        private readonly AuthenticationService _authenticationService;

        public AuthenticationServiceTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockJwtConfiguration = new Mock<IJwtConfiguration>();

            // Setup common JWT configuration mock
            _mockJwtConfiguration.Setup(x => x.Key).Returns("supersecretkeythatisatleast32characterslong");
            _mockJwtConfiguration.Setup(x => x.TokenExpirationInMinutes).Returns(60);

            _authenticationService = new AuthenticationService(
                _mockUserRepository.Object,
                _mockJwtConfiguration.Object);
        }

        [Fact]
        public async Task AuthenticateAsync_ValidCredentials_ReturnsAuthResponseDTO()
        {
            // Arrange
            var loginDto = new LoginDTO { Email = "test@example.com", Password = "Password123" };
            var userEntity = new UserEntity
            {
                Id = Guid.NewGuid(),
                Email = "test@example.com",
                Name = "Test User",
                Password = BCrypt.Net.BCrypt.HashPassword("Password123"), // Hash the password
                Profile = new ProfileEntity { Name = "Admin" }
            };

            _mockUserRepository.Setup(r => r.GetByEmailAsync(loginDto.Email))
                               .ReturnsAsync(userEntity);

            // Act
            var result = await _authenticationService.AuthenticateAsync(loginDto);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<AuthResponseDTO>(result);
            Assert.False(string.IsNullOrEmpty(result.Token));
            Assert.Equal(userEntity.Email, result.Email);
            Assert.Equal(userEntity.Name, result.Name);
            Assert.Equal(userEntity.Id, result.UserId);
            Assert.Equal(userEntity.Profile.Name, result.Role);
        }

        [Fact]
        public async Task AuthenticateAsync_UserNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var loginDto = new LoginDTO { Email = "nonexistent@example.com", Password = "Password123" };

            _mockUserRepository.Setup(r => r.GetByEmailAsync(loginDto.Email))
                               .ReturnsAsync((UserEntity)null!); // Simulate user not found

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _authenticationService.AuthenticateAsync(loginDto));
        }

        [Fact]
        public async Task AuthenticateAsync_InvalidPassword_ReturnsNull()
        {
            // Arrange
            var loginDto = new LoginDTO { Email = "test@example.com", Password = "WrongPassword" };
            var userEntity = new UserEntity
            {
                Id = Guid.NewGuid(),
                Email = "test@example.com",
                Name = "Test User",
                Password = BCrypt.Net.BCrypt.HashPassword("Password123"), // Correct hashed password
                Profile = new ProfileEntity { Name = "Admin" }
            };

            _mockUserRepository.Setup(r => r.GetByEmailAsync(loginDto.Email))
                               .ReturnsAsync(userEntity);

            // Act
            var result = await _authenticationService.AuthenticateAsync(loginDto);

            // Assert
            Assert.Null(result);
        }
    }
}
