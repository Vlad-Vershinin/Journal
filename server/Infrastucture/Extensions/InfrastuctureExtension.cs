using Application.Abstractions;
using Infrastucture.Configuration;
using Infrastucture.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Infrastucture.Extensions;

public static class InfrastuctureExtension
{
    public static IServiceCollection AddInfrastucture(
        this IServiceCollection services,
        IConfiguration configuration
        )
    {
        services.AddScoped<IJwtService, JwtService>();

        services.AddTransient<IPasswordService, PasswordService>();

        services.AddOptions<JwtSettings>()
            .Bind(configuration.GetSection("Jwt"))
            .ValidateOnStart();

        return services;
    }
}
