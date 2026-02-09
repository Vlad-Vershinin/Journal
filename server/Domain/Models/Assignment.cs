namespace Domain.Models;

public class Assignment
{
    public int Id { get; set; }
    public int SubjectId { get; set; }
    public string Title { get; set; } = null!;
    public AssignmentType Type { get; set; }
    public DateOnly Deadline { get; set; }
}
