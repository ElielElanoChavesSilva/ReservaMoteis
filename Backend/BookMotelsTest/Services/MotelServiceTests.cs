using BookMotelsApplication.Services;
using BookMotelsDomain.DTOs;
using BookMotelsDomain.Interfaces;
using Moq;
using Xunit;

namespace BookMotelsTest.Services
{
    public class MotelServiceTests
    {
        private readonly Mock<IMotelRepository> _mockMotelRepository;
        private readonly MotelService _motelService;

        public MotelServiceTests()
        {
            _mockMotelRepository = new Mock<IMotelRepository>();
            _motelService = new MotelService(_mockMotelRepository.Object);
        }

        [Fact]
        public async Task GetBillingReportAsync_ShouldReturnFullReport_WhenNoParametersProvided()
        {
            // Arrange
            var expectedReport = new List<BillingReportDTO>
            {
                new() { MotelId = 1, MotelName = "Motel A", Year = 2023, Month = 1, TotalRevenue = 1000m },
                new () { MotelId = 2, MotelName = "Motel B", Year = 2023, Month = 1, TotalRevenue = 1500m }
            };
            _mockMotelRepository.Setup(repo => repo.FindBillingReport(null, null, null)).ReturnsAsync(expectedReport);

            // Act
            var result = await _motelService.FindBillingReportAsync(null, null, null);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal(expectedReport, result);
            _mockMotelRepository.Verify(repo => repo.FindBillingReport(null, null, null), Times.Once);
        }

        [Fact]
        public async Task GetBillingReportAsync_ShouldReturnFilteredReport_WhenYearAndMonthProvided()
        {
            // Arrange
            int year = 2024;
            int month = 6;
            var expectedReport = new List<BillingReportDTO>
            {
                new BillingReportDTO { MotelId = 1, MotelName = "Motel A", Year = year, Month = month, TotalRevenue = 2000m }
            };
            _mockMotelRepository.Setup(repo => repo.FindBillingReport(null, year, month)).ReturnsAsync(expectedReport);

            // Act
            var result = await _motelService.FindBillingReportAsync(null, year, month);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(expectedReport, result);
            _mockMotelRepository.Verify(repo => repo.FindBillingReport(null, year, month), Times.Once);
        }

        [Fact]
        public async Task FindBillingReportAsync_ShouldReturnFilteredReport_WhenMotelIdYearAndMonthProvided()
        {
            // Arrange
            long motelId = 1;
            int year = 2024;
            int month = 7;
            var expectedReport = new List<BillingReportDTO>
            {
                new () { MotelId = motelId, MotelName = "Motel A", Year = year, Month = month, TotalRevenue = 3000m }
            };
            _mockMotelRepository.Setup(repo => repo.FindBillingReport(motelId, year, month)).ReturnsAsync(expectedReport);

            // Act
            var result = await _motelService.FindBillingReportAsync(motelId, year, month);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(expectedReport, result);
            _mockMotelRepository.Verify(repo => repo.FindBillingReport(motelId, year, month), Times.Once);
        }
    }
}
