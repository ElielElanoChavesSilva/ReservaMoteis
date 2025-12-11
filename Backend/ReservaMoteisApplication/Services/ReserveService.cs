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
            IEnumerable<ReserveEntity> reserves = await _reserveRepository.FindAll();
            var dtos = new List<GetReserveDTO>();

            foreach (var reserve in reserves)
            {
                SuiteEntity suite = await _suiteRepository.FindById(reserve.SuiteId)
                                    ?? throw new BadRequestException("Ocorreu um erro ao buscar as reservas");
                MotelEntity motel = await _motelRepository.FindById(suite.MotelId)
                                    ?? throw new BadRequestException("Ocorreu um erro ao buscar as reservas");

                GetReserveDTO dto = reserve.ToDTO();

                dto.SuiteName = suite.Name;
                dto.MotelName = motel.Name;
                dtos.Add(dto);
            }
            return dtos;
        }

        public async Task<IEnumerable<GetReserveDTO>> FindAllByUserAsync(Guid userId)
        {
            IEnumerable<ReserveEntity> reserves = await _reserveRepository.FindAllByUserAsync(userId);
            var dtos = new List<GetReserveDTO>();

            foreach (var reserve in reserves)
            {
                SuiteEntity? suite = await _suiteRepository.FindById(reserve.SuiteId) ??
                                    throw new NotFoundException("Ocorreu um erro ao buscar informações das reservas");


                MotelEntity motel = await _motelRepository.FindById(suite.MotelId) ??
                                    throw new NotFoundException("Ocorreu um erro ao buscar informações das reservas");

                GetReserveDTO dto = reserve.ToDTO();
                dto.SuiteName = suite.Name;
                dto.MotelName = motel.Name;
                dtos.Add(dto);
            }
            return dtos;
        }

        public async Task<GetReserveDTO> FindByIdAsync(long id)
        {
            ReserveEntity reserve = await _reserveRepository.FindById(id) ??
                          throw new NotFoundException("Reserva nÃ£o encontrada");

            SuiteEntity suite = await _suiteRepository.FindById(reserve.SuiteId) ??
                                throw new NotFoundException("SuÃ­te nÃ£o encontrada para a reserva.");

            MotelEntity motel = await _motelRepository.FindById(suite.MotelId) ??
                                throw new NotFoundException("Motel nÃ£o encontrado para a suÃ­te.");

            GetReserveDTO dto = reserve.ToDTO();
            dto.SuiteName = suite.Name;
            dto.MotelName = motel.Name;

            return dto;
        }

        public async Task<GetReserveDTO> AddAsync(Guid userId, ReserveDTO reserveDto)
        {
            await ValidateReservation(reserveDto);

            SuiteEntity suite = await _suiteRepository.FindById(reserveDto.SuiteId) ??
                            throw new NotFoundException("Suíte não encontrada");

            ReserveEntity entity = reserveDto.ToEntity();


            double totalHours = (reserveDto.CheckOut - reserveDto.CheckIn).TotalHours;

            entity.TotalPrice = suite.PricePerPeriod * (decimal)totalHours;

            entity.UserId = userId;
            entity = await _reserveRepository.Add(entity);

            return entity.ToDTO();
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
            return reports.Select(x => new BillingReportDTO
            {
                Month = x.Month,
                MotelId = x.MotelId,
                TotalRevenue = x.TotalRevenue,
                MotelName = x.MotelName,
                Year = x.Year
            });
        }

        public async Task DeleteAsync(long id)
        {
            var entity = await _reserveRepository.FindById(id) ??
                                  throw new NotFoundException("Reserva não encontrada");

            await _reserveRepository.Delete(entity);
        }
    }
}