using BookMotelsAPI;
using BookMotelsAPI.Configuration;
using BookMotelsApplication.Interfaces;
using BookMotelsDomain.Entities;
using BookMotelsInfra.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MainContext>(options =>
    options.UseSqlite("Data Source=main.db"));

// Add services to the container.
builder.Services.Configure<JwtConfiguration>(
    builder.Configuration.GetSection("JwtConfiguration"));

builder.Services.AddSingleton<IJwtConfiguration>(sp =>
    sp.GetRequiredService<IOptions<JwtConfiguration>>().Value);


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

    if (!db.Profiles.Any())
    {
        db.Profiles.AddRange(
            new ProfileEntity { Id = 1, Name = "Admin" },
            new ProfileEntity { Id = 2, Name = "User" }
        );
        db.SaveChanges();
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
