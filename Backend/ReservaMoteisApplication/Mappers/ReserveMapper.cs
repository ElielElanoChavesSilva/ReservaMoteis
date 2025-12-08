using BookMotelsApplication.DTOs.Reserve;
using BookMotelsDomain.Entities;

namespace BookMotelsApplication.Mappers
{
    public static class ReserveMapper
    {
        #region | ToDTO |
        public static GetReserveDTO ToDTO(this ReserveEntity entity)
        {
            return new GetReserveDTO
            {
                Id = entity.Id,
                UserId = entity.UserId,
                SuiteId = entity.SuiteId,
                CheckIn = entity.CheckIn,
                CheckOut = entity.CheckOut,
            };
        }

        public static IEnumerable<GetReserveDTO> ToDTO(this IEnumerable<ReserveEntity> entities)
        {
            return entities.Select(e => e.ToDTO());
        }
        #endregion


        #region | ToEntity |
        public static ReserveEntity ToEntity(this ReserveDTO dto)
        {
            return new ReserveEntity
            {
                SuiteId = dto.SuiteId,
                CheckIn = dto.CheckIn,
                CheckOut = dto.CheckOut,
            };
        }

        public static IEnumerable<ReserveEntity> ToEntity(this IEnumerable<ReserveDTO> dtos)
        {
            return dtos.Select(d => d.ToEntity());
        }
        #endregion
    }
}
