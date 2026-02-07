using Domain.Models;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Infrastucture.Repositories;

public class GroupRepository : BaseRepository<Group>, IGroupRepository
{
    public GroupRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Group?> GetWithUsersAsync(int id)
    {
        return await _dbSet.Include(g => g.Users).FirstOrDefaultAsync(g => g.Id == id);
    }
}
