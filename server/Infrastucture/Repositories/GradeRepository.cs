using Domain.Models;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Infrastucture.Repositories;

public class GradeRepository : BaseRepository<Grade>, IGradeRepository
{
    public GradeRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<List<Grade>> GetByStudentIdAsync(int studentId)
    {
        return await _dbSet.Where(g => g.StudentId == studentId).ToListAsync();
    }

    public async Task<List<Grade>> GetBySubjectIdAsync(int subjectId)
    {
        return await _dbSet.Where(g => g.SubjectId == subjectId).ToListAsync();
    }

    public async Task<Grade?> GetByStudentAndSubjectAsync(int studentId, int subjectId)
    {
        return await _dbSet.FirstOrDefaultAsync(g => g.StudentId == studentId && g.SubjectId == subjectId);
    }
}
