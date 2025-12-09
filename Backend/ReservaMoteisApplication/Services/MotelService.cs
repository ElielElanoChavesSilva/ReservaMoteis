using BookMotelsApplication.DTOs.Motel;
using BookMotelsApplication.Interfaces;
using BookMotelsApplication.Mappers;
using BookMotelsDomain.Exceptions;
using BookMotelsDomain.Interfaces;

namespace BookMotelsApplication.Services
{
    public class MotelService : IMotelService
    {
        private readonly IMotelRepository _motelRepository;

        public MotelService(IMotelRepository motelRepository)
        {
            _motelRepository = motelRepository;
        }

        public async Task<IEnumerable<GetMotelDTO>> FindAllAsync()
        {
            var motels = await _motelRepository.FindAll();
            return motels.ToDTO();
        }

        public async Task<GetMotelDTO> FindByIdAsync(long id)
        {
            var motel = await _motelRepository.FindById(id) ??
                        throw new NotFoundException("Motel não encontrado");

            return motel.ToDTO();
        }

        public async Task<GetMotelDTO> AddAsync(MotelDTO motelDto)
        {
            var entity = await _motelRepository.Add(motelDto.ToEntity());

            return entity.ToDTO();
        }

        public async Task UpdateAsync(long id, MotelDTO motelDto)
        {
            var existingMotel = await _motelRepository.FindById(id) ??
                                throw new NotFoundException("Motel não encontrado");

            existingMotel.Name = motelDto.Name;
            existingMotel.Address = motelDto.Address;
            existingMotel.Phone = motelDto.Phone;
            existingMotel.Description = motelDto.Description;

            await _motelRepository.Update(existingMotel);
        }

        public async Task DeleteAsync(long id)
        {
            var entity = await _motelRepository.FindById(id) ??
                                throw new NotFoundException("Motel não encontrado");

            await _motelRepository.Delete(entity);
        }
    }
}
