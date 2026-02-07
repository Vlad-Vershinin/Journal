using Domain.DTOs;
using Domain.Models;

namespace Domain.Services;

public interface IUserService
{
    Task CreateUser(User user);
    Task<bool> Login(LoginDto dto);
}
