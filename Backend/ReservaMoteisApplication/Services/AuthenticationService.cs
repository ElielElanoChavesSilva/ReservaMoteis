using BookMotelsApplication.DTOs.Auth;
using BookMotelsApplication.Interfaces;
using BookMotelsDomain.Entities;
using BookMotelsDomain.Exceptions;
using BookMotelsDomain.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookMotelsApplication.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtConfiguration _jwtConfiguration;

        public AuthenticationService(
            IUserRepository userRepository,
            IJwtConfiguration jwtConfiguration)

        {
            _jwtConfiguration = jwtConfiguration;
            _userRepository = userRepository;
        }

        public async Task<AuthResponseDTO?> AuthenticateAsync(LoginDTO login)
        {
            var user = await _userRepository.GetByEmailAsync(login.Email) ??
                        throw new NotFoundException("Usuário não encontrado");

            bool validPassword = BCrypt.Net.BCrypt.Verify(login.Password, user.Password);
            if (!validPassword) return null;

            return GenerateToken(user);
        }

        private AuthResponseDTO GenerateToken(UserEntity user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtConfiguration.Key);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Profile.Name)
                }),
                Expires = DateTime.UtcNow.AddMinutes(_jwtConfiguration.TokenExpirationInMinutes),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new AuthResponseDTO
            {
                Token = tokenHandler.WriteToken(token),
                UserId = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Profile.Name
            };
        }
    }
}
