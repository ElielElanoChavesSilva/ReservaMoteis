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
        public async Task FindAllAvailable_FetchesAndCaches_WhenCacheMiss()
        {
            // Arrange
            _memoryCache.Remove("AllMotels");
            var motels = new List<MotelEntity>
            {
                new () { Id = 1, Name = "Motel A" },
                new () { Id = 2, Name = "Motel B" }
            };
            _mockMotelRepository.Setup(r => r.FindAll()).ReturnsAsync(motels);

            // Act
            var result = await _motelService.FindAllAvailableAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            _mockMotelRepository.Verify(r => r.FindAll(), Times.Once);

            // Verify cache entry
            Assert.True(_memoryCache.TryGetValue("AllMotels", out var cachedMotels));
            Assert.Equal(2, ((IEnumerable<GetMotelDTO>)cachedMotels!).Count());
        }

        [Fact]
        public async Task FindAllAvailable_ReturnsCachedData_WhenAvailable()
        {
            // Arrange
            var cachedMotels = new List<GetMotelDTO>
            {
                new () { Id = 3, Name = "Motel C" }
            };
            _memoryCache.Set("AllMotels", cachedMotels);

            // Act
            var result = await _motelService.FindAllAvailableAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("Motel C", result.First().Name);
            _mockMotelRepository.Verify(r => r.FindAll(), Times.Never);
        }

        [Fact]
        public async Task FindByIdAsync_MotelFound_ReturnsMotelDTO()
        {
            // Arrange
            long motelId = 1;
            var motelEntity = new MotelEntity { Id = motelId, Name = "Motel A" };
            _mockMotelRepository.Setup(r => r.FindById(motelId)).ReturnsAsync(motelEntity);

            // Act
            var result = await _motelService.FindByIdAsync(motelId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(motelId, result.Id);
            Assert.Equal("Motel A", result.Name);
            _mockMotelRepository.Verify(r => r.FindById(motelId), Times.Once);
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
        public async Task AddAsync_SuccessfulAdd_ReturnsAddedMotelDTO()
        {
            // Arrange
            _memoryCache.Remove("AllMotels");
            var motelDto = new MotelDTO { Name = "New Motel", Address = "123 St" };
            var motelEntity = new MotelEntity { Id = 1, Name = "New Motel", Address = "123 St" };

            _mockMotelRepository.Setup(r => r.Add(It.IsAny<MotelEntity>()))
                                .ReturnsAsync(motelEntity);

            // Act
            var result = await _motelService.AddAsync(motelDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("New Motel", result.Name);
            _mockMotelRepository.Verify(r => r.Add(It.Is<MotelEntity>(m => m.Name == motelDto.Name)), Times.Once);
            Assert.False(_memoryCache.TryGetValue("AllMotels", out _));
        }

        [Fact]
        public async Task UpdateAsync_MotelNotFound_ThrowsNotFoundException()
        {
            // Arrange
            long motelId = 99;
            var motelDto = new MotelDTO { Name = "Updated Motel" };
            _mockMotelRepository.Setup(r => r.FindById(motelId)).ReturnsAsync((MotelEntity)null!);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _motelService.UpdateAsync(motelId, motelDto));
            _mockMotelRepository.Verify(r => r.FindById(motelId), Times.Once);
            _mockMotelRepository.Verify(r => r.Update(It.IsAny<MotelEntity>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_SuccessfulUpdate()
        {
            // Arrange
            _memoryCache.Set("AllMotels", new List<GetMotelDTO> { new() { Id = 1, Name = "Original" } });
            long motelId = 1;
            var motelDto = new MotelDTO { Name = "Updated Motel", Address = "New Address", Phone = "111", Description = "New Desc" };
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
            _memoryCache.Set("AllMotels", new List<GetMotelDTO> { new GetMotelDTO { Id = 1, Name = "Motel to delete" } });
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