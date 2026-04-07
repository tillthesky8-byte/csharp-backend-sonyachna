public class DreamEntry
{
    public int Id { get; set; }
    public DateOnly Date { get; set; } // The date of the dream entry, used for searching and organizing entries by date
    public string? Content { get; set; } // The actual dream content entered by the user
    public bool IsLucid { get; set; } // Indicates whether the dream was lucid or not

}