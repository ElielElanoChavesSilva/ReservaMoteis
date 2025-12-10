using BookMotelsApplication.DTOs.Suite;
using BookMotelsApplication.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookMotelsAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SuitesController : ApiController
    {
        private readonly ISuiteService _suiteService;

        public SuitesController(ISuiteService suiteService)
        {
            _suiteService = suiteService;
        }

        /// <summary>
        /// Recupera todas as su�tes.
        /// </summary>
        /// <returns>Uma lista de todas as su�tes.</returns>
        /// <response code="200">Retorna a lista de su�tes.</response>
        /// <response code="401">Se o usuário não estiver autenticado.</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetSuiteDTO>>> FindAllAsync()
        {
            var suites = await _suiteService.FindAllAsync();
            return Ok(suites);
        }

        /// <summary>
        /// Recupera uma suíte pelo seu ID.
        /// </summary>
        /// <param name="id">O ID da suíte a ser recuperada.</param>
        /// <returns>A suíte com o ID especificado.</returns>
        /// <response code="200">Retorna a suíte.</response>
        /// <response code="401">Se o usuário não estiver autenticado.</response>
        /// <response code="404">Se nenhuma suíte com o ID fornecido for encontrada.</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<GetSuiteDTO>> FindById(long id)
        {
            var suite = await _suiteService.FindByIdAsync(id);

            return Ok(suite);
        }

        /// <summary>
        /// Atualiza uma suíte existente.
        /// </summary>
        /// <param name="id">O ID da suíte a ser atualizada.</param>
        /// <param name="suiteDto">Os detalhes de atualização da suíte.</param>
        /// <returns>Nenhum conteúdo.</returns>
        /// <response code="204">Se a suíte foi atualizada com sucesso.</response>
        /// <response code="400">Se os detalhes de atualização da suíte forem inválidos.</response>
        /// <response code="401">Se o usuário não estiver autenticado.</response>
        /// <response code="403">Se o usuário autenticado não for um Admin.</response>
        /// <response code="404">Se nenhuma suíte com o ID fornecido for encontrada.</response>
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(long id, SuiteDTO suiteDto)
        {
            await _suiteService.UpdateAsync(id, suiteDto);
            return NoContent();
        }

        /// <summary>
        /// Exclui uma suíte.
        /// </summary>
        /// <param name="id">O ID da suíte a ser excluída.</param>
        /// <returns>Nenhum conteúdo.</returns>
        /// <response code="204">Se a suíte foi excluída com sucesso.</response>
        /// <response code="401">Se o usuário não estiver autenticado.</response>
        /// <response code="403">Se o usuário autenticado não for um Admin.</response>
        /// <response code="404">Se nenhuma suíte com o ID fornecido for encontrada.</response>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(long id)
        {
            await _suiteService.DeleteAsync(id);
            return NoContent();
        }
    }
}
