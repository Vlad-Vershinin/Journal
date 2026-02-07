using Domain.Models;

namespace Domain.Repositories;

public interface IAttendanceRepository : IBaseRepository<Attendance>
{
    Task<List<Attendance>> GetByStudentIdAsync(int studentId);
    Task<List<Attendance>> GetBySubjectIdAsync(int subjectId);
    Task<List<Attendance>> GetByGroupIdAsync(int groupId);
}
