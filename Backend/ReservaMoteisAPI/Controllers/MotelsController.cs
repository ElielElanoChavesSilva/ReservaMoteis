using BookMotelsApplication.DTOs.Motel;
using BookMotelsApplication.DTOs.Suite;
using BookMotelsApplication.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookMotelsAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MotelsController : ApiController
    {
        private readonly IMotelService _motelService;
        private readonly ISuiteService _suiteService;

        public MotelsController(
            IMotelService motelService,
            ISuiteService suiteService)
        {
            _motelService = motelService;
            _suiteService = suiteService;
        }

        /// <summary>
        /// Recupera todos os motéis.
        /// </summary>
        /// <returns>Uma lista de todos os motéis.</returns>
        /// <response code="200">Retorna a lista de motéis.</response>
        /// <response code="401">Se o usuário não estiver autenticado.</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetMotelDTO>>> FindAllAsync()
        {
            return Ok(await _motelService.FindAllAsync());
        }

        /// <summary>
        /// Recupera um motel pelo seu ID.
        /// </summary>
        /// <param name="motelId">O ID do motel a ser recuperado.</param>
        /// <returns>O motel com o ID especificado.</returns>
        /// <response code="200">Retorna o motel.</response>
        /// <response code="401">Se o usuário não estiver autenticado.</response>
        /// <response code="404">Se nenhum motel com o ID fornecido for encontrado.</response>
        [HttpGet("{motelId}")]
        public async Task<ActionResult<GetMotelDTO>> FindById(long motelId)
        {
            return Ok(await _motelService.FindByIdAsync(motelId));
        }

        /// <summary>
        /// Cria um novo motel.
        /// </summary>
        /// <param name="motelDto">Os detalhes de criação do motel.</param>
        /// <returns>O motel recém-criado.</returns>
        /// <response code="201">Retorna o motel recém-criado.</response>
        /// <response code="400">Se os detalhes de criação do motel forem inválidos.</response>
        /// <response code="401">Se o usuário não estiver autenticado.</response>
        /// <response code="403">Se o usuário autenticado não for um Administrador.</response>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<GetMotelDTO>> AddAsync(MotelDTO motelDto)
        {
            var newMotel = await _motelService.AddAsync(motelDto);
            return CreatedAtAction(nameof(FindById), new { motelId = newMotel.Id }, newMotel);
        }

        /// <summary>
        /// Atualiza um motel existente.
        /// </summary>
        /// <param name="motelId">O ID do motel a ser atualizado.</param>
        /// <param name="motelDto">Os detalhes de atualização do motel.</param>
        /// <returns>Nenhum conteúdo.</returns>
        /// <response code="204">Se o motel foi atualizado com sucesso.</response>
        /// <response code="400">Se os detalhes de atualização do motel forem inválidos.</response>
        /// <response code="401">Se o usuário não estiver autenticado.</response>
        /// <response code="403">Se o usuário autenticado não for um Administrador.</response>
        /// <response code="404">Se nenhum motel com o ID fornecido for encontrado.</response>
        [Authorize(Roles = "Admin")]
        [HttpPut("{motelId}")]
        public async Task<IActionResult> UpdateAsync(long motelId, MotelDTO motelDto)
        {
            await _motelService.UpdateAsync(motelId, motelDto);
            return NoContent();
        }

        /// <summary>
        /// Exclui um motel.
        /// </summary>
        /// <param name="motelId">O ID do motel a ser excluído.</param>
        /// <returns>Nenhum conteúdo.</returns>
        /// <response code="204">Se o motel foi excluído com sucesso.</response>
        /// <response code="401">Se o usuário não estiver autenticado.</response>
        /// <response code="403">Se o usuário autenticado não for um Administrador.</response>
        /// <response code="404">Se nenhum motel com o ID fornecido for encontrado.</response>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{motelId}")]
        public async Task<IActionResult> DeleteAsync(long motelId)
        {
            await _motelService.DeleteAsync(motelId);
            return NoContent();
        }

        /// <summary>
        /// Recupera todas as suítes disponíveis para um motel específico dentro de um determinado período.
        /// </summary>
        /// <param name="motelId">O ID do motel.</param>
        /// <param name="name">Opcional: Filtrar por nome da suíte.</param>
        /// <param name="checkin">Opcional: Data de check-in para disponibilidade.</param>
        /// <param name="checkout">Opcional: Data de check-out para disponibilidade.</param>
        /// <returns>Uma lista de suítes disponíveis.</returns>
        /// <response code="200">Retorna a lista de suítes disponíveis.</response>
        /// <response code="401">Se o usuário não estiver autenticado.</response>
        /// <response code="404">Se o motel com o ID fornecido não for encontrado.</response>
        [HttpGet("{motelId}/suites/available")]
        public async Task<ActionResult<IEnumerable<GetSuiteDTO>>> FindAllAvailable(long motelId, [FromQuery] string? name,
            [FromQuery] DateTime? checkin, [FromQuery] DateTime? checkout)
        {
            return Ok(await _suiteService.FindAllAvailable(motelId, name, checkin, checkout));
        }

        /// <summary>
        /// Cria uma nova suíte para um motel específico.
        /// </summary>
        /// <param name="motelId">O ID do motel ao qual adicionar a suíte.</param>
        /// <param name="suiteDto">Os detalhes de criação da suíte.</param>
        /// <returns>A suíte recém-criada.</returns>
        /// <response code="201">Retorna o ID da suíte recém-criada.</response>
        /// <response code="400">Se os detalhes de criação da suíte forem inválidos.</response>
        /// <response code="401">Se o usuário não estiver autenticado.</response>
        /// <response code="403">Se o usuário autenticado não for um Administrador.</response>
        /// <response code="404">Se o motel com o ID fornecido não for encontrado.</response>
        [Authorize(Roles = "Admin")]
        [HttpPost("{motelId}/suites")]
        public async Task<ActionResult<GetSuiteDTO>> AddAsync(long motelId, SuiteDTO suiteDto)
        {
            var newSuite = await _suiteService.AddAsync(motelId, suiteDto);
            return Created($"api/Suites/{newSuite.Id}", newSuite.Id);
        }
    }
}
