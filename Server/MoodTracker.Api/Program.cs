using Microsoft.EntityFrameworkCore;
using MoodTracker.Core.Helpers;
using MoodTracker.Core.Interfaces;
using MoodTracker.Core.Interfaces.Repositories;
using MoodTracker.Core.Interfaces.Services;
using MoodTracker.Core.Models;
using MoodTracker.Core.Models._Dtos;
using MoodTracker.Core.Services;
using MoodTracker.Data;
using MoodTracker.Data.Repositories;

namespace MoodTracker.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("MoodTrackerConnectionString");
            builder.Services.AddDbContext<MoodTrackerDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                    .EnableSensitiveDataLogging());

            ConfigureTypeMappings(builder);

            // Add services to the container.
            builder.Services.AddCors(o => o.AddDefaultPolicy(builder =>
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader()));

            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<MoodTrackerDbContext>();
                db.Database.Migrate();
            }

            await CreateAdminIfNotExistAsync();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
            //
            // Helper methods
            //
            void ConfigureTypeMappings(WebApplicationBuilder builder)
            {
                builder.Services.AddSingleton<IPasswordHasher<User>, AspNetCoreIdentityPasswordHasher<User>>();

                builder.Services.AddScoped<IUserService, UserService>();
                builder.Services.AddScoped<IUserRepository, UserRepository>();

                builder.Services.AddScoped<IMoodService, MoodService>();
                builder.Services.AddScoped<IMoodRepository, MoodRepository>();
            }

            async Task CreateAdminIfNotExistAsync()
            {
                using var scope = app.Services.CreateScope();
                var userService = scope.ServiceProvider.GetRequiredService<IUserService>();

                if (!await userService.UserWithRoleExists("Admin"))
                {
                    var adminAccount = new UserDto
                    {
                        Id = Guid.NewGuid(),
                        Username = "admin",
                        Role = "Admin"
                    };

                    await userService.CreateUserAsync(adminAccount, "admin$");
                }
            }
        }
    }
}