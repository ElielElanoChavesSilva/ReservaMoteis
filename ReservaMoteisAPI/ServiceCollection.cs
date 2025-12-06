namespace ReservaMoteisAPI
{
    public static class ServiceCollection
    {
        private const string _connectionString = "MsqlConnection";

        public static IServiceCollection ConfigureRepositories(this IServiceCollection collection)
        {

            return collection;
        }
    }
}
