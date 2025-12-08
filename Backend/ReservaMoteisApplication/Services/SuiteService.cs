using BookMotelsApplication.DTOs.Suite;
using BookMotelsApplication.Interfaces;
using BookMotelsApplication.Mappers;
using BookMotelsDomain.Entities;
using BookMotelsDomain.Exceptions;
using BookMotelsDomain.Interfaces;

namespace BookMotelsApplication.Services
{
    public class SuiteService : ISuiteService
    {
        private readonly ISuiteRepository _suiteRepository;
        private readonly IMotelRepository _motelRepository;

        public SuiteService(
            IMotelRepository motelRepository,
            ISuiteRepository suiteRepository)
        {
            _motelRepository = motelRepository;
            _suiteRepository = suiteRepository;
        }

        public async Task<IEnumerable<GetSuiteDTO>> FindAllAsync()
        {
            IEnumerable<SuiteEntity> suites = await _suiteRepository.FindAll();
            return suites.ToDTO();
        }

        public async Task<IEnumerable<GetSuiteDTO>> FindAllAvailable(long modelId, string? name, DateTime? chekin, DateTime? chekout)
        {
            if (!await _motelRepository.Exist(modelId))
                throw new NotFoundException("Suíte não encontrada");

            IEnumerable<SuiteEntity> suites = await _suiteRepository.FindAllAvailable(modelId, name, chekin, chekout);

            return suites.ToDTO();
        }

        public async Task<GetSuiteDTO> FindByIdAsync(long id)
        {
            SuiteEntity suite = await _suiteRepository.FindById(id) ??
                                throw new NotFoundException("Suíte não encontrada");

            return suite.ToDTO();
        }

        public async Task<GetSuiteDTO> AddAsync(long motelId, SuiteDTO suiteDto)
        {
            SuiteEntity entity = suiteDto.ToEntity();
            entity.MotelId = motelId;
            entity = await _suiteRepository.Add(entity);

            return entity.ToDTO();
        }

        public async Task UpdateAsync(long id, SuiteDTO suiteDto)
        {
            SuiteEntity existingSuite = await _suiteRepository.FindById(id) ??
                                        throw new NotFoundException("Suíte não encontrada");

            existingSuite.Name = suiteDto.Name;
            existingSuite.Description = suiteDto.Description;
            existingSuite.PricePerPeriod = suiteDto.PricePerPeriod;
            existingSuite.MaxOccupancy = suiteDto.MaxOccupancy;

            await _suiteRepository.Update(existingSuite);
        }

        public async Task DeleteAsync(long id)
        {
            SuiteEntity entity = await _suiteRepository.FindById(id) ??
                                 throw new NotFoundException("Suíte não encontrada");

            await _suiteRepository.Delete(entity);
        }
    }
}