namespace BookMotelsApplication.DTOs.User
{
    public class CreateUserDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int ProfileId { get; set; }
        public string Password { get; set; } = string.Empty;
    }
}
