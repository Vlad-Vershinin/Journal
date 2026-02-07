using DotNetEnv;
using Domain.Repositories;
using Infrastucture.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Domain.Services;
using Application.Services;

namespace API;

public class Program
{
    public static void Main(string[] args)
    {
        Env.Load();

        var connectionString = Env.GetString("DB_CONNECTION_STRING")
            ?? throw new InvalidOperationException("No connection str");

        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
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

        var app = builder.Build();

        app.UseCors("AllowAll");
        app.MapControllers();
        app.UseRouting();

        app.Run();
    }
}
