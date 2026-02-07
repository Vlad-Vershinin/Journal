using Domain.Services;

namespace Infrastucture.Services;

public class PasswordService : IPasswordService
{
    public string HashPassword(string password) 
        => BCrypt.Net.BCrypt.EnhancedHashPassword(password);

    public bool ValidatePassword(string password, string hashPassword)
        => BCrypt.Net.BCrypt.EnhancedVerify(password, hashPassword);
}
