namespace BookMotelsAPI.Configuration
{
    public interface IJwtConfiguration
    {
        public string Key { get; set; }
        public int TokenExpirationInMinutes { get; set; }
    }
}
