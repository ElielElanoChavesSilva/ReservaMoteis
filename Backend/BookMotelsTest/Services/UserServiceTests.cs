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
        public async Task FindAllAsync_ReturnsAllUsers()
        {
            // Arrange
            var users = new List<UserEntity>
            {
                new UserEntity { Id = Guid.NewGuid(), Name = "User 1", Email = "user1@example.com", Profile = new ProfileEntity { Id = 1, Name = "Client" } },
                new UserEntity { Id = Guid.NewGuid(), Name = "User 2", Email = "user2@example.com", Profile = new ProfileEntity { Id = 2, Name = "Admin" } }
            };
            _mockUserRepository.Setup(r => r.FindAll()).ReturnsAsync(users);

            // Act
            var result = await _userService.FindAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            _mockUserRepository.Verify(r => r.FindAll(), Times.Once);
        }

        [Fact]
        public async Task FindAllAsync_ReturnsEmptyList_WhenNoUsers()
        {
            // Arrange
            _mockUserRepository.Setup(r => r.FindAll()).ReturnsAsync(new List<UserEntity>());

            // Act
            var result = await _userService.FindAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockUserRepository.Verify(r => r.FindAll(), Times.Once);
        }

        [Fact]
        public async Task FindByIdAsync_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var userEntity = new UserEntity { Id = userId, Name = "Eliel", Email = "eliel@example.com", Profile = new ProfileEntity { Id = 1, Name = "Client" } };
            _mockUserRepository.Setup(repo => repo.FindById(userId)).ReturnsAsync(userEntity);

            // Act
            var result = await _userService.FindByIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.Id);
            Assert.Equal("Eliel", result.Name); // Corrected assertion
            _mockUserRepository.Verify(repo => repo.FindById(userId), Times.Once);
        }

        [Fact]
        public async Task FindByIdAsync_ShouldThrowNotFoundException_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _mockUserRepository.Setup(repo => repo.FindById(userId)).ReturnsAsync((UserEntity)null!);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _userService.FindByIdAsync(userId));
            _mockUserRepository.Verify(repo => repo.FindById(userId), Times.Once);
        }

        [Fact]
        public async Task AddAsync_ShouldAddUser_WhenEmailIsNotRegistered()
        {
            // Arrange
            var userDto = new UserDTO { Name = "Eliel Silva", Email = "eliel@example.com", Password = "eliel", ProfileId = 1 };
            _mockUserRepository.Setup(repo => repo.GetByEmailAsync(userDto.Email)).ReturnsAsync((UserEntity)null!);
            _mockUserRepository.Setup(repo => repo.Add(It.IsAny<UserEntity>())).ReturnsAsync((UserEntity entity) =>
            {
                entity.Id = Guid.NewGuid();
                entity.Profile = new ProfileEntity { Id = userDto.ProfileId, Name = "Client" }; // Simulate profile
                return entity;
            });

            // Act
            var result = await _userService.AddAsync(userDto);

            // Assert
            Assert.NotNull(result);
            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.Equal("Eliel Silva", result.Name);
            Assert.Equal(userDto.Email, result.Email);
            Assert.Equal(userDto.ProfileId, result.ProfileId);
            _mockUserRepository.Verify(repo => repo.GetByEmailAsync(userDto.Email), Times.Once);
            _mockUserRepository.Verify(repo => repo.Add(It.IsAny<UserEntity>()), Times.Once);
        }

        [Fact]
        public async Task AddAsync_ShouldThrowConflictException_WhenEmailIsAlreadyRegistered()
        {
            // Arrange
            var userDto = new UserDTO { Name = "Usuário Existente", Email = "existente@example.com", Password = "eliel", ProfileId = 1 };
            var existingUserEntity = new UserEntity { Id = Guid.NewGuid(), Name = "Usuário Existente", Email = "existente@example.com", Profile = new ProfileEntity { Id = 1, Name = "Client" } };
            _mockUserRepository.Setup(repo => repo.GetByEmailAsync(userDto.Email)).ReturnsAsync(existingUserEntity);

            // Act & Assert
            await Assert.ThrowsAsync<ConflictException>(() => _userService.AddAsync(userDto));
            _mockUserRepository.Verify(repo => repo.GetByEmailAsync(userDto.Email), Times.Once);
            _mockUserRepository.Verify(repo => repo.Add(It.IsAny<UserEntity>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_UserNotFound_ThrowsException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var userDto = new GetUserDTO { Id = userId, Name = "Updated" };
            _mockUserRepository.Setup(r => r.FindById(userId)).ReturnsAsync((UserEntity)null!);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _userService.UpdateAsync(userId, userDto));
            _mockUserRepository.Verify(r => r.FindById(userId), Times.Once);
            _mockUserRepository.Verify(r => r.Update(It.IsAny<UserEntity>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_SuccessfulUpdate()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var userDto = new GetUserDTO { Id = userId, Name = "Updated Name", Email = "updated@example.com", ProfileId = 2 };
            var existingUser = new UserEntity { Id = userId, Name = "Original Name", Email = "original@example.com", ProfileId = 1, Profile = new ProfileEntity { Id = 1, Name = "Client" } };

            _mockUserRepository.Setup(r => r.FindById(userId)).ReturnsAsync(existingUser);
            _mockUserRepository.Setup(r => r.Update(It.IsAny<UserEntity>())).ReturnsAsync((UserEntity u) => u);

            // Act
            var result = await _userService.UpdateAsync(userId, userDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userDto.Name, result.Name);
            Assert.Equal(userDto.Email, result.Email);
            Assert.Equal(userDto.ProfileId, result.ProfileId);
            _mockUserRepository.Verify(r => r.FindById(userId), Times.Once);
            _mockUserRepository.Verify(r => r.Update(It.Is<UserEntity>(u =>
                u.Id == userId &&
                u.Name == userDto.Name &&
                u.Email == userDto.Email &&
                u.ProfileId == userDto.ProfileId
            )), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_UserNotFound_ThrowsException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _mockUserRepository.Setup(r => r.FindById(userId)).ReturnsAsync((UserEntity)null!);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _userService.DeleteAsync(userId));
            _mockUserRepository.Verify(r => r.FindById(userId), Times.Once);
            _mockUserRepository.Verify(r => r.Delete(It.IsAny<UserEntity>()), Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_SuccessfulDelete()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var userEntity = new UserEntity { Id = userId, Name = "Usu�rio pra deletar" };
            _mockUserRepository.Setup(r => r.FindById(userId)).ReturnsAsync(userEntity);
            _mockUserRepository.Setup(r => r.Delete(userEntity)).Returns(Task.CompletedTask);

            // Act
            await _userService.DeleteAsync(userId);

            // Assert
            _mockUserRepository.Verify(r => r.FindById(userId), Times.Once);
            _mockUserRepository.Verify(r => r.Delete(userEntity), Times.Once);
        }
    }
}
