using BookMotelsAPI.Configuration;
using BookMotelsApplication.Interfaces;
using BookMotelsApplication.Services;
using BookMotelsDomain.Interfaces;
using BookMotelsInfra.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BookMotelsAPI
{
    public static class ServiceCollection
    {

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
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IReserveService, ReserveService>();
        }


        public static void ConfigureAuthentication(this IServiceCollection services)
        {
            var jwtConfiguration = services.BuildServiceProvider().GetRequiredService<IJwtConfiguration>();
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    var key = Encoding.ASCII.GetBytes(jwtConfiguration.Key);
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ClockSkew = TimeSpan.Zero
                    };
                });
        }

    }
}
