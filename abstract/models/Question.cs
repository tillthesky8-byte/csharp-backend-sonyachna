public class Question
{
    public int Id { get; set; }
    public string? Code { get; set; } // Unique code for the question, e.g., "Q1", "Q2", etc.
    public string? Text { get; set; } // The actual question text to display to the user

    //navigation properties
    public List<Answer>? Answers { get; set; }
}