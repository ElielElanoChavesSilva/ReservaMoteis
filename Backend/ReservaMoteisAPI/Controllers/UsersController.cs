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
        /// Recupera todos os usuários (somente Admin).
        /// </summary>
        /// <returns>Uma lista de todos os usuários.</returns>
        /// <response code="200">Retorna a lista de usuários.</response>
        /// <response code="401">Se o usuário não estiver autenticado.</response>
        /// <response code="403">Se o usuário autenticado não for um Admin.</response>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetUserDTO>>> FindAllAsync()
        {
            return Ok(await _userService.FindAllAsync());
        }

        /// <summary>
        /// Recupera um usuário pelo seu ID.
        /// </summary>
        /// <param name="id">O ID do usuário a ser recuperado.</param>
        /// <returns>O usuário com o ID especificado.</returns>
        /// <response code="200">Retorna o usuário.</response>
        /// <response code="401">Se o usuário não estiver autenticado.</response>
        /// <response code="404">Se nenhum usuário com o ID fornecido for encontrado.</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<GetUserDTO>> FindById(Guid id)
        {
            return Ok(await _userService.FindByIdAsync(id));
        }

        /// <summary>
        /// Atualiza um usuário existente.
        /// </summary>
        /// <param name="id">O ID do usuário a ser atualizado.</param>
        /// <param name="userDto">Os detalhes de atualização do usuário.</param>
        /// <returns>Nenhum conteúdo.</returns>
        /// <response code="204">Se o usuário foi atualizado com sucesso.</response>
        /// <response code="400">Se os detalhes de atualização do usuário forem invÃ¡lidos.</response>
        /// <response code="401">Se o usuário não estiver autenticado.</response>
        /// <response code="403">Se o usuário autenticado não estiver autorizado a atualizar este usuário.</response>
        /// <response code="404">Se nenhum usuário com o ID fornecido for encontrado.</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, UpdateUserDTO userDto)
        {
            await _userService.UpdateAsync(id, userDto);
            return NoContent();
        }

        /// <summary>
        /// Exclui um usuário (somente Admin).
        /// </summary>
        /// <param name="id">O ID do usuário a ser excluÃ­do.</param>
        /// <returns>Nenhum conteúdo.</returns>
        /// <response code="204">Se o usuário foi excluÃ­do com sucesso.</response>
        /// <response code="401">Se o usuário não estiver autenticado.</response>
        /// <response code="403">Se o usuário autenticado não for um Admin.</response>
        /// <response code="404">Se nenhum usuário com o ID fornecido for encontrado.</response>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await _userService.DeleteAsync(id);
            return NoContent();
        }
    }
}
