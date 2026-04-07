public class DreamEntry
{
    public int Id { get; set; }
    public int SurveySessionId { get; set; } // foreign key to associate with a specific survey session
    public string? Content { get; set; } // The actual dream content entered by the user
    public bool IsLucid { get; set; } // Indicates whether the dream was lucid or not
    
    //navigation properties
    public SurveySession? SurveySession { get; set; }
}