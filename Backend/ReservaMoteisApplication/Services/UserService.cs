using BookMotelsApplication.DTOs.User;
using BookMotelsApplication.Interfaces;
using BookMotelsApplication.Mappers;
using BookMotelsDomain.Entities;
using BookMotelsDomain.Exceptions;
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
        IEnumerable<UserEntity> users = await _userRepository.FindAll();
        return users.ToDTO();
    }

    public async Task<GetUserDTO> FindByIdAsync(Guid id)
    {
        UserEntity user = await _userRepository.FindById(id) ??
                          throw new NotFoundException("Usuário não encontrado");

        return user.ToDTO();
    }

    public async Task<GetUserDTO> AddAsync(UserDTO userDto)
    {
        if (await _userRepository.GetByEmailAsync(userDto.Email) is not null)
            throw new ConflictException("Este email já está cadastrado");

        userDto.Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
        UserEntity entity = userDto.ToEntity();
        entity.Id = Guid.NewGuid();

        entity = await _userRepository.Add(entity);

        return entity.ToDTO();
    }

    public async Task<GetUserDTO> UpdateAsync(Guid id, GetUserDTO userDto)
    {
        UserEntity existingUser = await _userRepository.FindById(id) ??
                                  throw new Exception("Usuï¿½rio nï¿½o encontrado");

        existingUser.Name = userDto.Name;
        existingUser.Email = userDto.Email;
        existingUser.ProfileId = userDto.ProfileId;

        existingUser = await _userRepository.Update(existingUser);
        return existingUser.ToDTO();
    }

    public async Task DeleteAsync(Guid id)
    {
        UserEntity entity = await _userRepository.FindById(id) ??
                            throw new Exception("Usuï¿½rio nï¿½o encontrado");

        await _userRepository.Delete(entity);
    }
}