using BookMotelsAPI;
using BookMotelsAPI.Configuration;
using BookMotelsAPI.Middleware;
using BookMotelsApplication.Interfaces;
using BookMotelsInfra.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MainContext>(options =>
    options.UseSqlite("Data Source=main.db"));

builder.Services.Configure<JwtConfiguration>(
    builder.Configuration.GetSection("JwtConfiguration"));

builder.Services.AddSingleton<IJwtConfiguration>(sp =>
    sp.GetRequiredService<IOptions<JwtConfiguration>>().Value);
builder.Services.ConfigureRedisCache(builder.Configuration);

builder.Services.ConfigureCors();

builder.Services.ConfigureRepositories();
builder.Services.ConfigureServices();

builder.Services.ConfigureAuthentication(builder.Configuration);
builder.Services.AddAuthorization();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<MainContext>();
    db.Database.Migrate();
    DbInitializer.Seed(db);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
app.UseCors("FrontendAllow");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
