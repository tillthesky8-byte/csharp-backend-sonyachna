public class SurveySession
{
    public int Id { get; set; }
    public DateOnly Date { get; set; } // Date for serching specific day, and check existance of session for that day
    public DateTime CreatedAt { get; set; } // Unix timestamp in seconds
    
    //navigation properties
    public List<Answer>? Answers { get; set; }
    public List<DreamEntry>? DreamEntries { get; set; }

}