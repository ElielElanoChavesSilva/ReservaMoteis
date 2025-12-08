using BookMotelsApplication.DTOs.Motel;
using BookMotelsDomain.DTOs;

namespace BookMotelsApplication.Interfaces;

public interface IMotelService
{
    Task<IEnumerable<GetMotelDTO>> FindAllAsync();
    Task<GetMotelDTO> FindByIdAsync(long id);
    Task<GetMotelDTO> AddAsync(MotelDTO motelDto);
    Task UpdateAsync(long id, GetMotelDTO motelDto);
    Task DeleteAsync(long id);
    Task<IEnumerable<BillingReportDTO>> FindBillingReportAsync(long? motelId, int? year, int? month);
}
