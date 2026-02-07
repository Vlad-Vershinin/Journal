namespace Domain.Services;

public interface IPasswordService
{
    string HashPassword(string password);
    bool ValidatePassword(string password, string hashPassword);
}
