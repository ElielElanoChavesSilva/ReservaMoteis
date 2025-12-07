namespace BookMotelsApplication.DTOs.User;

public class GetUserDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int ProfileId { get; set; }
}
