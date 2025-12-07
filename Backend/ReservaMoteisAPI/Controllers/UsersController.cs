using BookMotelsApplication.DTOs.User;
using Microsoft.AspNetCore.Mvc;

namespace BookMotelsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
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

        [HttpPost]
        public async Task<ActionResult<GetUserDTO>> AddAsync(UserDTO userDto)
        {
            var newUser = await _userService.AddAsync(userDto);
            return CreatedAtAction(nameof(FindById), new { id = newUser.Id }, newUser);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, GetUserDTO userDto)
        {
            var updatedUser = await _userService.UpdateAsync(id, userDto);

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
