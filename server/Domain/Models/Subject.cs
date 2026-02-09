namespace Domain.Models;

public class Subject
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int TeacherId { get; set; }
    public int GroupId { get; set; }
}
