namespace BookMotelsApplication.DTOs.User
{
    public record UpdateUserDTO(
        string Name,
        string Email,
        int ProfileId
    );
}
