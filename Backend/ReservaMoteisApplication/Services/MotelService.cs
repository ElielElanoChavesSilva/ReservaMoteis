using BookMotelsApplication.DTOs.Motel;
using BookMotelsApplication.Interfaces;
using BookMotelsApplication.Mappers;
using BookMotelsDomain.Exceptions;
using BookMotelsDomain.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace BookMotelsApplication.Services
{
    public class MotelService : IMotelService
    {
        private readonly IMotelRepository _motelRepository;
        private readonly IMemoryCache _cache;
        private const string AllMotelsCacheKey = "AllMotels";

        public MotelService(IMotelRepository motelRepository, IMemoryCache cache)
        {
            _motelRepository = motelRepository;
            _cache = cache;
        }

        public async Task<IEnumerable<GetMotelDTO>> FindAllAsync()
        {
            var motels = await _motelRepository.FindAll();
            return motels.ToDTO();
        }

        public async Task<IEnumerable<GetMotelDTO>> FindAllAvailableAsync()
        {
            if (_cache.TryGetValue(AllMotelsCacheKey, out IEnumerable<GetMotelDTO>? motels))
            {
                return motels!;
            }

            motels = (await _motelRepository.FindAll()).ToDTO();

            _cache.Set(AllMotelsCacheKey, motels, TimeSpan.FromMinutes(5)); // Cache for 5 minutes

            return motels;
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
            _cache.Remove(AllMotelsCacheKey); // Invalidate cache on add

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
            _cache.Remove(AllMotelsCacheKey); // Invalidate cache on update
        }

        public async Task DeleteAsync(long id)
        {
            var entity = await _motelRepository.FindById(id) ??
                                throw new NotFoundException("Motel não encontrado");

            await _motelRepository.Delete(entity);
            _cache.Remove(AllMotelsCacheKey); // Invalidate cache on delete
        }
    }
}