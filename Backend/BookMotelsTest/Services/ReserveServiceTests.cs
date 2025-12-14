using BookMotelsApplication.DTOs.Reserve;
using BookMotelsApplication.Services;
using BookMotelsDomain.Entities;
using BookMotelsDomain.Exceptions;
using BookMotelsDomain.Interfaces;
using BookMotelsDomain.Projections;
using Moq;
using Xunit;

namespace BookMotelsTest.Services
{
    public class ReserveServiceTests
    {
        private readonly Mock<IReserveRepository> _mockReserveRepository;
        private readonly Mock<ISuiteRepository> _mockSuiteRepository;
        private readonly Mock<IMotelRepository> _mockMotelRepository;
        private readonly ReserveService _reserveService;

        public ReserveServiceTests()
        {
            _mockReserveRepository = new Mock<IReserveRepository>();
            _mockSuiteRepository = new Mock<ISuiteRepository>();
            _mockMotelRepository = new Mock<IMotelRepository>();
            _reserveService = new ReserveService(
                _mockSuiteRepository.Object,
                _mockMotelRepository.Object,
                _mockReserveRepository.Object);
        }

        [Fact]
        public async Task FindAllAsync_ReturnsAllReserves()
        {
            // Arrange
            var reservesProjection = new List<GetReserveProjection>
            {
                new() { Id = 1, SuiteId = 1, CheckIn = DateTime.Now, CheckOut = DateTime.Now.AddDays(1), SuiteName = "Suite 1", MotelName = "Motel 1", UserName = "User 1" },
                new() { Id = 2, SuiteId = 2, CheckIn = DateTime.Now, CheckOut = DateTime.Now.AddDays(2), SuiteName = "Suite 2", MotelName = "Motel 1", UserName = "User 2" }
            };

            _mockReserveRepository.Setup(r => r.FindAllProjection()).ReturnsAsync(reservesProjection);

            // Act
            var result = await _reserveService.FindAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            _mockReserveRepository.Verify(r => r.FindAllProjection(), Times.Once);
        }

        [Fact]
        public async Task FindAllAsync_ReturnsEmptyList_WhenNoReserves()
        {
            // Arrange
            _mockReserveRepository.Setup(r => r.FindAllProjection()).ReturnsAsync(new List<GetReserveProjection>());

            // Act
            var result = await _reserveService.FindAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockReserveRepository.Verify(r => r.FindAllProjection(), Times.Once);
        }

        [Fact]
        public async Task FindAllByUserAsync_ReturnsReservesForUser()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            var reservesProjection = new List<GetReserveProjection>
            {
                new() { Id = 1, SuiteId = 1, CheckIn = DateTime.Now, CheckOut = DateTime.Now.AddDays(1), SuiteName = "Suite 1", MotelName = "Motel 1", UserName = "User 1" },
                new() { Id = 2, SuiteId = 2, CheckIn = DateTime.Now, CheckOut = DateTime.Now.AddDays(2), SuiteName = "Suite 2", MotelName = "Motel 1", UserName = "User 2" }
            };

            _mockReserveRepository.Setup(r => r.FindAllByUser(userId)).ReturnsAsync(reservesProjection);

            // Act
            var result = await _reserveService.FindAllByUserAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            _mockReserveRepository.Verify(r => r.FindAllByUser(userId), Times.Once);
        }

        [Fact]
        public async Task FindAllByUserAsync_ReturnsEmptyList_WhenNoReservesForUser()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            _mockReserveRepository.Setup(r => r.FindAllByUser(userId)).ReturnsAsync(new List<GetReserveProjection>());

            // Act
            var result = await _reserveService.FindAllByUserAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockReserveRepository.Verify(r => r.FindAllByUser(userId), Times.Once);
        }

        [Fact]
        public async Task FindByIdAsync_ReserveFound_ReturnsReserveDTO()
        {
            // Arrange
            long reserveId = 1;
            var reserveProjection = new GetReserveProjection { Id = reserveId, SuiteId = 1, CheckIn = DateTime.Now, CheckOut = DateTime.Now.AddDays(1), SuiteName = "Suite Name", MotelName = "Motel Name", UserName = "User Name" };

            _mockReserveRepository.Setup(r => r.FindByIdProjection(reserveId)).ReturnsAsync(reserveProjection);

            // Act
            var result = await _reserveService.FindByIdAsync(reserveId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(reserveId, result.Id);
            _mockReserveRepository.Verify(r => r.FindByIdProjection(reserveId), Times.Once);
        }

        [Fact]
        public async Task FindByIdAsync_ReserveNotFound_ThrowsNotFoundException()
        {
            // Arrange
            long reserveId = 99;
            _mockReserveRepository.Setup(r => r.FindByIdProjection(reserveId)).ReturnsAsync((GetReserveProjection)null!);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _reserveService.FindByIdAsync(reserveId));
            _mockReserveRepository.Verify(r => r.FindByIdProjection(reserveId), Times.Once);
        }

        [Fact]
        public async Task AddAsync_ShouldAddReserve_WhenNoConflictAndValidDates()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            var suiteId = 1L;
            var checkIn = DateTime.UtcNow.Date.AddDays(1);
            var checkOut = DateTime.UtcNow.Date.AddDays(3);
            var reserveDto = new ReserveDTO(suiteId, checkIn, checkOut);
            var suiteEntity = new SuiteEntity { Id = suiteId, PricePerPeriod = 100m };

            _mockReserveRepository.Setup(repo => repo.HasConflictingReservation(suiteId, checkIn, checkOut))
                .ReturnsAsync(false);
            _mockSuiteRepository.Setup(s => s.FindById(suiteId)).ReturnsAsync(suiteEntity);
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
            Assert.Equal(suiteId, result.SuiteId);

            _mockReserveRepository.Verify(repo => repo.HasConflictingReservation(suiteId, checkIn, checkOut), Times.Once);
            _mockSuiteRepository.Verify(s => s.FindById(suiteId), Times.Once);
            _mockReserveRepository.Verify(repo => repo.Add(It.IsAny<ReserveEntity>()), Times.Once);
        }

        [Fact]
        public async Task AddAsync_ShouldThrowConflictException_WhenConflictExists()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            var reserveDto = new ReserveDTO(1, DateTime.Now.AddDays(1), DateTime.Now.AddDays(3));

            _mockReserveRepository.Setup(repo => repo.HasConflictingReservation(
                reserveDto.SuiteId,
                reserveDto.CheckIn,
                reserveDto.CheckOut))
                .ReturnsAsync(true);
            _mockSuiteRepository.Setup(s => s.FindById(reserveDto.SuiteId)).ReturnsAsync(new SuiteEntity { Id = reserveDto.SuiteId, PricePerPeriod = 100m });

            // Act & Assert
            await Assert.ThrowsAsync<ConflictException>(() => _reserveService.AddAsync(userId, reserveDto));
            _mockReserveRepository.Verify(repo => repo.HasConflictingReservation(
                reserveDto.SuiteId,
                reserveDto.CheckIn,
                reserveDto.CheckOut), Times.Once);
            _mockReserveRepository.Verify(repo => repo.Add(It.IsAny<ReserveEntity>()), Times.Never);
        }

        [Fact]
        public async Task AddAsync_ShouldThrowBadRequestException_WhenCheckOutBeforeCheckIn()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            var reserveDto = new ReserveDTO(1, DateTime.Now.AddDays(3), DateTime.Now.AddDays(1));

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(() => _reserveService.AddAsync(userId, reserveDto));
            _mockReserveRepository.Verify(repo => repo.HasConflictingReservation(
                It.IsAny<long>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Never);
            _mockReserveRepository.Verify(repo => repo.Add(It.IsAny<ReserveEntity>()), Times.Never);
        }

        [Fact]
        public async Task AddAsync_ShouldThrowNotFoundException_WhenSuiteNotFound()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            var reserveDto = new ReserveDTO(99, DateTime.Now.AddDays(1), DateTime.Now.AddDays(3));
            _mockSuiteRepository.Setup(s => s.FindById(reserveDto.SuiteId)).ReturnsAsync(null as SuiteEntity);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _reserveService.AddAsync(userId, reserveDto));
            _mockSuiteRepository.Verify(s => s.FindById(reserveDto.SuiteId), Times.Once);

            _mockReserveRepository.Verify(repo => repo.Add(It.IsAny<ReserveEntity>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_ReserveNotFound_ThrowsNotFoundException()
        {
            // Arrange
            long reserveId = 99;
            var reserveDto = new ReserveDTO(1, DateTime.Now, DateTime.Now.AddDays(1));
            _mockReserveRepository.Setup(r => r.FindById(reserveId)).ReturnsAsync((ReserveEntity)null!);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _reserveService.UpdateAsync(reserveId, reserveDto));
            _mockReserveRepository.Verify(r => r.FindById(reserveId), Times.Once);
            _mockReserveRepository.Verify(r => r.Update(It.IsAny<ReserveEntity>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_SuccessfulUpdate()
        {
            // Arrange
            long reserveId = 1;
            var reserveDto = new ReserveDTO(2, DateTime.Now.AddDays(5), DateTime.Now.AddDays(7));
            var existingReserve = new ReserveEntity { Id = reserveId, UserId = Guid.NewGuid(), SuiteId = 1, CheckIn = DateTime.Now, CheckOut = DateTime.Now.AddDays(1) };

            _mockReserveRepository.Setup(r => r.FindById(reserveId)).ReturnsAsync(existingReserve);
            _mockReserveRepository.Setup(r => r.Update(It.IsAny<ReserveEntity>())).ReturnsAsync(existingReserve);

            // Act
            await _reserveService.UpdateAsync(reserveId, reserveDto);

            // Assert
            _mockReserveRepository.Verify(r => r.FindById(reserveId), Times.Once);
            _mockReserveRepository.Verify(r => r.Update(It.Is<ReserveEntity>(rsv =>
                rsv.Id == reserveId &&
                rsv.SuiteId == reserveDto.SuiteId &&
                rsv.CheckIn == reserveDto.CheckIn &&
                rsv.CheckOut == reserveDto.CheckOut
            )), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ReserveNotFound_ThrowsNotFoundException()
        {
            // Arrange
            long reserveId = 99;
            _mockReserveRepository.Setup(r => r.FindById(reserveId)).ReturnsAsync((ReserveEntity)null!);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _reserveService.DeleteAsync(reserveId));
            _mockReserveRepository.Verify(r => r.FindById(reserveId), Times.Once);
            _mockReserveRepository.Verify(r => r.Delete(It.IsAny<ReserveEntity>()), Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_SuccessfulDelete()
        {
            // Arrange
            long reserveId = 1;
            var reserveEntity = new ReserveEntity { Id = reserveId, UserId = Guid.NewGuid(), SuiteId = 1, CheckIn = DateTime.Now, CheckOut = DateTime.Now.AddDays(1) };
            _mockReserveRepository.Setup(r => r.FindById(reserveId)).ReturnsAsync(reserveEntity);
            _mockReserveRepository.Setup(r => r.Delete(reserveEntity)).Returns(Task.CompletedTask);

            // Act
            await _reserveService.DeleteAsync(reserveId);

            // Assert
            _mockReserveRepository.Verify(r => r.FindById(reserveId), Times.Once);
            _mockReserveRepository.Verify(r => r.Delete(reserveEntity), Times.Once);
        }


        [Fact]
        public async Task FindBillingReportAsync_ShouldReturnFullReport_WhenNoParametersProvided()
        {
            // Arrange
            var expectedReport = new List<BillingReportDTO>
            {
                new(1, "Motel A", 2023, 1, 1000m),
                new(2, "Motel B", 2023, 1, 1500m)
            };

            var modelReport = new List<BillingReportProjection>
            {
                new (1,"Motel A", 2023,  1, 1000m),
                new (2, "Motel B", 2023,  1, 1500m )
            };


            _mockReserveRepository.Setup(repo => repo.FindBillingReport(null, null, null))
                .ReturnsAsync(modelReport);

            // Act
            var result = (await _reserveService.FindBillingReportAsync(null, null, null)).ToList();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal(
                expectedReport.Select(e => new { e.MotelId, e.MotelName, e.Year, e.Month, e.TotalRevenue }),
                result.Select(r => new { r.MotelId, r.MotelName, r.Year, r.Month, r.TotalRevenue }));
            _mockReserveRepository.Verify(repo => repo.FindBillingReport(null, null, null), Times.Once);
        }

        [Fact]
        public async Task FindBillingReportAsync_ShouldReturnFilteredReport_WhenYearAndMonthProvided()
        {
            // Arrange
            int year = 2024;
            int month = 6;
            long motelId = 1;
            IEnumerable<BillingReportDTO> expectedReport = new List<BillingReportDTO>
            {
                new(1, "Motel A", year, month, 3000m)
            };

            IEnumerable<BillingReportProjection> modelReport = new List<BillingReportProjection>
            {
                new ( motelId,  "Motel A",year, month, 3000m)
            };
            _mockReserveRepository.Setup(repo => repo.FindBillingReport(null, year, month))
                .ReturnsAsync(modelReport);

            // Act
            var result = await _reserveService.FindBillingReportAsync(null, year, month);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(
                expectedReport.Select(e => new { e.MotelId, e.MotelName, e.Year, e.Month, e.TotalRevenue }),
                result.Select(r => new { r.MotelId, r.MotelName, r.Year, r.Month, r.TotalRevenue }));
            _mockReserveRepository.Verify(repo => repo.FindBillingReport(null, year, month), Times.Once);
        }

        [Fact]
        public async Task FindBillingReportAsync_ShouldReturnFilteredReport_WhenMotelIdYearAndMonthProvided()
        {
            // Arrange
            long motelId = 1;
            int year = 2024;
            int month = 7;
            IEnumerable<BillingReportProjection> modelReport = new List<BillingReportProjection>
            {
                new (motelId, "Motel A", year, month, 3000m )
            };

            var expectedReport = new List<BillingReportDTO>
            {
                new(motelId, "Motel A", year, month, 3000m)
            };
            _mockMotelRepository.Setup(r => r.Exist(motelId)).ReturnsAsync(true);
            _mockReserveRepository.Setup(repo => repo.FindBillingReport(motelId, year, month))
                .ReturnsAsync(modelReport);

            // Act
            var result = await _reserveService.FindBillingReportAsync(motelId, year, month);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(
                expectedReport.Select(e => new { e.MotelId, e.MotelName, e.Year, e.Month, e.TotalRevenue }),
                result.Select(r => new { r.MotelId, r.MotelName, r.Year, r.Month, r.TotalRevenue }));
            _mockMotelRepository.Verify(r => r.Exist(motelId), Times.Once);
            _mockReserveRepository.Verify(repo => repo.FindBillingReport(motelId, year, month), Times.Once);
        }

        [Fact]
        public async Task FindBillingReportAsync_MotelNotFound_ThrowsNotFoundException()
        {
            // Arrange
            long motelId = 99;
            _mockMotelRepository.Setup(r => r.Exist(motelId)).ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _reserveService.FindBillingReportAsync(motelId, null, null));
            _mockMotelRepository.Verify(r => r.Exist(motelId), Times.Once);
            _mockReserveRepository.Verify(repo => repo.FindBillingReport(It.IsAny<long?>(), It.IsAny<int?>(), It.IsAny<int?>()), Times.Never);
        }
    }
}
