using BookMotelsApplication.DTOs;
using BookMotelsApplication.Mappers;
using BookMotelsDomain.Interfaces;

namespace BookMotelsApplication.Services
{
    public class MotelService
    {
        private readonly IMotelRepository _motelRepository;

        public MotelService(IMotelRepository motelRepository)
        {
            _motelRepository = motelRepository;
        }

        public async Task Add(MotelDTO dto)
        {
            var entity = dto.ToEntity();
            await _motelRepository.Add(entity);
        }

        public async Task<MotelDTO> FindById(long id)
        {
            var entity = await _motelRepository.FindById(id);

            return entity.ToDTO();
        }

        public async Task<IEnumerable<MotelDTO>> FindAll(long id)
        {
            var entities = await _motelRepository.FindAll();

            return entities.ToDTO();
        }

    }
}
