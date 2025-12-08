using BookMotelsApplication.DTOs.Auth;
using BookMotelsApplication.DTOs.User;
using BookMotelsApplication.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookMotelsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ApiController
    {
        private readonly IAuthenticationService _authService;
        private readonly IUserService _userService;

        public AuthController(
            IUserService userService,
        IAuthenticationService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        [HttpPost("sign-in")]
        public async Task<IActionResult> Login(LoginDTO loginDto)
        {
            var result = await _authService.AuthenticateAsync(loginDto);

            if (result is null)
                return Unauthorized("Credenciais inválidas");

            return Ok(result);
        }

        [HttpPost("sign-up")]
        public async Task<ActionResult<GetUserDTO>> AddAsync(UserDTO userDto)
        {
            var newUser = await _userService.AddAsync(userDto);
            return Created($"api/users/{newUser.Id}", newUser.Id);
        }
    }
}
