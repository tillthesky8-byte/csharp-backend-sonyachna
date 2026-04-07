public class Todo
{
    public int Id { get; set; }
    public string? Description { get; set; }
    public TodoStatus Status { get; set; }
    public TodoScope Scope { get; set; }
    public int CreatedAt { get; set; } // Unix timestamp in seconds
    public int UpdatedAt { get; set; } // Unix timestamp in seconds
    public int? CompletedAt { get; set; } // Unix timestamp in seconds, nullable for not completed tasks
    public int dueAt { get; set; } // Unix timestamp in seconds, for task deadlines
}