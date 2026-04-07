namespace DTOs
{
    public class CheckSurveyExistenceRequest
    {
        public DateOnly Date { get; set; }
    }

    public class CheckSurveyExistenceResponse
    {
        public bool Exists { get; set; }
    }

}