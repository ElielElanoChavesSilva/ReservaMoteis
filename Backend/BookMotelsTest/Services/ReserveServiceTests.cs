using BookMotelsApplication.DTOs.Reserve;
using BookMotelsApplication.Services;
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

        public ReserveServiceTests()
        {
            _mockReserveRepository = new Mock<IReserveRepository>();
            _reserveService = new ReserveService(_mockReserveRepository.Object);
        }

        [Fact]
        public async Task AddAsync_ShouldAddReserve_WhenNoConflictExists()
        {
            // Arrange
            var reserveDto = new ReserveDTO
            {
                UserId = Guid.NewGuid(),
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
            var result = await _reserveService.AddAsync(reserveDto);

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
            var reserveDto = new ReserveDTO
            {
                UserId = Guid.NewGuid(),
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
            await Assert.ThrowsAsync<ConflictException>(() => _reserveService.AddAsync(reserveDto));
            _mockReserveRepository.Verify(repo => repo.HasConflictingReservation(
                reserveDto.SuiteId,
                reserveDto.CheckIn,
                reserveDto.CheckOut), Times.Once);
            _mockReserveRepository.Verify(repo => repo.Add(It.IsAny<ReserveEntity>()), Times.Never);
        }
    }
}