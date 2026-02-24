using Application.Abstractions;
using Application.Services;
using Domain.Models;
using DotNetEnv;
using Infrastucture.Extensions;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Serilog;

namespace API;

public class Program
{
    public static void Main(string[] args)
    {
        Env.Load();

        var connectionString = Env.GetString("DB_CONNECTION_STRING")
            ?? throw new InvalidOperationException("No connection str");

        var builder = WebApplication.CreateBuilder(args);


        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        builder.Host.UseSerilog((context, services, configuration) => configuration
            .ReadFrom.Configuration(context.Configuration));


        builder.Services.AddControllers();
        builder.Services.AddAuthorization();

        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));

        builder.Services.AddScoped<UserService>();

        builder.Services.AddInfrastucture(builder.Configuration);
        builder.Services.AddRepositories(builder.Configuration);
        builder.Services.ConfigureJwt(builder.Configuration);


        var app = builder.Build();

        app.UseSerilogRequestLogging();
        app.UseCors("AllowAll");
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.Migrate();

            if (!db.Users.Any(u => u.Role == UserRole.Admin))
            {
                var admin_fullane = Env.GetString("USER__DEFAULT_ADMIN_FULLNAME")
                    ?? throw new InvalidOperationException("No admin password");
                var admin_login = Env.GetString("USER__DEFAULT_ADMIN_LOGIN")
                    ?? throw new InvalidOperationException("No admin login");
                var admin_password = Env.GetString("USER__DEFAULT_ADMIN_PASSWORD")
                    ?? throw new InvalidOperationException("No admin password");
                var password_service = scope.ServiceProvider.GetRequiredService<IPasswordService>();

                db.Users.Add(new User
                {
                    FullName = admin_login,
                    Login = admin_login,
                    PasswordHash = password_service.HashPassword(admin_password),
                    Role = UserRole.Admin
                });

                db.SaveChanges();
            }
        }

        app.Lifetime.ApplicationStarted.Register(() =>
        {
            using var scope = app.Services.CreateScope();
            var dataSource = scope.ServiceProvider.GetRequiredService<EndpointDataSource>();
            var endpoints = dataSource.Endpoints;

            Log.Information("Сервер запущен. Адреса: {Urls}. Кол-во эндпоинтов: {EndpointCount}",
                string.Join(", ", app.Urls),
                endpoints.Count);
        });

        app.Run();

        Log.Information("Сервер остановлен.");
    }
}
