using BookMotelsApplication.DTOs.User;
using BookMotelsApplication.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookMotelsAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetUserDTO>>> FindAllAsync()
        {
            var users = await _userService.FindAllAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetUserDTO>> FindById(Guid id)
        {
            var user = await _userService.FindByIdAsync(id);

            return Ok(user);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, GetUserDTO userDto)
        {
            await _userService.UpdateAsync(id, userDto);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await _userService.DeleteAsync(id);
            return NoContent();
        }

    }
}
