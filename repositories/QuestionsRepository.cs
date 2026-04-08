public class QuestionsRepository : IRepository<Question>
{
    private readonly AppDbContext db;
    private readonly ILogger<QuestionsRepository> logger;

    public QuestionsRepository(AppDbContext db, ILogger<QuestionsRepository> logger)
    {
        this.db = db;
        this.logger = logger;
    }

    public InternalResponse<List<Question>> GetAll()
    {
        try
        {
            var questions = db.Questions.ToList();
            logger.LogInformation($"All questions were retrived from the database; count: {questions.Count}");
            return new InternalResponse<List<Question>> { Success = true, Data = questions };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "ERROR AT REPOSITORY: Error fetching all questions");
            return new InternalResponse<List<Question>> { Success = false, Message = "Error fetching all questions", Data = null };
        }
    }

    public InternalResponse<Question> GetById(int id)
    {
        try
        {
            var question = db.Questions.FirstOrDefault(q => q.Id == id);
            logger.LogInformation($"Question with id = {id} was retrived from the database");
            return new InternalResponse<Question> { Success = true, Data = question };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"ERROR AT REPOSITORY: Error fetching question with id {id}");
            return new InternalResponse<Question> { Success = false, Message = $"Error fetching question with id {id}", Data = null };
        }
    }

    public InternalResponse<Question> Add(Question entity)
    {
        try
        {
            db.Questions.Add(entity);
            db.SaveChanges();
            logger.LogInformation($"Question with id = {entity.Id} was added to the database");
            return new InternalResponse<Question> { Success = true, Data = entity };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"ERROR AT REPOSITORY: Error adding question with id {entity.Id}");
            return new InternalResponse<Question> { Success = false, Message = $"Error adding question with id {entity.Id}", Data = null };
        }
    }

    public InternalResponse<Question> Update(Question entity)
    {
        try
        {
            var existingQuestion = db.Questions.FirstOrDefault(q => q.Id == entity.Id);
            if (existingQuestion == null)
            {
                logger.LogWarning($"Question with id = {entity.Id} not found for update");
                return new InternalResponse<Question> { Success = false, Message = $"Question with id {entity.Id} not found", Data = null };
            }

            existingQuestion.Code = entity.Code;
            existingQuestion.Text = entity.Text;
            db.SaveChanges();
            logger.LogInformation($"Question with id = {entity.Id} was updated in the database");
            return new InternalResponse<Question> { Success = true, Data = existingQuestion };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"ERROR AT REPOSITORY: Error updating question with id {entity.Id}");
            return new InternalResponse<Question> { Success = false, Message = $"Error updating question with id {entity.Id}", Data = null };
        }
    }

    public InternalResponse<Question> Delete(int id)
    {
        try
        {
            var existingQuestion = db.Questions.FirstOrDefault(q => q.Id == id);
            if (existingQuestion == null)
            {
                logger.LogWarning($"Question with id = {id} not found for deletion");
                return new InternalResponse<Question> { Success = false, Message = $"Question with id {id} not found" };
            }

            db.Questions.Remove(existingQuestion);
            db.SaveChanges();
            logger.LogInformation($"Question with id = {id} was deleted from the database");
            return new InternalResponse<Question> { Success = true, Data = existingQuestion };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"ERROR AT REPOSITORY: Error deleting question with id {id}");
            return new InternalResponse<Question> { Success = false, Message = $"Error deleting question with id {id}", Data = null };
        }
    }
}