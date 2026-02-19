using Application.Abstractions;
using Domain.Models;
using Infrastucture.Configuration;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Infrastucture.Services;

public class JwtService : IJwtService
{
    private readonly IOptions<JwtSettings> _options;

    public JwtService(IOptions<JwtSettings> options)
    {
        _options = options;
    }

    public string GenerateJwt(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim("fullName", user.FullName),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var jwtToken = new JwtSecurityToken(
            expires: DateTime.UtcNow.Add(_options.Value.Expires),
            claims: claims,
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.SecretKey)),
                SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(jwtToken);
    }
}
