using Domain.Models;

namespace Application.Abstractions;

public interface IGradeRepository : IBaseRepository<Grade>
{
    Task<List<Grade>> GetByStudentIdAsync(int studentId);
    Task<List<Grade>> GetBySubjectIdAsync(int subjectId);
    Task<Grade?> GetByStudentAndSubjectAsync(int studentId, int subjectId);
}
