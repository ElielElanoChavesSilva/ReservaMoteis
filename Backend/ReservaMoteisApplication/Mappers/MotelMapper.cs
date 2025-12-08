using BookMotelsApplication.DTOs.Motel;
using BookMotelsDomain.Entities;

namespace BookMotelsApplication.Mappers
{
    public static class MotelMapper
    {
        public static GetMotelDTO ToDTO(this MotelEntity entity)
        {
            return new GetMotelDTO
            {
                Id = entity.Id,
                Name = entity.Name,
                Address = entity.Address,
                Phone = entity.Phone,
                Description = entity.Description,
                Suites = entity.Suites?.Select(s => s.ToDTO()).ToList()
            };
        }

        public static MotelEntity ToEntity(this MotelDTO dto)
        {
            return new MotelEntity
            {
                Name = dto.Name,
                Address = dto.Address,
                Phone = dto.Phone,
                Description = dto.Description,
                Suites = dto.Suites?.Select(s => s.ToEntity()).ToList()
            };
        }

        #region | ToEntity |

        public static IEnumerable<GetMotelDTO> ToDTO(this IEnumerable<MotelEntity> entities)
        {
            return entities.Select(e => e.ToDTO());
        }
        #endregion
    }
}
