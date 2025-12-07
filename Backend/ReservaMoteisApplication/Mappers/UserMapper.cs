using BookMotelsApplication.DTOs.User;
using BookMotelsDomain.Entities;

namespace BookMotelsApplication.Mappers
{
    public static class UserMapper
    {
        public static UserDTO ToDTO(this UserEntity entity)
        {
            return new UserDTO
            {
                Id = entity.Id,
                Name = entity.Name,
                Email = entity.Email,
                ProfileId = entity.ProfileId
            };
        }


        public static UserEntity ToEntity(this UserDTO dto)
        {
            return new UserEntity
            {
                Id = dto.Id,
                Name = dto.Name,
                Email = dto.Email,
                ProfileId = dto.ProfileId,
            };
        }

        public static UserEntity ToEntity(this CreateUserDTO dto)
        {
            return new UserEntity
            {
                Name = dto.Name,
                Email = dto.Email,
                ProfileId = dto.ProfileId,
                Password = dto.Password
            };
        }

        public static IEnumerable<UserDTO> ToDTO(this IEnumerable<UserEntity> entities)
        {
            return entities.Select(e => e.ToDTO());
        }

        public static IEnumerable<UserEntity> ToEntity(this IEnumerable<UserDTO> dtos)
        {
            return dtos.Select(d => d.ToEntity());
        }
    }
}
