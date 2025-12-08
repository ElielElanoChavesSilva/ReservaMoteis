using BookMotelsApplication.DTOs.Reserve;
using BookMotelsApplication.Interfaces;
using BookMotelsApplication.Mappers;
using BookMotelsDomain.Entities;
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
            IEnumerable<ReserveEntity> reserves = await _reserveRepository.FindAll();
            return reserves.ToDTO();
        }

        public async Task<IEnumerable<GetReserveDTO>> FindAllByUserAsync(Guid userId)
        {
            IEnumerable<ReserveEntity> reserves = await _reserveRepository.FindAllByUserAsync(userId);
            return reserves.ToDTO();
        }

        public async Task<GetReserveDTO> FindByIdAsync(long id)
        {
            ReserveEntity reserve = await _reserveRepository.FindById(id) ??
                          throw new NotFoundException($"Reserva de Id: {id} nï¿½o encontrada");

            return reserve.ToDTO();
        }

        public async Task<GetReserveDTO> AddAsync(Guid userId, ReserveDTO reserveDto)
        {
            if (reserveDto.CheckOut <= reserveDto.CheckIn)
                throw new BadRequestException("O check-out deve ser maior que o check-in.");

            bool hasConflict = await _reserveRepository.HasConflictingReservation(
                reserveDto.SuiteId,
                reserveDto.CheckIn,
                reserveDto.CheckOut);

            if (hasConflict)
                throw new ConflictException("A suíte já está reservada no período informado.");

            ReserveEntity entity = reserveDto.ToEntity();
            entity.UserId = userId;
            entity = await _reserveRepository.Add(entity);

            return entity.ToDTO();
        }

        public async Task UpdateAsync(long id, ReserveDTO reserveDto)
        {
            var existingReserve = await _reserveRepository.FindById(id) ??
                                  throw new NotFoundException($"Reserva de Id: {id} não encontrada");

            existingReserve.SuiteId = reserveDto.SuiteId;
            existingReserve.CheckIn = reserveDto.CheckIn;
            existingReserve.CheckOut = reserveDto.CheckOut;

            await _reserveRepository.Update(existingReserve);
        }

        public async Task DeleteAsync(long id)
        {
            var entity = await _reserveRepository.FindById(id) ??
                                  throw new NotFoundException($"Reserva de Id: {id} não encontrada");

            await _reserveRepository.Delete(entity);
        }
    }
}