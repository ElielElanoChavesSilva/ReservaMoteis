using BookMotelsDomain.Interfaces;
using BookMotelsInfra.Repositories;

namespace BookMotelsAPI
{
    public static class ServiceCollection
    {
        private const string _connectionString = "MsqlConnection";

        public static void ConfigureRepositories(this IServiceCollection collection)
        {
            collection.AddScoped<IUserRepository, UserRepository>();
            collection.AddScoped<ISuiteRepository, SuiteRepository>();
            collection.AddScoped<IReserveRepository, ReserveRepository>();
            collection.AddScoped<IProfileRepository, ProfileRepository>();
            collection.AddScoped<IMotelRepository, MotelRepository>();
            collection.AddScoped<IUserRepository, UserRepository>();
        }
    }
}
