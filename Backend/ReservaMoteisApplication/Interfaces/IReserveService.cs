using BookMotelsApplication.DTOs.Reserve;

namespace BookMotelsApplication.Interfaces;

public interface IReserveService
{
    Task<IEnumerable<GetReserveDTO>> FindAllAsync();
    Task<GetReserveByUserDTO> FindByIdAsync(long id);
    Task<GetReserveByUserDTO> AddAsync(Guid userId, ReserveDTO reserveDto);
    Task<IEnumerable<GetReserveByUserDTO>> FindAllByUserAsync(Guid userId);
    Task UpdateAsync(long id, ReserveDTO reserveDto);
    Task DeleteAsync(long id);
    Task<IEnumerable<BillingReportDTO>> FindBillingReportAsync(long? motelId, int? year, int? month);

}
