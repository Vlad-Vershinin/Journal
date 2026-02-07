using Domain.Models;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Infrastucture.Repositories;

public class AttendanceRepository : BaseRepository<Attendance>, IAttendanceRepository
{
    public AttendanceRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<List<Attendance>> GetByStudentIdAsync(int studentId)
    {
        return await _dbSet.Where(a => a.StudentId == studentId).ToListAsync();
    }

    public async Task<List<Attendance>> GetBySubjectIdAsync(int subjectId)
    {
        return await _dbSet.Where(a => a.SubjectId == subjectId).ToListAsync();
    }

    public async Task<List<Attendance>> GetByGroupIdAsync(int groupId)
    {
        return await _dbSet.Where(a => a.GroupId == groupId).ToListAsync();
    }
}
