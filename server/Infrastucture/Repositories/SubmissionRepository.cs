using Domain.Models;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Infrastucture.Repositories;

public class SubmissionRepository : BaseRepository<Submission>, ISubmissionRepository
{
    public SubmissionRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<List<Submission>> GetByAssignmentIdAsync(int assignmentId)
    {
        return await _dbSet.Where(s => s.AssignmentId == assignmentId).ToListAsync();
    }

    public async Task<List<Submission>> GetByStudentIdAsync(int studentId)
    {
        return await _dbSet.Where(s => s.StudentId == studentId).ToListAsync();
    }

    public async Task<Submission?> GetByAssignmentAndStudentAsync(int assignmentId, int studentId)
    {
        return await _dbSet.FirstOrDefaultAsync(s => s.AssignmentId == assignmentId && s.StudentId == studentId);
    }
}
