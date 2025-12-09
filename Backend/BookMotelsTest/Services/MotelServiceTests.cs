using BookMotelsApplication.Services;
using BookMotelsDomain.Interfaces;
using Moq;

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

    }
}
