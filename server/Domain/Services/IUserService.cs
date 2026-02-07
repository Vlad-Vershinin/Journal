using Domain.Models;

namespace Domain.Services;

public interface IUserService
{
    Task CreateUser(User user);
}
