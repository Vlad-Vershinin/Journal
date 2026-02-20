using Domain.Models;

namespace Application.Abstractions;

public interface IAssignmentRepository : IBaseRepository<Assignment>
{
    Task<List<Assignment>> GetBySubjectIdAsync(int subjectId);
}
