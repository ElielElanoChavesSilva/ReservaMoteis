using BookMotelsApplication.DTOs.Reserve;
using BookMotelsApplication.Interfaces;
using BookMotelsApplication.Mappers;
using BookMotelsDomain.Interfaces;

namespace BookMotelsApplication.Services
{
    public class ReserveService : IReserveService
    {
        private readonly IReserveRepository _reserveRepository;

        public ReserveService(IReserveRepository reserveRepository)
        {
            _reserveRepository = reserveRepository;
        }

        public async Task<IEnumerable<GetReserveDTO>> FindAllAsync()
        {
            var reserves = await _reserveRepository.FindAll();
            return reserves.ToDTO();
        }

        public async Task<GetReserveDTO> FindByIdAsync(long id)
        {
            var reserve = await _reserveRepository.FindById(id) ??
                          throw new Exception($"Reserva de Id: {id} n�o encontrada");

            return reserve.ToDTO();
        }

        public async Task<GetReserveDTO> AddAsync(ReserveDTO reserveDto)
        {
            var entity = await _reserveRepository.Add(reserveDto.ToEntity());

            return entity.ToDTO();
        }

        public async Task UpdateAsync(long id, ReserveDTO reserveDto)
        {
            var existingReserve = await _reserveRepository.FindById(id) ??
                                  throw new Exception($"Reserva de Id: {id} n�o encontrada");

            existingReserve.UserId = reserveDto.UserId;
            existingReserve.SuiteId = reserveDto.SuiteId;
            existingReserve.CheckIn = reserveDto.CheckIn;
            existingReserve.CheckOut = reserveDto.CheckOut;
            existingReserve.IsReserve = reserveDto.IsReserve;

            await _reserveRepository.Update(existingReserve);
        }

        public async Task DeleteAsync(long id)
        {
            var entity = await _reserveRepository.FindById(id) ??
                                  throw new Exception($"Reserva de Id: {id} n�o encontrada");

            await _reserveRepository.Delete(entity);
        }
    }
}