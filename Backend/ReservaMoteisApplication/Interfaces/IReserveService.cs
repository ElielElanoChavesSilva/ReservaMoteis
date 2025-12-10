using BookMotelsApplication.DTOs.Reserve;
using BookMotelsDomain;

namespace BookMotelsApplication.Interfaces;

public interface IReserveService
{
    Task<IEnumerable<GetReserveDTO>> FindAllAsync();
    Task<GetReserveDTO> FindByIdAsync(long id);
    Task<GetReserveDTO> AddAsync(Guid userId, ReserveDTO reserveDto);
    Task<IEnumerable<GetReserveDTO>> FindAllByUserAsync(Guid userId);
    Task UpdateAsync(long id, ReserveDTO reserveDto);
    Task DeleteAsync(long id);
    Task<IEnumerable<BillingReportDTO>> FindBillingReportAsync(long? motelId, int? year, int? month);

}
