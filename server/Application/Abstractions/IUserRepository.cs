using Domain.Models;

namespace Application.Abstractions;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> GetByLoginAsync(string login);
}
