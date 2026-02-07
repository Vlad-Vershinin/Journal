using Domain.Models;

namespace Domain.Repositories;

public interface ISubmissionRepository : IBaseRepository<Submission>
{
    Task<List<Submission>> GetByAssignmentIdAsync(int assignmentId);
    Task<List<Submission>> GetByStudentIdAsync(int studentId);
    Task<Submission?> GetByAssignmentAndStudentAsync(int assignmentId, int studentId);
}
