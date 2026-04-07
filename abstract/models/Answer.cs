public class Answer
{
    public int Id { get; set; }
    public int QuestionId { get; set; }
    public int SurveySessionId { get; set; }
    public string? Response { get; set; }
    public string? Remark { get; set; }

    //navigation properties
    public Question? Question { get; set; }
    public SurveySession? SurveySession { get; set; }
}