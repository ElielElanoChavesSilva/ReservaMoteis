using BookMotelsApplication.DTOs.User;
using BookMotelsDomain.Entities;
using BookMotelsDomain.Exceptions;
using BookMotelsDomain.Interfaces;
using Moq;
using Xunit;

namespace BookMotelsTest.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _userService = new UserService(_mockUserRepository.Object);
        }

        [Fact]
        public async Task FindByIdAsync_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var userEntity = new UserEntity { Id = userId, Name = "Eliel", Email = "eliel@example.com" };
            _mockUserRepository.Setup(repo => repo.FindById(userId)).ReturnsAsync(userEntity);

            // Act
            var result = await _userService.FindByIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.Id);
            Assert.Equal("Test User", result.Name);
            _mockUserRepository.Verify(repo => repo.FindById(userId), Times.Once);
        }

        [Fact]
        public async Task FindByIdAsync_ShouldThrowNotFoundException_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _mockUserRepository.Setup(repo => repo.FindById(userId)).ReturnsAsync((UserEntity)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _userService.FindByIdAsync(userId));
            _mockUserRepository.Verify(repo => repo.FindById(userId), Times.Once);
        }

        [Fact]
        public async Task AddAsync_ShouldAddUser_WhenEmailIsNotRegistered()
        {
            // Arrange
            var userDto = new UserDTO { Name = "Eliel Silva", Email = "eliel@example.com", Password = "eliel", ProfileId = 1 };
            _mockUserRepository.Setup(repo => repo.GetByEmailAsync(userDto.Email)).ReturnsAsync((UserEntity)null);
            _mockUserRepository.Setup(repo => repo.Add(It.IsAny<UserEntity>())).ReturnsAsync((UserEntity entity) =>
            {
                entity.Id = Guid.NewGuid();
                return entity;
            });

            // Act
            var result = await _userService.AddAsync(userDto);

            // Assert
            Assert.NotNull(result);
            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.Equal("Eliel Silva", result.Name);
            _mockUserRepository.Verify(repo => repo.GetByEmailAsync(userDto.Email), Times.Once);
            _mockUserRepository.Verify(repo => repo.Add(It.IsAny<UserEntity>()), Times.Once);
        }

        [Fact]
        public async Task AddAsync_ShouldThrowConflictException_WhenEmailIsAlreadyRegistered()
        {
            // Arrange
            var userDto = new UserDTO { Name = "Usuário Existente", Email = "existente@example.com", Password = "eliel", ProfileId = 1 };
            var existingUserEntity = new UserEntity { Id = Guid.NewGuid(), Name = "Usuário Existente", Email = "existente@example.com" };
            _mockUserRepository.Setup(repo => repo.GetByEmailAsync(userDto.Email)).ReturnsAsync(existingUserEntity);

            // Act & Assert
            await Assert.ThrowsAsync<ConflictException>(() => _userService.AddAsync(userDto));
            _mockUserRepository.Verify(repo => repo.GetByEmailAsync(userDto.Email), Times.Once);
            _mockUserRepository.Verify(repo => repo.Add(It.IsAny<UserEntity>()), Times.Never);
        }
    }
}