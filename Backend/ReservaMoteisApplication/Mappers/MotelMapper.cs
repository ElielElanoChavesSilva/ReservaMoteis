using BookMotelsApplication.DTOs;
using BookMotelsDomain.Entities;

namespace BookMotelsApplication.Mappers
{
    public static class MotelMapper
    {
        public static MotelDTO ToDTO(this MotelEntity entity)
        {
            return new MotelDTO
            {
                Id = entity.Id,
                Nome = entity.Nome,
                Address = entity.Address,
                Phone = entity.Phone,
                Description = entity.Description,
                Suites = entity.Suites.Select(s => s.ToDTO()).ToList()
            };
        }

        public static MotelEntity ToEntity(this MotelDTO dto)
        {
            return new MotelEntity
            {
                Id = dto.Id,
                Nome = dto.Nome,
                Address = dto.Address,
                Phone = dto.Phone,
                Description = dto.Description,
                Suites = dto.Suites.Select(s => s.ToEntity()).ToList()
            };
        }

        public static IEnumerable<MotelDTO> ToDTO(this IEnumerable<MotelEntity> entities)
        {
            return entities.Select(e => e.ToDTO());
        }

        public static IEnumerable<MotelEntity> ToEntity(this IEnumerable<MotelDTO> dtos)
        {
            return dtos.Select(d => d.ToEntity());
        }
    }
}
