namespace BookMotelsApplication.DTOs.User
{
    public record UserDTO(
        string Name,
        string Email,
        string Password,
        int ProfileId);
}
