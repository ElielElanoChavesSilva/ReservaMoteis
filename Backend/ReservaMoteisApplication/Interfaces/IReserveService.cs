using BookMotelsApplication.DTOs.Reserve;

namespace BookMotelsApplication.Interfaces;

public interface IReserveService
{
    Task<IEnumerable<GetReserveDTO>> FindAllAsync();
    Task<GetReserveDTO> FindByIdAsync(long id);
    Task<GetReserveDTO> AddAsync(ReserveDTO reserveDto);
    Task<IEnumerable<GetReserveDTO>> FindAllByUserAsync(Guid userId);
    Task UpdateAsync(long id, ReserveDTO reserveDto);
    Task DeleteAsync(long id);
}
