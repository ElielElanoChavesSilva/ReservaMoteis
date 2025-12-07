using BookMotelsApplication.DTOs;
using BookMotelsApplication.Mappers;
using BookMotelsDomain.Interfaces;

namespace BookMotelsApplication.Services
{
    public class SuiteService
    {
        private readonly ISuiteRepository _suiteRepository;

        public SuiteService(ISuiteRepository suiteRepository)
        {
            _suiteRepository = suiteRepository;
        }

        public async Task<IEnumerable<SuiteDTO>> FindAllAsync()
        {
            var suites = await _suiteRepository.FindAll();
            return suites.ToDTO();
        }

        public async Task<SuiteDTO> FindByIdAsync(long id)
        {
            var suite = await _suiteRepository.FindById(id) ??
                        throw new Exception($"Suíte de Id: {id} não encontrada");

            return suite.ToDTO();
        }

        public async Task<SuiteDTO> AddAsync(SuiteDTO suiteDto)
        {
            var entity = await _suiteRepository.Add(suiteDto.ToEntity());

            return entity.ToDTO();
        }

        public async Task<SuiteDTO> UpdateAsync(long id, SuiteDTO suiteDto)
        {
            var existingSuite = await _suiteRepository.FindById(id) ??
                                throw new Exception($"Suíte de Id: {id} não encontrada");

            existingSuite.Nome = suiteDto.Nome;
            existingSuite.Description = suiteDto.Description;
            existingSuite.PricePerPeriod = suiteDto.PricePerPeriod;
            existingSuite.MaxOccupancy = suiteDto.MaxOccupancy;
            existingSuite.MotelId = suiteDto.MotelId;

            existingSuite = await _suiteRepository.Update(existingSuite);
            return existingSuite.ToDTO();
        }

        public async Task DeleteAsync(long id)
        {
            var entity = await _suiteRepository.FindById(id) ??
                                throw new Exception($"Suíte de Id: {id} não encontrada");

            await _suiteRepository.Delete(entity);
        }
    }
}