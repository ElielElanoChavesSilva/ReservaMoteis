using BookMotelsApplication.DTOs;
using BookMotelsDomain.Entities;

namespace BookMotelsApplication.Mappers
{
    public static class ReserveMapper
    {
        public static ReserveDTO ToDTO(this ReserveEntity entity)
        {
            return new ReserveDTO
            {
                Id = entity.Id,
                UserId = entity.UserId,
                SuiteId = entity.SuiteId,
                CheckIn = entity.CheckIn,
                CheckOut = entity.CheckOut,
                IsReserve = entity.IsReserve
            };
        }

        public static ReserveEntity ToEntity(this ReserveDTO dto)
        {
            return new ReserveEntity
            {
                Id = dto.Id,
                UserId = dto.UserId,
                SuiteId = dto.SuiteId,
                CheckIn = dto.CheckIn,
                CheckOut = dto.CheckOut,
                IsReserve = dto.IsReserve
            };
        }

        public static IEnumerable<ReserveDTO> ToDTO(this IEnumerable<ReserveEntity> entities)
        {
            return entities.Select(e => e.ToDTO());
        }

        public static IEnumerable<ReserveEntity> ToEntity(this IEnumerable<ReserveDTO> dtos)
        {
            return dtos.Select(d => d.ToEntity());
        }
    }
}
