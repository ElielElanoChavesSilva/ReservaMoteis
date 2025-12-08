using BookMotelsApplication.Interfaces;

namespace BookMotelsAPI.Configuration
{
    public class JwtConfiguration : IJwtConfiguration
    {
        public string Key { get; set; } = string.Empty;
        public int TokenExpirationInMinutes { get; set; }
    }
}
