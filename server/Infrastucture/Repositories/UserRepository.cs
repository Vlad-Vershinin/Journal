using Domain.Models;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Infrastucture.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<User?> GetByLoginAsync(string login)
    {
        return await _dbSet.FirstOrDefaultAsync(u => u.Login == login);
    }
}
