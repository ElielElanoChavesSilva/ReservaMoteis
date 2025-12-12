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
        /// Recupera todas as suítes.
        /// </summary>
        /// <returns>Uma lista de todas as suítes.</returns>
        /// <response code="200">Retorna a lista de suítes.</response>
        /// <response code="401">Se o usuÃ¡rio nÃ£o estiver autenticado.</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetSuiteDTO>>> FindAllAsync()
        {
            return Ok(await _suiteService.FindAllAsync());
        }

        /// <summary>
        /// Recupera uma suÃ­te pelo seu ID.
        /// </summary>
        /// <param name="id">O ID da suÃ­te a ser recuperada.</param>
        /// <returns>A suÃ­te com o ID especificado.</returns>
        /// <response code="200">Retorna a suÃ­te.</response>
        /// <response code="401">Se o usuÃ¡rio nÃ£o estiver autenticado.</response>
        /// <response code="404">Se nenhuma suÃ­te com o ID fornecido for encontrada.</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<GetSuiteDTO>> FindById(long id)
        {
            return Ok(await _suiteService.FindByIdAsync(id));
        }

        /// <summary>
        /// Atualiza uma suÃ­te existente.
        /// </summary>
        /// <param name="id">O ID da suÃ­te a ser atualizada.</param>
        /// <param name="suiteDto">Os detalhes de atualizaÃ§Ã£o da suÃ­te.</param>
        /// <returns>Nenhum conteÃºdo.</returns>
        /// <response code="204">Se a suÃ­te foi atualizada com sucesso.</response>
        /// <response code="400">Se os detalhes de atualizaÃ§Ã£o da suÃ­te forem invÃ¡lidos.</response>
        /// <response code="401">Se o usuÃ¡rio nÃ£o estiver autenticado.</response>
        /// <response code="403">Se o usuÃ¡rio autenticado nÃ£o for um Admin.</response>
        /// <response code="404">Se nenhuma suÃ­te com o ID fornecido for encontrada.</response>
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(long id, SuiteDTO suiteDto)
        {
            await _suiteService.UpdateAsync(id, suiteDto);
            return NoContent();
        }

        /// <summary>
        /// Exclui uma suÃ­te.
        /// </summary>
        /// <param name="id">O ID da suÃ­te a ser excluÃ­da.</param>
        /// <returns>Nenhum conteÃºdo.</returns>
        /// <response code="204">Se a suÃ­te foi excluÃ­da com sucesso.</response>
        /// <response code="401">Se o usuÃ¡rio nÃ£o estiver autenticado.</response>
        /// <response code="403">Se o usuÃ¡rio autenticado nÃ£o for um Admin.</response>
        /// <response code="404">Se nenhuma suÃ­te com o ID fornecido for encontrada.</response>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(long id)
        {
            await _suiteService.DeleteAsync(id);
            return NoContent();
        }
    }
}
