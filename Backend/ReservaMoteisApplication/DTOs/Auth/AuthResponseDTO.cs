namespace BookMotelsApplication.DTOs.Auth
{
    public record AuthResponseDTO(
        string Token,
        Guid UserId,
        string Name,
        string Email,
        string Role
    );
}
