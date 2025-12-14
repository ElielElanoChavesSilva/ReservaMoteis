namespace BookMotelsApplication.DTOs.User;

public record GetUserDTO(
    Guid Id,
    string Name,
    string Email,
    int ProfileId
);
