using BookMotelsApplication.DTOs.Reserve;
using BookMotelsDomain.Entities;
using BookMotelsDomain.Projections;

namespace BookMotelsApplication.Mappers
{
    public static class ReserveMapper
    {
        #region | ToDTO |
        public static GetReserveByUserDTO ToDTO(this GetReserveByUserProjection projection)
        {
            return new GetReserveByUserDTO
            {
                Id = projection.Id,
                MotelName = projection.MotelName,
                SuiteName = projection.SuiteName,
                SuiteId = projection.SuiteId,
                CheckIn = projection.CheckIn,
                CheckOut = projection.CheckOut
            };
        }

        public static GetReserveDTO ToDTO(this GetReserveProjection projection)
        {
            return new GetReserveDTO
            {
                Id = projection.Id,
                UserName = projection.UserName,
                MotelName = projection.MotelName,
                TotalPrice = projection.TotalPrice,
                SuiteName = projection.SuiteName,
                SuiteId = projection.SuiteId,
                CheckIn = projection.CheckIn,
                CheckOut = projection.CheckOut
            };
        }
        public static GetReserveByUserDTO ToDTO(this ReserveEntity entity)
        {
            return new GetReserveByUserDTO
            {
                Id = entity.Id,
                TotalPrice = entity.TotalPrice,
                SuiteId = entity.SuiteId,
                CheckIn = entity.CheckIn,
                CheckOut = entity.CheckOut
            };
        }


        public static IEnumerable<GetReserveDTO> ToDTO(this IEnumerable<GetReserveProjection> entities)
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
        #endregion
    }
}
