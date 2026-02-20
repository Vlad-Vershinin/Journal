using Domain.Models;

namespace Application.Abstractions;

public interface ISubjectRepository : IBaseRepository<Subject>
{
    Task<List<Subject>> GetByGroupIdAsync(int groupId);
    Task<List<Subject>> GetByTeacherIdAsync(int teacherId);
}
