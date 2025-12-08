using BookMotelsApplication.DTOs.Suite;
using BookMotelsDomain.Entities;

namespace BookMotelsApplication.Mappers
{
    public static class SuiteMapper
    {
        #region | ToDTO |
        public static IEnumerable<GetSuiteDTO> ToDTO(this IEnumerable<SuiteEntity> entities)
        {
            return entities.Select(e => e.ToDTO());
        }


        public static GetSuiteDTO ToDTO(this SuiteEntity entity)
        {
            return new GetSuiteDTO
            {
                Name = entity.Name,
                Description = entity.Description,
                PricePerPeriod = entity.PricePerPeriod,
                MaxOccupancy = entity.MaxOccupancy,
                MotelId = entity.MotelId,
                IsAvailable = entity.IsAvailable
            };
        }
        #endregion

        #region | ToEntity |
        public static IEnumerable<SuiteEntity> ToEntity(this IEnumerable<SuiteDTO> dtos)
        {
            return dtos.Select(d => d.ToEntity());
        }

        public static SuiteEntity ToEntity(this SuiteDTO dto)
        {
            return new SuiteEntity
            {
                Name = dto.Name,
                Description = dto.Description,
                PricePerPeriod = dto.PricePerPeriod,
                MaxOccupancy = dto.MaxOccupancy,
                MotelId = dto.MotelId
            };
        }
        #endregion

    }
}
