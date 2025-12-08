using BookMotelsApplication.DTOs.Suite;

namespace BookMotelsApplication.Interfaces;

public interface ISuiteService
{
    Task<IEnumerable<GetSuiteDTO>> FindAllAsync();
    Task<IEnumerable<GetSuiteDTO>> FindAllAvailable(long modelId, string? name, DateTime? chekin, DateTime? chekout);
    Task<GetSuiteDTO> FindByIdAsync(long id);
    Task<GetSuiteDTO> AddAsync(SuiteDTO suiteDto);
    Task UpdateAsync(long id, SuiteDTO suiteDto);
    Task DeleteAsync(long id);
}
