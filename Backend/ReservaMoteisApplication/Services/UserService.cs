using BookMotelsApplication.DTOs.User;
using BookMotelsApplication.Interfaces;
using BookMotelsApplication.Mappers;
using BookMotelsDomain.Interfaces;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<GetUserDTO>> FindAllAsync()
    {
        var users = await _userRepository.FindAll();
        return users.ToDTO();
    }

    public async Task<GetUserDTO> FindByIdAsync(Guid id)
    {
        var user = await _userRepository.FindById(id) ??
                   throw new Exception($"Usuï¿½rio nï¿½o encontrado");

        return user.ToDTO();
    }

    public async Task<GetUserDTO> AddAsync(UserDTO userDto)
    {
        if (await _userRepository.GetByEmailAsync(userDto.Email) is not null)
            throw new Exception("Este email já está cadastrado");

        userDto.Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
        var user = userDto.ToEntity();
        user.Id = Guid.NewGuid();

        var entity = await _userRepository.Add(user);

        return entity.ToDTO();
    }

    public async Task<GetUserDTO> UpdateAsync(Guid id, GetUserDTO userDto)
    {
        var existingUser = await _userRepository.FindById(id) ??
                           throw new Exception("Usuï¿½rio nï¿½o encontrado");

        existingUser.Name = userDto.Name;
        existingUser.Email = userDto.Email;
        existingUser.ProfileId = userDto.ProfileId;

        existingUser = await _userRepository.Update(existingUser);
        return existingUser.ToDTO();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _userRepository.FindById(id) ??
                           throw new Exception("Usuï¿½rio nï¿½o encontrado");

        await _userRepository.Delete(entity);
    }
}