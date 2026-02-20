using Application.Abstractions;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Infrastucture.Repositories;

public class AssignmentRepository : BaseRepository<Assignment>, IAssignmentRepository
{
    public AssignmentRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<List<Assignment>> GetBySubjectIdAsync(int subjectId)
    {
        return await _dbSet.Where(a => a.SubjectId == subjectId).ToListAsync();
    }
}
