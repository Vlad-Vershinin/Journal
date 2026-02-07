namespace Domain.Models;

public class Grade
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public int SubjectId { get; set; }
    public int Value { get; set; } // от 1 до 5
}
