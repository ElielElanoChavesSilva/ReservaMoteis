using BookMotelsApplication.DTOs.User;

namespace BookMotelsApplication.Interfaces;

public interface IUserService
{
    Task<IEnumerable<GetUserDTO>> FindAllAsync();
    Task<GetUserDTO> FindByIdAsync(Guid id);
    Task<GetUserDTO> AddAsync(UserDTO userDto);
    Task<GetUserDTO> UpdateAsync(Guid id, UpdateUserDTO userDto);
    Task DeleteAsync(Guid id);
}
