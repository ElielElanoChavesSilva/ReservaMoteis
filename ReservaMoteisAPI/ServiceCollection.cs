namespace ReservaMoteisAPI
{
    public static class ServiceCollection
    {
        private const string _connectionString = "MsqlConnection";
        public static IServiceCollection ConfigureDatabase(this IServiceCollection collection, IConfiguration configuration)
        {
            collection.AddDbContext<MainContext>(option =>
            {
                string conn = configuration.GetConnectionString(_connectionString) ??
                              throw new InvalidOperationException($"Connection string '{_connectionString}' not found."); ;

                option.UseMySql(conn,
                    ServerVersion.AutoDetect(conn));
            });

            return collection;
        }
    }
}
