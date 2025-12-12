using BookMotelsApplication.DTOs.User;
using BookMotelsApplication.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookMotelsAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ApiController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Recupera todos os usu·rios (somente Admin).
        /// </summary>
        /// <returns>Uma lista de todos os usu·rios.</returns>
        /// <response code="200">Retorna a lista de usu·rios.</response>
        /// <response code="401">Se o usu·rio n„o estiver autenticado.</response>
        /// <response code="403">Se o usu·rio autenticado n„o for um Admin.</response>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetUserDTO>>> FindAllAsync()
        {
            return Ok(await _userService.FindAllAsync());
        }

        /// <summary>
        /// Recupera um usu·rio pelo seu ID.
        /// </summary>
        /// <param name="id">O ID do usu·rio a ser recuperado.</param>
        /// <returns>O usu·rio com o ID especificado.</returns>
        /// <response code="200">Retorna o usu·rio.</response>
        /// <response code="401">Se o usu·rio n„o estiver autenticado.</response>
        /// <response code="404">Se nenhum usu·rio com o ID fornecido for encontrado.</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<GetUserDTO>> FindById(Guid id)
        {
            return Ok(await _userService.FindByIdAsync(id));
        }


        /// <summary>
        /// Atualiza um usu·rio existente.
        /// </summary>
        /// <param name="id">O ID do usu·rio a ser atualizado.</param>
        /// <param name="userDto">Os detalhes de atualiza√ß√£o do usu·rio.</param>
        /// <returns>Nenhum conte˙do.</returns>
        /// <response code="204">Se o usu·rio foi atualizado com sucesso.</response>
        /// <response code="400">Se os detalhes de atualiza√ß√£o do usu·rio forem inv√°lidos.</response>
        /// <response code="401">Se o usu·rio n„o estiver autenticado.</response>
        /// <response code="403">Se o usu·rio autenticado n„o estiver autorizado a atualizar este usu·rio.</response>
        /// <response code="404">Se nenhum usu·rio com o ID fornecido for encontrado.</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, GetUserDTO userDto)
        {
            await _userService.UpdateAsync(id, userDto);
            return NoContent();
        }

        /// <summary>
        /// Exclui um usu·rio (somente Admin).
        /// </summary>
        /// <param name="id">O ID do usu·rio a ser exclu√≠do.</param>
        /// <returns>Nenhum conte˙do.</returns>
        /// <response code="204">Se o usu·rio foi exclu√≠do com sucesso.</response>
        /// <response code="401">Se o usu·rio n„o estiver autenticado.</response>
        /// <response code="403">Se o usu·rio autenticado n„o for um Admin.</response>
        /// <response code="404">Se nenhum usu·rio com o ID fornecido for encontrado.</response>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await _userService.DeleteAsync(id);
            return NoContent();
        }
    }
}
