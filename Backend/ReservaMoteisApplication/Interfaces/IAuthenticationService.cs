using BookMotelsApplication.DTOs.Auth;

namespace BookMotelsApplication.Interfaces
{
    public interface IAuthenticationService
    {
        Task<AuthResponseDTO?> AuthenticateAsync(LoginDTO login);
    }
}
