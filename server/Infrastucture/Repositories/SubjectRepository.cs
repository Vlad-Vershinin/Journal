using Domain.Models;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Infrastucture.Repositories;

public class SubjectRepository : BaseRepository<Subject>, ISubjectRepository
{
    public SubjectRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<List<Subject>> GetByGroupIdAsync(int groupId)
    {
        return await _dbSet.Where(s => s.GroupId == groupId).ToListAsync();
    }

    public async Task<List<Subject>> GetByTeacherIdAsync(int teacherId)
    {
        return await _dbSet.Where(s => s.TeacherId == teacherId).ToListAsync();
    }
}
