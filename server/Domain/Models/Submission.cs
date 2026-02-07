namespace Domain.Models;

public class Submission
{
    public int Id { get; set; }
    public int AssignmentId { get; set; }
    public int StudentId { get; set; }
    public string FilePath { get; set; }
    public DateTime SubmittedAt { get; set; }
    public int? Grade { get; set; }
    public string? TeacherFeedback { get; set; }
}
