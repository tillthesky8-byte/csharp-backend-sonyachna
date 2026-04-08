public class Todo
{
    public int Id { get; set; }
    public string? Description { get; set; }
    public TodoStatus Status { get; set; }
    public TodoScope Scope { get; set; }
    public long CreatedAt { get; set; } // Unix timestamp in seconds
    public long? UpdatedAt { get; set; } // Unix timestamp in seconds
    public long? CompletedAt { get; set; } // Unix timestamp in seconds, nullable for not completed tasks
    public long? DueAt { get; set; } // Unix timestamp in seconds, for task deadlines
}