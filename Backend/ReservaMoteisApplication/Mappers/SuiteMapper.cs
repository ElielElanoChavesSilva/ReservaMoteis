using BookMotelsApplication.DTOs;
using BookMotelsDomain.Entities;

namespace BookMotelsApplication.Mappers
{
    public static class SuiteMapper
    {
        public static SuiteDTO ToDTO(this SuiteEntity entity)
        {
            return new SuiteDTO
            {
                Id = entity.Id,
                Nome = entity.Nome,
                Description = entity.Description,
                PricePerPeriod = entity.PricePerPeriod,
                MaxOccupancy = entity.MaxOccupancy,
                MotelId = entity.MotelId
            };
        }

        public static SuiteEntity ToEntity(this SuiteDTO dto)
        {
            return new SuiteEntity
            {
                Id = dto.Id,
                Nome = dto.Nome,
                Description = dto.Description,
                PricePerPeriod = dto.PricePerPeriod,
                MaxOccupancy = dto.MaxOccupancy,
                MotelId = dto.MotelId
            };
        }

        public static IEnumerable<SuiteDTO> ToDTO(this IEnumerable<SuiteEntity> entities)
        {
            return entities.Select(e => e.ToDTO());
        }

        public static IEnumerable<SuiteEntity> ToEntity(this IEnumerable<SuiteDTO> dtos)
        {
            return dtos.Select(d => d.ToEntity());
        }
    }
}
