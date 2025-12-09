using BookMotelsApplication.DTOs.Suite;
using BookMotelsApplication.Mappers;
using BookMotelsApplication.Services;
using BookMotelsDomain.Entities;
using BookMotelsDomain.Exceptions;
using BookMotelsDomain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace BookMotelsTest.Services
{
    public class SuiteServiceTests
    {
        private readonly Mock<ISuiteRepository> _mockSuiteRepository;
        private readonly Mock<IMotelRepository> _mockMotelRepository;
        private readonly Mock<IDistributedCache> _mockDistributedCache;
        private readonly SuiteService _suiteService;

        public SuiteServiceTests()
        {
            _mockSuiteRepository = new Mock<ISuiteRepository>();
            _mockMotelRepository = new Mock<IMotelRepository>();
            _mockDistributedCache = new Mock<IDistributedCache>();
            _suiteService = new SuiteService(
                _mockMotelRepository.Object,
                _mockSuiteRepository.Object,
                _mockDistributedCache.Object);
        }

        [Fact]
        public async Task FindAllAsync_ReturnsAllSuites()
        {
            // Arrange
            var suites = new List<SuiteEntity>
            {
                new () { Id = 1, Name = "Suite 1", Description = "Desc 1", PricePerPeriod = 100, MaxOccupancy = 2, MotelId = 1 },
                new SuiteEntity { Id = 2, Name = "Suite 2", Description = "Desc 2", PricePerPeriod = 150, MaxOccupancy = 3, MotelId = 1 }
            };
            _mockSuiteRepository.Setup(r => r.FindAll()).ReturnsAsync(suites);

            // Act
            var result = await _suiteService.FindAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            _mockSuiteRepository.Verify(r => r.FindAll(), Times.Once);
        }

        [Fact]
        public async Task FindAllAsync_ReturnsEmptyList_WhenNoSuites()
        {
            // Arrange
            _mockSuiteRepository.Setup(r => r.FindAll()).ReturnsAsync(new List<SuiteEntity>());

            // Act
            var result = await _suiteService.FindAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockSuiteRepository.Verify(r => r.FindAll(), Times.Once);
        }

        [Fact]
        public async Task FindAllAvailable_MotelNotFound_ThrowsNotFoundException()
        {
            // Arrange
            long motelId = 99;
            _mockMotelRepository.Setup(r => r.Exist(motelId)).ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _suiteService.FindAllAvailable(motelId, null, null, null));
            _mockMotelRepository.Verify(r => r.Exist(motelId), Times.Once);
        }

        [Fact]
        public async Task FindAllAvailable_ReturnsCachedData_WhenAvailable()
        {
            // Arrange
            long motelId = 1;
            string cacheKey = $"suites:available:{motelId}:::00:00";
            var cachedSuites = new List<GetSuiteDTO> { new GetSuiteDTO { Id = 1, Name = "Cached Suite" } };
            var cachedJson = JsonConvert.SerializeObject(cachedSuites);

            _mockMotelRepository.Setup(r => r.Exist(motelId)).ReturnsAsync(true);
            _mockDistributedCache.Setup(c => c.GetStringAsync(cacheKey, default)).ReturnsAsync(cachedJson);

            // Act
            var result = await _suiteService.FindAllAvailable(motelId, null, null, null);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("Cached Suite", result.First().Name);
            _mockMotelRepository.Verify(r => r.Exist(motelId), Times.Once);
            _mockDistributedCache.Verify(c => c.GetStringAsync(cacheKey, default), Times.Once);
            _mockSuiteRepository.Verify(r => r.FindAllAvailable(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>()), Times.Never);
        }

        [Fact]
        public async Task FindAllAvailable_FetchesAndCaches_WhenCacheMiss()
        {
            // Arrange
            long motelId = 1;
            string cacheKey = $"suites:available:{motelId}:::00:00";
            var repoSuites = new List<SuiteEntity> { new SuiteEntity { Id = 1, Name = "Repo Suite", MotelId = motelId } };
            var expectedDto = repoSuites.ToDTO().ToList();
            var expectedJson = System.Text.Json.JsonSerializer.Serialize(
                expectedDto,
                new System.Text.Json.JsonSerializerOptions { PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase }
            );

            _mockMotelRepository.Setup(r => r.Exist(motelId)).ReturnsAsync(true);
            _mockDistributedCache.Setup(c => c.GetStringAsync(cacheKey, default)).ReturnsAsync((string)null!);
            _mockSuiteRepository.Setup(r => r.FindAllAvailable(motelId, null, null, null)).ReturnsAsync(repoSuites);
            _mockDistributedCache.Setup(c => c.SetStringAsync(cacheKey, expectedJson, It.IsAny<DistributedCacheEntryOptions>(), default)).Returns(Task.CompletedTask);

            // Act
            var result = await _suiteService.FindAllAvailable(motelId, null, null, null);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("Repo Suite", result.First().Name);
            _mockMotelRepository.Verify(r => r.Exist(motelId), Times.Once);
            _mockDistributedCache.Verify(c => c.GetStringAsync(cacheKey, default), Times.Once);
            _mockSuiteRepository.Verify(r => r.FindAllAvailable(motelId, null, null, null), Times.Once);
            _mockDistributedCache.Verify(c => c.SetStringAsync(cacheKey, expectedJson, It.IsAny<DistributedCacheEntryOptions>(), default), Times.Once);
        }

        [Fact]
        public async Task FindByIdAsync_SuiteFound_ReturnsSuiteDTO()
        {
            // Arrange
            long suiteId = 1;
            var suiteEntity = new SuiteEntity { Id = suiteId, Name = "Test Suite" };
            _mockSuiteRepository.Setup(r => r.FindById(suiteId)).ReturnsAsync(suiteEntity);

            // Act
            var result = await _suiteService.FindByIdAsync(suiteId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(suiteId, result.Id);
            Assert.Equal("Test Suite", result.Name);
            _mockSuiteRepository.Verify(r => r.FindById(suiteId), Times.Once);
        }

        [Fact]
        public async Task FindByIdAsync_SuiteNotFound_ThrowsNotFoundException()
        {
            // Arrange
            long suiteId = 99;
            _mockSuiteRepository.Setup(r => r.FindById(suiteId)).ReturnsAsync((SuiteEntity)null!);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _suiteService.FindByIdAsync(suiteId));
            _mockSuiteRepository.Verify(r => r.FindById(suiteId), Times.Once);
        }

        [Fact]
        public async Task AddAsync_MotelNotFound_ThrowsNotFoundException()
        {
            // Arrange
            long motelId = 99;
            var suiteDto = new SuiteDTO { Name = "New Suite" };
            _mockMotelRepository.Setup(r => r.Exist(motelId)).ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _suiteService.AddAsync(motelId, suiteDto));
            _mockMotelRepository.Verify(r => r.Exist(motelId), Times.Once);
            _mockSuiteRepository.Verify(r => r.Add(It.IsAny<SuiteEntity>()), Times.Never);
        }

        [Fact]
        public async Task AddAsync_SuccessfulAdd_ReturnsAddedSuiteDTO()
        {
            // Arrange
            long motelId = 1;
            var suiteDto = new SuiteDTO { Name = "New Suite", PricePerPeriod = 200, MaxOccupancy = 4 };
            var suiteEntity = new SuiteEntity { Id = 1, Name = "New Suite", PricePerPeriod = 200, MaxOccupancy = 4, MotelId = motelId };

            _mockMotelRepository.Setup(r => r.Exist(motelId)).ReturnsAsync(true);
            _mockSuiteRepository.Setup(r => r.Add(It.IsAny<SuiteEntity>()))
                                .ReturnsAsync(suiteEntity);

            // Act
            var result = await _suiteService.AddAsync(motelId, suiteDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("New Suite", result.Name);
            Assert.Equal(motelId, result.MotelId);
            _mockMotelRepository.Verify(r => r.Exist(motelId), Times.Once);
            _mockSuiteRepository.Verify(r => r.Add(It.Is<SuiteEntity>(s => s.MotelId == motelId && s.Name == suiteDto.Name)), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_SuiteNotFound_ThrowsNotFoundException()
        {
            // Arrange
            long suiteId = 99;
            var suiteDto = new SuiteDTO { Name = "Updated Suite" };
            _mockSuiteRepository.Setup(r => r.FindById(suiteId)).ReturnsAsync((SuiteEntity)null!);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _suiteService.UpdateAsync(suiteId, suiteDto));
            _mockSuiteRepository.Verify(r => r.FindById(suiteId), Times.Once);
            _mockSuiteRepository.Verify(r => r.Update(It.IsAny<SuiteEntity>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_SuccessfulUpdate()
        {
            // Arrange
            long suiteId = 1;
            var suiteDto = new SuiteDTO { Name = "Updated Suite", Description = "New Desc", PricePerPeriod = 250, MaxOccupancy = 5 };
            var existingSuite = new SuiteEntity { Id = suiteId, Name = "Original", Description = "Old Desc", PricePerPeriod = 200, MaxOccupancy = 4, MotelId = 1 };

            _mockSuiteRepository.Setup(r => r.FindById(suiteId)).ReturnsAsync(existingSuite);
            _mockSuiteRepository.Setup(r => r.Update(It.IsAny<SuiteEntity>())).ReturnsAsync(existingSuite);

            // Act
            await _suiteService.UpdateAsync(suiteId, suiteDto);

            // Assert
            _mockSuiteRepository.Verify(r => r.FindById(suiteId), Times.Once);
            _mockSuiteRepository.Verify(r => r.Update(It.Is<SuiteEntity>(s =>
                s.Id == suiteId &&
                s.Name == suiteDto.Name &&
                s.Description == suiteDto.Description &&
                s.PricePerPeriod == suiteDto.PricePerPeriod &&
                s.MaxOccupancy == suiteDto.MaxOccupancy
            )), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_SuiteNotFound_ThrowsNotFoundException()
        {
            // Arrange
            long suiteId = 99;
            _mockSuiteRepository.Setup(r => r.FindById(suiteId)).ReturnsAsync((SuiteEntity)null!);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _suiteService.DeleteAsync(suiteId));
            _mockSuiteRepository.Verify(r => r.FindById(suiteId), Times.Once);
            _mockSuiteRepository.Verify(r => r.Delete(It.IsAny<SuiteEntity>()), Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_SuccessfulDelete()
        {
            // Arrange
            long suiteId = 1;
            var suiteEntity = new SuiteEntity { Id = suiteId, Name = "Suite to delete" };
            _mockSuiteRepository.Setup(r => r.FindById(suiteId)).ReturnsAsync(suiteEntity);
            _mockSuiteRepository.Setup(r => r.Delete(suiteEntity)).Returns(Task.CompletedTask);

            // Act
            await _suiteService.DeleteAsync(suiteId);

            // Assert
            _mockSuiteRepository.Verify(r => r.FindById(suiteId), Times.Once);
            _mockSuiteRepository.Verify(r => r.Delete(suiteEntity), Times.Once);
        }
    }
}
