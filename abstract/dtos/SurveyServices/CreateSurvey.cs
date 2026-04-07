namespace DTOs
{
    public class CreateSurveyRequest
    {
        public List<Answer> Answers { get; set; } = new List<Answer>();
        public DateTime Timestamp { get; set; } // Unix timestamp in seconds
    }

    public class CreateSurveyResponse
    {
        public int SurveySessionId { get; set; }
        public DateOnly Date { get; set; }
    }
}