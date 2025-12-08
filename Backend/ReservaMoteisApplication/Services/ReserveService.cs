using BookMotelsApplication.DTOs.Reserve;
using BookMotelsApplication.Interfaces;
using BookMotelsApplication.Mappers;
using BookMotelsDomain.Exceptions;
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

        public async Task<IEnumerable<GetReserveDTO>> FindAllByUserAsync(Guid userId)
        {
            var reserves = await _reserveRepository.FindAllByUserAsync(userId);
            return reserves.ToDTO();
        }

        public async Task<GetReserveDTO> FindByIdAsync(long id)
        {
            var reserve = await _reserveRepository.FindById(id) ??
                          throw new NotFoundException($"Reserva de Id: {id} n�o encontrada");

            return reserve.ToDTO();
        }

        public async Task<GetReserveDTO> AddAsync(ReserveDTO reserveDto)
        {
            var hasConflict = await _reserveRepository.HasConflictingReservation(
                reserveDto.SuiteId,
                reserveDto.CheckIn,
                reserveDto.CheckOut);

            if (hasConflict)
                throw new ConflictException("A suíte já está reservada para este período.");

            var entity = await _reserveRepository.Add(reserveDto.ToEntity());

            return entity.ToDTO();
        }

        public async Task UpdateAsync(long id, ReserveDTO reserveDto)
        {
            var existingReserve = await _reserveRepository.FindById(id) ??
                                  throw new NotFoundException($"Reserva de Id: {id} n�o encontrada");

            existingReserve.UserId = reserveDto.UserId;
            existingReserve.SuiteId = reserveDto.SuiteId;
            existingReserve.CheckIn = reserveDto.CheckIn;
            existingReserve.CheckOut = reserveDto.CheckOut;

            await _reserveRepository.Update(existingReserve);
        }

        public async Task DeleteAsync(long id)
        {
            var entity = await _reserveRepository.FindById(id) ??
                                  throw new NotFoundException($"Reserva de Id: {id} n�o encontrada");

            await _reserveRepository.Delete(entity);
        }
    }
}