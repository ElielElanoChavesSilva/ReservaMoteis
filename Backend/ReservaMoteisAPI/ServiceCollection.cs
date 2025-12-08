using BookMotelsApplication.Interfaces;
using BookMotelsApplication.Services;
using BookMotelsDomain.Interfaces;
using BookMotelsInfra.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using IAuthenticationService = BookMotelsApplication.Interfaces.IAuthenticationService;

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


        public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration config)
        {
            string secret = config["JwtConfiguration:Key"];

            if (string.IsNullOrWhiteSpace(secret))
                throw new Exception("JWT Secret is missing in configuration.");

            var key = Encoding.ASCII.GetBytes(secret);


            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ClockSkew = TimeSpan.Zero
                    };
                });
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
             {
                 c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                 {
                     Title = "Reserva Motéis API",
                     Version = "v1"
                 });

                 var securitySchema = new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                 {
                     Description = "Insira o token JWT usando: Bearer {seu token}",
                     Name = "Authorization",
                     In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                     Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                     Scheme = "bearer",
                     BearerFormat = "JWT"
                 };

                 c.AddSecurityDefinition("Bearer", securitySchema);

                 c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                 {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                 });
             });
        }
    }
}
