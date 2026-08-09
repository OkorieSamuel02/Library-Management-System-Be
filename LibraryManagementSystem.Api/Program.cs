using LibraryManagementSystem.Infrastructure;
using LibraryManagementSystem.Application;
using LibraryManagementSystem.Domain.Entity;
using LibraryManagementSystem.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using LibraryManagementSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ApplicationDependencies();
builder.Services.InfrastructureDependencies(builder.Configuration);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<ApplicationDbContext>();
    await context.Database.MigrateAsync();

    var userManager = services.GetRequiredService<UserManager<User>>();

    await UserSeeder.SeedAdminAsync(userManager);
}

// Configure the HTTP request pipeline.


app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
