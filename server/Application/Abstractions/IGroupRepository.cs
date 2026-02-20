using Domain.Models;

namespace Application.Abstractions;

public interface IGroupRepository : IBaseRepository<Group>
{
    Task<Group?> GetWithUsersAsync(int id);
}
