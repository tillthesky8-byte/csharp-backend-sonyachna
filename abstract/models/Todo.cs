public class Todo
{
    public int Id { get; set; }
    public string? Description { get; set; }
    public TodoStatus Status { get; set; }
    public TodoScope Scope { get; set; }
    public DateTime CreatedAt { get; set; } // Unix timestamp in seconds
    public DateTime? UpdatedAt { get; set; } // Unix timestamp in seconds
    public DateTime? CompletedAt { get; set; } // Unix timestamp in seconds, nullable for not completed tasks
    public DateTime? DueAt { get; set; } // Unix timestamp in seconds, for task deadlines
}