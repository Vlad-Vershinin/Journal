using Domain.Models;

namespace Domain.Repositories;

public interface ISubjectRepository : IBaseRepository<Subject>
{
    Task<List<Subject>> GetByGroupIdAsync(int groupId);
    Task<List<Subject>> GetByTeacherIdAsync(int teacherId);
}
