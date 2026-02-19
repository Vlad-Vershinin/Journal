using Application.Services;
using Domain.Repositories;
using Domain.Services;
using DotNetEnv;
using Infrastucture;
using Infrastucture.Repositories;
using Infrastucture.Services;
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


        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IGroupRepository, GroupRepository>();
        builder.Services.AddScoped<ISubjectRepository, SubjectRepository>();
        builder.Services.AddScoped<IGradeRepository, GradeRepository>();
        builder.Services.AddScoped<IAttendanceRepository, AttendanceRepository>();
        builder.Services.AddScoped<IAssignmentRepository, AssignmentRepository>();
        builder.Services.AddScoped<ISubmissionRepository, SubmissionRepository>();

        builder.Services.AddScoped<IUserService, UserService>();

        builder.Services.AddInfrastucture(builder.Configuration);
        builder.Services.ConfigureJwt(builder.Configuration);

        builder.Services.AddTransient<IPasswordService, PasswordService>();


        var app = builder.Build();

        app.UseSerilogRequestLogging();
        app.UseCors("AllowAll");
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

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
