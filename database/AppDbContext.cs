using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<SurveySession> SurveySessions { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Answer> Answers { get; set; }
    public DbSet<DreamEntry> DreamEntries { get; set; }
    public DbSet<Todo> Todos { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
}