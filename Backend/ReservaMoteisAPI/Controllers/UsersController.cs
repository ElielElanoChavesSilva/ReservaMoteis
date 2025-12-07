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
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAll()
        {
            var users = await _userService.FindAllAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> FindByIdAsync(Guid id)
        {
            var user = await _userService.FindByIdAsync(id);

            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<UserDTO>> AddAsync(CreateUserDTO userDto)
        {
            var newUser = await _userService.AddAsync(userDto);
            return CreatedAtAction(nameof(FindByIdAsync), new { id = newUser.Id }, newUser);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UserDTO userDto)
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
