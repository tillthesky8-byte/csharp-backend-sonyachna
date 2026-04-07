public class SurveyService
{
    private readonly IRepository<SurveySession> surveySessionRepository;
    private readonly AppDbContext db;
    private readonly ILogger<SurveyService> logger;

    public SurveyService(
        IRepository<SurveySession> surveySessionRepository,
        AppDbContext db,
        ILogger<SurveyService> logger)
    {
        this.surveySessionRepository = surveySessionRepository;
        this.db = db;
        this.logger = logger;
    }

    public bool SurveyExistsForDate(DateOnly date)
    {
        try
        {
            var allSessionsResponse = surveySessionRepository.GetAll();
            if (!allSessionsResponse.Success)
            {
                logger.LogError("Failed to retrieve survey sessions: {Message}", allSessionsResponse.Message);
                return false;
            }

            var sessions = allSessionsResponse.Data;
            var exists = sessions!.Any(s => s.Date == date);
            logger.LogInformation($"Survey session existence check for date {date}: {exists}");
            return exists;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error checking survey session existence for date");
            return false;
        }
    }

    public bool SubmitSurvey(List<Answer> answers)
    {
        try
        {
            var createSessionResponse = surveySessionRepository.Add(new SurveySession { Date = DateOnly.FromDateTime(DateTime.UtcNow) });
            if (!createSessionResponse.Success)
            {
                logger.LogError("Failed to create survey session: {Message}", createSessionResponse.Message);
                return false;
            }
            var session = createSessionResponse.Data;
            foreach (var answer in answers)
            {
                answer.SurveySessionId = session!.Id;
                db.Answers.Add(answer);
            }
            db.SaveChanges();
            logger.LogInformation($"Survey session with id = {session!.Id} was created and {answers.Count} answers were added to the database");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while submitting the survey");
            return false;
        }

        return true;

    }
}