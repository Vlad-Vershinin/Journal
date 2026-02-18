using Domain.DTOs;
using Domain.Models;

namespace Domain.Services;

public interface IUserService
{
    Task<Result<User>> CreateUser(User user);
    Task<Result<string>> Login(LoginDto dto);
}
