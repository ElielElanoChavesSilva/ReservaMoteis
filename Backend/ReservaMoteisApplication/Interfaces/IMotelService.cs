using BookMotelsApplication.DTOs.Motel;
namespace BookMotelsApplication.Interfaces;

public interface IMotelService
{
    Task<IEnumerable<GetMotelDTO>> FindAllAsync();
    Task<GetMotelDTO> FindByIdAsync(long id);
    Task<GetMotelDTO> AddAsync(MotelDTO motelDto);
    Task UpdateAsync(long id, MotelDTO motelDto);
    Task DeleteAsync(long id);
}
