using BookMotelsApplication.DTOs;
using BookMotelsApplication.Mappers;
using BookMotelsDomain.Interfaces;

namespace BookMotelsApplication.Services
{
    public class ReserveService
    {
        private readonly IReserveRepository _reserveRepository;

        public ReserveService(IReserveRepository reserveRepository)
        {
            _reserveRepository = reserveRepository;
        }

        public async Task<IEnumerable<ReserveDTO>> FindAllAsync()
        {
            var reserves = await _reserveRepository.FindAll();
            return reserves.ToDTO();
        }

        public async Task<ReserveDTO> FindByIdAsync(long id)
        {
            var reserve = await _reserveRepository.FindById(id) ??
                          throw new Exception($"Reserva de Id: {id} não encontrada");

            return reserve.ToDTO();
        }

        public async Task<ReserveDTO> AddAsync(ReserveDTO reserveDto)
        {
            var entity = await _reserveRepository.Add(reserveDto.ToEntity());

            return entity.ToDTO();
        }

        public async Task<ReserveDTO> UpdateAsync(long id, ReserveDTO reserveDto)
        {
            var existingReserve = await _reserveRepository.FindById(id) ??
                                  throw new Exception($"Reserva de Id: {id} não encontrada");

            existingReserve.UserId = reserveDto.UserId;
            existingReserve.SuiteId = reserveDto.SuiteId;
            existingReserve.CheckIn = reserveDto.CheckIn;
            existingReserve.CheckOut = reserveDto.CheckOut;
            existingReserve.IsReserve = reserveDto.IsReserve;

            existingReserve = await _reserveRepository.Update(existingReserve);
            return existingReserve.ToDTO();
        }

        public async Task DeleteAsync(long id)
        {
            var entity = await _reserveRepository.FindById(id) ??
                                  throw new Exception($"Reserva de Id: {id} não encontrada");

            await _reserveRepository.Delete(entity);
        }
    }
}