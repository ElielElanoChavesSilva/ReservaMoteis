using BookMotelsApplication.DTOs.Reserve;
using BookMotelsApplication.Interfaces;
using BookMotelsDomain.Projections;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookMotelsAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ReservesController : ApiController
    {
        private readonly IReserveService _reserveService;

        public ReservesController(IReserveService reserveService)
        {
            _reserveService = reserveService;
        }

        /// <summary>
        /// Recupera todas as reservas (somente Admin).
        /// </summary>
        /// <returns>Uma lista de todas as reservas.</returns>
        /// <response code="200">Retorna a lista de reservas.</response>
        /// <response code="401">Se o usuário não estiver autenticado.</response>
        /// <response code="403">Se o usuário autenticado não for um Admin.</response>
        [Authorize(Roles = "Admin")]
        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<GetReserveDTO>>> FindAllAsync()
        {
            return Ok(await _reserveService.FindAllAsync());
        }

        /// <summary>
        /// Retorna o faturamento(somente Admin).
        /// </summary>
        /// <param name="motelId">Opcional: Filtrar por ID do motel.</param>
        /// <param name="year">Opcional: Filtrar por ano.</param>
        /// <param name="month">Opcional: Filtrar por mÃªs.</param>
        /// <returns>Um relatório de faturamento.</returns>
        /// <response code="200">Retorna o relatório de faturamento.</response>
        /// <response code="401">Se o usuário não estiver autenticado.</response>
        /// <response code="403">Se o usuário autenticado não for um Admin.</response>
        /// <response code="404">Se o motel especificado não for encontrado.</response>
        [Authorize(Roles = "Admin")]
        [HttpGet("billing-report")]
        public async Task<ActionResult<IEnumerable<BillingReportProjection>>> FindBillingReport([FromQuery] long? motelId,
            [FromQuery] int? year, [FromQuery] int? month)
        {
            return Ok(await _reserveService.FindBillingReportAsync(motelId, year, month));
        }


        /// <summary>
        /// Recupera todas as reservas para o usuário autenticado.
        /// </summary>
        /// <returns>Uma lista de reservas para o usuário atual.</returns>
        /// <response code="200">Retorna a lista de reservas.</response>
        /// <response code="401">Se o usuário não estiver autenticado.</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetReserveByUserDTO>>> FindAllByUserAsync()
        {
            return Ok(await _reserveService.FindAllByUserAsync(LoggedUserId));
        }

        /// <summary>
        /// Recupera uma reserva pelo seu ID.
        /// </summary>
        /// <param name="id">O ID da reserva a ser recuperada.</param>
        /// <returns>A reserva com o ID especificado.</returns>
        /// <response code="200">Retorna a reserva.</response>
        /// <response code="401">Se o usuário não estiver autenticado.</response>
        /// <response code="404">Se nenhuma reserva com o ID fornecido for encontrada.</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<GetReserveDTO>> FindById(long id)
        {
            return Ok(await _reserveService.FindByIdAsync(id));
        }

        /// <summary>
        /// Cria uma nova reserva.
        /// </summary>
        /// <param name="reserveDto">Os detalhes de criação da reserva.</param>
        /// <returns>A reserva recém-criada.</returns>
        /// <response code="201">Retorna a reserva recém-criada.</response>
        /// <response code="400">Se os detalhes de criação da reserva forem inválidos.</response>
        /// <response code="401">Se o usuário não estiver autenticado.</response>
        /// <response code="404">Se a suíte ou motel associado ÃƒÂ  reserva não for encontrado.</response>
        /// <response code="409">Se houver um conflito com uma reserva existente.</response>
        [HttpPost]
        public async Task<ActionResult<ReserveDTO>> AddAsync(ReserveDTO reserveDto)
        {
            var newReserve = await _reserveService.AddAsync(LoggedUserId, reserveDto);
            return CreatedAtAction(nameof(FindById), new { id = newReserve.Id }, newReserve);
        }

        /// <summary>
        /// Atualiza uma reserva existente.
        /// </summary>
        /// <param name="id">O ID da reserva a ser atualizada.</param>
        /// <param name="reserveDto">Os detalhes de atualização da reserva.</param>
        /// <returns>Nenhum conteúdo.</returns>
        /// <response code="204">Se a reserva foi atualizada com sucesso.</response>
        /// <response code="400">Se os detalhes de atualização da reserva forem inválidos.</response>
        /// <response code="401">Se o usuário não estiver autenticado.</response>
        /// <response code="404">Se nenhuma reserva com o ID fornecido for encontrada.</response>
        /// <response code="409">Se houver um conflito com uma reserva existente.</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(long id, ReserveDTO reserveDto)
        {
            await _reserveService.UpdateAsync(id, reserveDto);
            return NoContent();
        }

        /// <summary>
        /// Exclui uma reserva.
        /// </summary>
        /// <param name="id">O ID da reserva a ser excluída.</param>
        /// <returns>Nenhum conteúdo.</returns>
        /// <response code="204">Se a reserva foi excluída com sucesso.</response>
        /// <response code="401">Se o usuário não estiver autenticado.</response>
        /// <response code="404">Se nenhuma reserva com o ID fornecido for encontrada.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(long id)
        {
            await _reserveService.DeleteAsync(id);
            return NoContent();
        }
    }
}
