public class SurveyService
{
    private readonly IRepository<SurveySession> repo;
    private readonly AppDbContext db;
    private readonly ILogger<SurveyService> logger;

    public SurveyService(
        IRepository<SurveySession> repo,
        AppDbContext db,
        ILogger<SurveyService> logger)
    {
        this.repo = repo;
        this.db = db;
        this.logger = logger;
    }

    public bool SurveyExistsForDate(DateOnly date)
    {
        try
        {
            var allSessionsResponse = repo.GetAll();
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
            //block where an error occurs.
            var createSessionResponse = repo.Add(new SurveySession { Date = DateOnly.FromDateTime(DateTime.UtcNow) });
            if (!createSessionResponse.Success)
            {
                logger.LogError("Failed to create survey session: {Message}", createSessionResponse.Message);
                return false;
            }
            var session = createSessionResponse.Data;
            foreach (var answer in answers)
            {
                // probably passed in the request answer model doesn't match the actual model
                // maybe i try to submit an answer without questionid which corresponds to the real questionid in the database, which causes a foreign key error. need to check the model and the request body.
                var answerEntity = new Answer
                {
                    QuestionId = answer.QuestionId,
                    SurveySessionId = session!.Id,
                    Response = answer.Response,
                    Remark = answer.Remark
                };
                db.Answers.Add(answerEntity);
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