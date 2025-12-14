using BookMotelsApplication.DTOs.Reserve;
using BookMotelsApplication.Interfaces;
using BookMotelsApplication.Mappers;
using BookMotelsDomain.Entities;
using BookMotelsDomain.Exceptions;
using BookMotelsDomain.Interfaces;
using BookMotelsDomain.Projections;

namespace BookMotelsApplication.Services
{
    public class ReserveService : IReserveService
    {
        private readonly IReserveRepository _reserveRepository;
        private readonly ISuiteRepository _suiteRepository;
        private readonly IMotelRepository _motelRepository;

        public ReserveService(
            ISuiteRepository suiteRepository,
            IMotelRepository motelRepository,
            IReserveRepository reserveRepository)
        {
            _motelRepository = motelRepository;
            _suiteRepository = suiteRepository;
            _reserveRepository = reserveRepository;
        }

        public async Task<IEnumerable<GetReserveDTO>> FindAllAsync()
        {
            IEnumerable<GetReserveProjection> reserves = await _reserveRepository.FindAllProjection();

            return reserves.ToDTO();
        }

        public async Task<IEnumerable<GetReserveByUserDTO>> FindAllByUserAsync(Guid userId)
        {
            IEnumerable<GetReserveProjection> reserves = await _reserveRepository.FindAllByUser(userId);
            return reserves.ToDTO();
        }

        public async Task<GetReserveByUserDTO> FindByIdAsync(long id)
        {
            GetReserveProjection reserve = await _reserveRepository.FindByIdProjection(id) ??
                          throw new NotFoundException("Reserva não encontrada");

            return reserve.ToDTO();
        }

        public async Task<GetReserveByUserDTO> AddAsync(Guid userId, ReserveDTO reserveDto)
        {
            await ValidateReservation(reserveDto);

            SuiteEntity suite = await _suiteRepository.FindById(reserveDto.SuiteId) ??
                            throw new NotFoundException("Suíte não encontrada");

            ReserveEntity entity = reserveDto.ToEntity();

            double totalHours = (reserveDto.CheckOut - reserveDto.CheckIn).TotalHours;

            entity.TotalPrice = suite.PricePerPeriod * (decimal)totalHours;
            entity.UserId = userId;
            entity = await _reserveRepository.Add(entity);
            GetReserveByUserDTO dto = entity.ToDTO();
            dto.SuiteName = suite.Name;

            return dto;
        }

        private async Task ValidateReservation(ReserveDTO reserveDto)
        {
            if (reserveDto.CheckOut <= reserveDto.CheckIn)
                throw new BadRequestException("O check-out deve ser maior que o check-in.");

            bool hasConflict = await _reserveRepository.HasConflictingReservation(
                reserveDto.SuiteId,
                reserveDto.CheckIn,
                reserveDto.CheckOut);

            if (hasConflict)
                throw new ConflictException("A suíte já está reservada no período informado.");
        }

        public async Task UpdateAsync(long id, ReserveDTO reserveDto)
        {
            var existingReserve = await _reserveRepository.FindById(id) ??
                                  throw new NotFoundException("Reserva não encontrada");

            existingReserve.SuiteId = reserveDto.SuiteId;
            existingReserve.CheckIn = reserveDto.CheckIn;
            existingReserve.CheckOut = reserveDto.CheckOut;

            await _reserveRepository.Update(existingReserve);
        }

        public async Task<IEnumerable<BillingReportDTO>> FindBillingReportAsync(long? motelId, int? year, int? month)
        {
            if (motelId is not null)
                if (!await _motelRepository.Exist(motelId.GetValueOrDefault()))
                    throw new NotFoundException("Motel não encontrado");

            var reports = await _reserveRepository.FindBillingReport(motelId, year, month);
            return reports.Select(x => new BillingReportDTO(
            x.MotelId,
            x.MotelName,
            x.Year,
            x.Month,
            x.TotalRevenue));
        }

        public async Task DeleteAsync(long id)
        {
            var entity = await _reserveRepository.FindById(id) ??
                                  throw new NotFoundException("Reserva não encontrada");

            await _reserveRepository.Delete(entity);
        }
    }
}