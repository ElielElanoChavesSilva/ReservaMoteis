using BookMotelsApplication.DTOs.Motel;
using BookMotelsApplication.Services;
using BookMotelsDomain.Entities;
using BookMotelsDomain.Exceptions;
using BookMotelsDomain.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace BookMotelsTest.Services
{
    public class MotelServiceTests
    {
        private readonly Mock<IMotelRepository> _mockMotelRepository;
        private readonly IMemoryCache _memoryCache;
        private readonly MotelService _motelService;

        public MotelServiceTests()
        {
            _mockMotelRepository = new Mock<IMotelRepository>();
            _memoryCache = new MemoryCache(Options.Create(new MemoryCacheOptions()));
            _motelService = new MotelService(_mockMotelRepository.Object, _memoryCache);
        }

        [Fact]
        public async Task FindAllAsync_ReturnsAllMotels()
        {
            // Arrange
            var motels = new List<MotelEntity>
            {
                new () { Id = 1, Name = "Motel A" },
                new () { Id = 2, Name = "Motel B" }
            };
            _mockMotelRepository.Setup(r => r.FindAll()).ReturnsAsync(motels);

            // Act
            var result = await _motelService.FindAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            _mockMotelRepository.Verify(r => r.FindAll(), Times.Once);
        }

        [Fact]
        public async Task FindAllAsync_ReturnsEmptyList_WhenNoMotels()
        {
            // Arrange
            _mockMotelRepository.Setup(r => r.FindAll()).ReturnsAsync(new List<MotelEntity>());

            // Act
            var result = await _motelService.FindAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockMotelRepository.Verify(r => r.FindAll(), Times.Once);
        }

        [Fact]
        public async Task FindByIdAsync_MotelNotFound_ThrowsNotFoundException()
        {
            // Arrange
            long motelId = 99;
            _mockMotelRepository.Setup(r => r.FindById(motelId)).ReturnsAsync((MotelEntity)null!);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _motelService.FindByIdAsync(motelId));
            _mockMotelRepository.Verify(r => r.FindById(motelId), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_SuccessfulUpdate()
        {
            // Arrange
            _memoryCache.Set("AllMotels", new List<GetMotelDTO> { new(1, "Original", "", "", "", null) });
            long motelId = 1;
            var motelDto = new MotelDTO("Updated Motel", "New Address", "111", "New Desc");
            var existingMotel = new MotelEntity { Id = motelId, Name = "Original", Address = "Old Address", Phone = "000", Description = "Old Desc" };

            _mockMotelRepository.Setup(r => r.FindById(motelId)).ReturnsAsync(existingMotel);
            _mockMotelRepository.Setup(r => r.Update(It.IsAny<MotelEntity>())).ReturnsAsync(existingMotel);

            // Act
            await _motelService.UpdateAsync(motelId, motelDto);

            // Assert
            _mockMotelRepository.Verify(r => r.FindById(motelId), Times.Once);
            _mockMotelRepository.Verify(r => r.Update(It.Is<MotelEntity>(m =>
                m.Id == motelId &&
                m.Name == motelDto.Name &&
                m.Address == motelDto.Address &&
                m.Phone == motelDto.Phone &&
                m.Description == motelDto.Description
            )), Times.Once);
            Assert.False(_memoryCache.TryGetValue("AllMotels", out _));
        }

        [Fact]
        public async Task DeleteAsync_MotelNotFound_ThrowsNotFoundException()
        {
            // Arrange
            long motelId = 99;
            _mockMotelRepository.Setup(r => r.FindById(motelId)).ReturnsAsync((MotelEntity)null!);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _motelService.DeleteAsync(motelId));
            _mockMotelRepository.Verify(r => r.FindById(motelId), Times.Once);
            _mockMotelRepository.Verify(r => r.Delete(It.IsAny<MotelEntity>()), Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_SuccessfulDelete()
        {
            // Arrange
            _memoryCache.Set("AllMotels", new List<GetMotelDTO> { new(1, "Motel to delete", "", "", "", null) });
            long motelId = 1;
            var motelEntity = new MotelEntity { Id = motelId, Name = "Motel to delete" };
            _mockMotelRepository.Setup(r => r.FindById(motelId)).ReturnsAsync(motelEntity);
            _mockMotelRepository.Setup(r => r.Delete(motelEntity)).Returns(Task.CompletedTask);

            // Act
            await _motelService.DeleteAsync(motelId);

            // Assert
            _mockMotelRepository.Verify(r => r.FindById(motelId), Times.Once);
            _mockMotelRepository.Verify(r => r.Delete(motelEntity), Times.Once);
            Assert.False(_memoryCache.TryGetValue("AllMotels", out _));
        }
    }
}
