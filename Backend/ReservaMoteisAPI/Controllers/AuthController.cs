using BookMotelsApplication.DTOs.Auth;
using BookMotelsApplication.DTOs.User;
using BookMotelsApplication.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookMotelsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthenticationService _authService;
        private readonly UserService _userService;

        public AuthController(
            UserService userService,
        AuthenticationService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        [HttpPost("sign-in")]
        public async Task<IActionResult> Login(LoginDTO loginDto)
        {
            var result = await _authService.AuthenticateAsync(loginDto);

            if (result is null)
                return Unauthorized("Invalid credentials");

            return Ok(result);
        }


        [HttpPost("sign-up")]
        [HttpPost]
        public async Task<ActionResult<GetUserDTO>> AddAsync(UserDTO userDto)
        {
            var newUser = await _userService.AddAsync(userDto);
            return Created($"api/users/{newUser.Id}", newUser.Id);
        }
    }
}
