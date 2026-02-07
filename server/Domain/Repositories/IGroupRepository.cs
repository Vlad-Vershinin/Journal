using Domain.Models;

namespace Domain.Repositories;

public interface IGroupRepository : IBaseRepository<Group>
{
    Task<Group?> GetWithUsersAsync(int id);
}
