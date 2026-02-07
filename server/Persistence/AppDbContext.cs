using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class AppDbContext : DbContext
{
    public DbSet<Assignment> Assigments { get; set; }
    public DbSet<Attendance> Attendances { get; set; }
    public DbSet<Grade> Grades { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<Submission> Submissions { get; set; }
    public DbSet<User> Users { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
