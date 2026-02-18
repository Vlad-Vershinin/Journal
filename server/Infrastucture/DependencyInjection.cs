using Application.Abstractions;
using Infrastucture.Configuration;
using Infrastucture.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastucture;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastucture(
        this IServiceCollection services,
        IConfiguration configuration
        )
    {
        services.AddScoped<IJwtService, JwtService>();

        services.AddOptions<JwtSettings>()
            .Bind(configuration.GetSection("Jwt"))
            .ValidateOnStart();

        return services;
    }
}
