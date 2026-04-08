public class SurveySessionsRepository : IRepository<SurveySession>
{
    private readonly AppDbContext db;
    private readonly ILogger<SurveySessionsRepository> logger;

    public SurveySessionsRepository(AppDbContext db, ILogger<SurveySessionsRepository> logger)
    {
        this.db = db;
        this.logger = logger;
    }

    public InternalResponse<List<SurveySession>> GetAll()
    {
        try
        {
            var sessions = db.SurveySessions.ToList();
            logger.LogInformation($"All survey sessions were retrived from the database; count: {sessions.Count}");
            return new InternalResponse<List<SurveySession>> { Success = true, Data = sessions };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "ERROR AT REPOSITORY: Error fetching all survey sessions");
            return new InternalResponse<List<SurveySession>> { Success = false, Message = "Error fetching all survey sessions", Data = null };
        }
    }

    public InternalResponse<SurveySession> GetById(int id)
    {
        try
         {
             var session = db.SurveySessions.FirstOrDefault(s => s.Id == id);
             logger.LogInformation($"Survey session with id = {id} was retrived from the database");
             return new InternalResponse<SurveySession> { Success = true, Data = session };
         }
         catch (Exception ex)

         {
             logger.LogError(ex, $"ERROR AT REPOSITORY: Error fetching survey session with id {id}");
             return new InternalResponse<SurveySession> { Success = false, Message = $"Error fetching survey session with id {id}", Data = null };
         }
    } 

    public InternalResponse<SurveySession> Add(SurveySession entity)
    {
        try
        {
            db.SurveySessions.Add(entity);
            db.SaveChanges();
            logger.LogInformation($"Survey session with id = {entity.Id} was added to the database");
            return new InternalResponse<SurveySession> { Success = true, Data = entity };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "ERROR AT REPOSITORY: Error adding new survey session");
            return new InternalResponse<SurveySession> { Success = false, Message = "Error adding new survey session", Data = null };
        }
    }

    public InternalResponse<SurveySession> Update(SurveySession entity)
    {
        try
        {
            var existingSession = db.SurveySessions.FirstOrDefault(s => s.Id == entity.Id);
            if (existingSession == null)
            {
                logger.LogWarning($"Survey session with id = {entity.Id} not found for update");
                return new InternalResponse<SurveySession> { Success = false, Message = "Survey session not found", Data = null };
            }
            existingSession.Date = entity.Date;
            existingSession.CreatedAt = entity.CreatedAt;
            db.SaveChanges();
            logger.LogInformation($"Survey session with id = {entity.Id} was updated in the database ");
            return new InternalResponse<SurveySession> { Success = true, Data = existingSession };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"ERROR AT REPOSITORY: Error updating survey session with id {entity.Id}");
            return new InternalResponse<SurveySession> { Success = false, Message = $"Error updating survey session with id {entity.Id}", Data = null };
        }
    }

    public InternalResponse<SurveySession> Delete(int id)
    {
        try
        {
            var session = db.SurveySessions.FirstOrDefault(s => s.Id == id);
            if (session == null)
            {
                logger.LogWarning($"Survey session with id = {id} not found for deletion");
                return new InternalResponse<SurveySession> { Success = false, Message = "Survey session not found", Data = null };
            }
            db.SurveySessions.Remove(session);
            db.SaveChanges();
            logger.LogInformation($"Survey session with id = {id} was deleted from the database");
            return new InternalResponse<SurveySession> { Success = true, Data = session };
        }
         catch (Exception ex)
         {
             logger.LogError(ex, $"ERROR AT REPOSITORY: Error deleting survey session with id {id}");
             return new InternalResponse<SurveySession> { Success = false, Message = $"Error deleting survey session with id {id}", Data = null };
         }
    }

}