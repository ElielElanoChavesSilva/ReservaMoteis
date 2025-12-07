using BookMotelsApplication.DTOs.User;
using BookMotelsApplication.Mappers;
using BookMotelsDomain.Interfaces;

public class UserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<UserDTO>> FindAllAsync()
    {
        var users = await _userRepository.FindAll();
        return users.ToDTO();
    }

    public async Task<UserDTO> FindByIdAsync(Guid id)
    {
        var user = await _userRepository.FindById(id) ??
                   throw new Exception($"Usuário não encontrado");

        return user.ToDTO();
    }

    public async Task<UserDTO> AddAsync(CreateUserDTO userDto)
    {
        var user = userDto.ToEntity();
        user.Id = Guid.NewGuid();

        var entity = await _userRepository.Add(user);

        return entity.ToDTO();
    }

    public async Task<UserDTO> UpdateAsync(Guid id, UserDTO userDto)
    {
        var existingUser = await _userRepository.FindById(id) ??
                           throw new Exception("Usuário não encontrado");

        existingUser.Name = userDto.Name;
        existingUser.Email = userDto.Email;
        existingUser.ProfileId = userDto.ProfileId;

        existingUser = await _userRepository.Update(existingUser);
        return existingUser.ToDTO();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _userRepository.FindById(id) ??
                           throw new Exception("Usuário não encontrado");

        await _userRepository.Delete(entity);
    }
}