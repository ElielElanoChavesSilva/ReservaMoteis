using BookMotelsApplication.DTOs.User;
using BookMotelsDomain.Entities;

namespace BookMotelsApplication.Mappers
{
    public static class UserMapper
    {

        #region | ToDTO |
        public static GetUserDTO ToDTO(this UserEntity entity)
        {
            return new GetUserDTO
            {
                Id = entity.Id,
                Name = entity.Name,
                Email = entity.Email,
                ProfileId = entity.ProfileId
            };
        }

        public static IEnumerable<GetUserDTO> ToDTO(this IEnumerable<UserEntity> entities)
        {
            return entities.Select(e => e.ToDTO());
        }
        #endregion

        #region | ToEntity |
        public static UserEntity ToEntity(this GetUserDTO dto)
        {
            return new UserEntity
            {
                Id = dto.Id,
                Name = dto.Name,
                Email = dto.Email,
                ProfileId = dto.ProfileId,
            };
        }

        public static UserEntity ToEntity(this UserDTO dto)
        {
            return new UserEntity
            {
                Name = dto.Name,
                Email = dto.Email,
                ProfileId = dto.ProfileId,
                Password = dto.Password
            };
        }

        public static IEnumerable<UserEntity> ToEntity(this IEnumerable<GetUserDTO> dtos)
        {
            return dtos.Select(d => d.ToEntity());
        }

        #endregion
    }
}
