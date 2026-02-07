using Domain.Models;

namespace Domain.Repositories;

public interface IAssignmentRepository : IBaseRepository<Assignment>
{
    Task<List<Assignment>> GetBySubjectIdAsync(int subjectId);
}
