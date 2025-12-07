using BookMotelsApplication.Interfaces;
using BookMotelsApplication.Services;
using BookMotelsDomain.Interfaces;
using BookMotelsInfra.Repositories;

namespace BookMotelsAPI
{
    public static class ServiceCollection
    {
        private const string _connectionString = "MsqlConnection";

        public static void ConfigureRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ISuiteRepository, SuiteRepository>();
            services.AddScoped<IReserveRepository, ReserveRepository>();
            services.AddScoped<IProfileRepository, ProfileRepository>();
            services.AddScoped<IMotelRepository, MotelRepository>();
        }

        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IMotelService, MotelService>();
            services.AddScoped<ISuiteService, SuiteService>();
            services.AddScoped<IReserveService, ReserveService>();
        }
    }
}
