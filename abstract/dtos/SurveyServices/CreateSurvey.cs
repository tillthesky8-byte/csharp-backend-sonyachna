namespace DTOs
{
    public class CreateSurveyRequest
    {
        public List<Answer> Answers { get; set; } = new List<Answer>();
    }

    public class CreateSurveyResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
    }
}