using BookMotelsAPI;
using BookMotelsDomain.Entities;
using BookMotelsInfra.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MainContext>(options =>
    options.UseSqlite("Data Source=main.db"));

// Add services to the container.
builder.Services.ConfigureRepositories();
builder.Services.ConfigureServices();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
app.UseAuthorization();
app.MapControllers();
app.Run();
