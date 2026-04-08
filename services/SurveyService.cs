public class SurveyService
{
    private readonly IRepository<SurveySession> repo;
    private readonly AppDbContext db;
    private readonly ILogger<SurveyService> logger;

    public SurveyService(IRepository<SurveySession> repo, AppDbContext db, ILogger<SurveyService> logger)
    {
        this.repo = repo;
        this.db = db;
        this.logger = logger;
    }

    public InternalResponse<bool> SurveyExistsForDate(DateOnly date)
    {
        try
        {
            var repoResponse = repo.GetAll();
            if (!repoResponse.Success)
            {
                return new InternalResponse<bool> { Success = false };
            }

            var sessions = repoResponse.Data;
            var exists = sessions!.Any(s => s.Date == date);
            logger.LogInformation($"Survey session existence check for date {date}: {exists}");
            return new InternalResponse<bool> { Success = true, Data = exists };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "ERROR AT SERVICE: Error checking survey session existence for date");
            return new InternalResponse<bool> { Success = false };
        }
    }

    public InternalResponse SubmitSurvey(List<Answer> answers)
    {
        try
        {
            //block where an error occurs.
            var repoResponse = repo.Add(new SurveySession
            {
                Date = DateOnly.FromDateTime(DateTime.UtcNow),
                CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            });
            if (!repoResponse.Success)
            {
                return new InternalResponse { Success = false };
            }
            var session = repoResponse.Data;
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
            logger.LogError(ex, "ERROR AT SERVICE: An error occurred while submitting the survey");
            return new InternalResponse { Success = false };
        }

        return new InternalResponse { Success = true };

    }
}