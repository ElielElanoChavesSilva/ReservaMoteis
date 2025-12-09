using BookMotelsApplication.DTOs.Reserve;
using BookMotelsApplication.Services;
using BookMotelsDomain.DTOs;
using BookMotelsDomain.Entities;
using BookMotelsDomain.Exceptions;
using BookMotelsDomain.Interfaces;
using Moq;
using Xunit;

namespace BookMotelsTest.Services
{
    public class ReserveServiceTests
    {
        private readonly Mock<IReserveRepository> _mockReserveRepository;
        private readonly ReserveService _reserveService;
        private readonly Mock<ISuiteRepository> _suiteRepository;
        private readonly Mock<IMotelRepository> _motelRepository;

        public ReserveServiceTests()
        {
            _mockReserveRepository = new Mock<IReserveRepository>();
            _suiteRepository = new Mock<ISuiteRepository>();
            _motelRepository = new Mock<IMotelRepository>();
            _reserveService = new ReserveService(
                _suiteRepository.Object,
                _motelRepository.Object,
                _mockReserveRepository.Object);
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
            _mockReserveRepository.Setup(repo => repo.FindBillingReport(null, null, null)).ReturnsAsync(expectedReport);

            // Act
            var result = await _reserveService.FindBillingReportAsync(null, null, null);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal(expectedReport, result);
            _mockReserveRepository.Verify(repo => repo.FindBillingReport(null, null, null), Times.Once);
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
            _mockReserveRepository.Setup(repo => repo.FindBillingReport(null, year, month)).ReturnsAsync(expectedReport);

            // Act
            var result = await _reserveService.FindBillingReportAsync(null, year, month);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(expectedReport, result);
            _mockReserveRepository.Verify(repo => repo.FindBillingReport(null, year, month), Times.Once);
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
            _mockReserveRepository.Setup(repo => repo.FindBillingReport(motelId, year, month)).ReturnsAsync(expectedReport);

            // Act
            var result = await _reserveService.FindBillingReportAsync(motelId, year, month);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(expectedReport, result);
            _mockReserveRepository.Verify(repo => repo.FindBillingReport(motelId, year, month), Times.Once);
        }

        [Fact]
        public async Task AddAsync_ShouldAddReserve_WhenNoConflictExists()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            var reserveDto = new ReserveDTO
            {
                SuiteId = 1,
                CheckIn = DateTime.Now.AddDays(1),
                CheckOut = DateTime.Now.AddDays(3)
            };

            _mockReserveRepository.Setup(repo => repo.HasConflictingReservation(
                reserveDto.SuiteId,
                reserveDto.CheckIn,
                reserveDto.CheckOut))
                .ReturnsAsync(false);
            _mockReserveRepository.Setup(repo => repo.Add(It.IsAny<ReserveEntity>()))
                .ReturnsAsync((ReserveEntity entity) =>
                {
                    entity.Id = 1;
                    return entity;
                });

            // Act
            var result = await _reserveService.AddAsync(userId, reserveDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            _mockReserveRepository.Verify(repo => repo.HasConflictingReservation(
                reserveDto.SuiteId,
                reserveDto.CheckIn,
                reserveDto.CheckOut), Times.Once);
            _mockReserveRepository.Verify(repo => repo.Add(It.IsAny<ReserveEntity>()), Times.Once);
        }

        [Fact]
        public async Task AddAsync_ShouldThrowConflictException_WhenConflictExists()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            var reserveDto = new ReserveDTO
            {
                SuiteId = 1,
                CheckIn = DateTime.Now.AddDays(1),
                CheckOut = DateTime.Now.AddDays(3)
            };

            _mockReserveRepository.Setup(repo => repo.HasConflictingReservation(
                reserveDto.SuiteId,
                reserveDto.CheckIn,
                reserveDto.CheckOut))
                .ReturnsAsync(true);

            // Act & Assert
            await Assert.ThrowsAsync<ConflictException>(() => _reserveService.AddAsync(userId, reserveDto));
            _mockReserveRepository.Verify(repo => repo.HasConflictingReservation(
                reserveDto.SuiteId,
                reserveDto.CheckIn,
                reserveDto.CheckOut), Times.Once);
            _mockReserveRepository.Verify(repo => repo.Add(It.IsAny<ReserveEntity>()), Times.Never);
        }
    }
}