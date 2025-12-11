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

        /// <summary>
        /// Autentica um usuário e retorna um token JWT.
        /// </summary>
        /// <param name="loginDto">As credenciais de login (email e senha).</param>
        /// <returns>Um ActionResult contendo o token JWT se a autenticação for bem-sucedida, ou Não Autorizado se as credenciais forem inválidas.</returns>
        /// <response code="200">Retorna o token JWT após autenticação bem-sucedida.</response>
        /// <response code="401">Se as credenciais forem inválidas.</response>
        [HttpPost("sign-in")]
        public async Task<IActionResult> Login(LoginDTO loginDto)
        {
            var result = await _authService.AuthenticateAsync(loginDto);

            if (result is null)
                return Unauthorized("Credenciais inválidas");

            return Ok(result);
        }

        /// <summary>
        /// Registra um novo usuário.
        /// </summary>
        /// <param name="userDto">Os detalhes de registro do usuário.</param>
        /// <returns>Um ActionResult contendo os detalhes do usuário recém-criado.</returns>
        /// <response code="201">Retorna o ID do usuário recém-criado.</response>
        /// <response code="400">Se os detalhes de registro do usuário forem inválidos.</response>
        /// <response code="409">Se já existir um usuário com o email fornecido.</response>
        [HttpPost("sign-up")]
        public async Task<ActionResult<GetUserDTO>> AddAsync(UserDTO userDto)
        {
            var newUser = await _userService.AddAsync(userDto);
            return Created($"api/users/{newUser.Id}", newUser.Id);
        }

        /// <summary>
        /// Retorna o perfil do usuário logado.
        /// </summary>
        /// <returns>O UserDTO do usuário logado.</returns>
        /// <response code="200">Retorna o UserDTO do usuário logado.</response>
        /// <response code="404">Se o usuário não for encontrado.</response>
        [HttpGet("me")]
        public async Task<ActionResult<GetUserDTO>> MeAsync()
        {
            return Ok(await _userService.FindByIdAsync(LoggedUserId));
        }
    }
}
