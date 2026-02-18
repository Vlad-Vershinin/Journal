using Domain.Models;

namespace Application.Abstractions;

public interface IJwtService
{
    string GenerateJwt(User user);
}
