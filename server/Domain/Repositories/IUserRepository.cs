using Domain.Models;

namespace Domain.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> GetByLoginAsync(string login);
}
