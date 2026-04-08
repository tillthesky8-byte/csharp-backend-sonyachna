//Typo in the file name, should be "AnswerRepository.cs" instead of "AnswersRepositry.cs". Future reminder to fix the file name.
//Inacuracy in the class name, should be "AnswerRepository" instead of "AnswersRepository". Future reminder to fix the class name.
public class AnswerRepository : IRepository<Answer>
{
    private readonly AppDbContext db;
    private readonly ILogger<AnswerRepository> logger;
    public AnswerRepository(AppDbContext db, ILogger<AnswerRepository> logger)
    {
        this.db = db;
        this.logger = logger;   
    }

    public InternalResponse<List<Answer>> GetAll()
    {
        try
        {
            var answers = db.Answers.ToList();
            logger.LogInformation($"All answers were retrived from the database; count: {answers.Count}");
            return new InternalResponse<List<Answer>> { Success = true, Data = answers };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "ERROR AT REPOSITORY: Error fetching all answers");
            return new InternalResponse<List<Answer>> { Success = false, Message = "Error fetching all answers", Data = null };
        }   
    }

    public InternalResponse<Answer> GetById(int id)
    {
       try
        {
            var answer = db.Answers.FirstOrDefault(a => a.Id == id);
            logger.LogInformation($"Answer with id = {id} was retrived from the database");
            return new InternalResponse<Answer> { Success = true, Data = answer };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"ERROR AT REPOSITORY: Error fetching answer with id {id}");
            return new InternalResponse<Answer> { Success = false, Message = $"Error fetching answer with id {id}", Data = null };
        }
    }

    public InternalResponse<Answer> Add(Answer entity)
    {
        try
        {
            db.Answers.Add(entity);
            db.SaveChanges();
            logger.LogInformation($"Answer with id = {entity.Id} was added to the database");
            return new InternalResponse<Answer> { Success = true, Data = entity };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "ERROR AT REPOSITORY: Error adding new answer");
            return new InternalResponse<Answer> { Success = false, Message = "Error adding new answer", Data = null };
        }
    }

    public InternalResponse<Answer> Update(Answer entity)
    {
        try
        {
            var existingAnswer = db.Answers.FirstOrDefault(a => a.Id == entity.Id);
            if (existingAnswer == null)
            {
                return new InternalResponse<Answer> { Success = false, Message = "Answer not found"};
            }
            else
            {
                existingAnswer.Response = entity.Response;
                existingAnswer.Remark = entity.Remark;
                db.SaveChanges();
                logger.LogInformation($"The answer with id = {existingAnswer.Id} is updated succesfully");
                return new InternalResponse<Answer> { Success = true, Data = existingAnswer };
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"ERROR AT REPOSITORY: Error updating answer with id {entity.Id}");
            return new InternalResponse<Answer> { Success = false, Message = $"Error updating answer with id {entity.Id}", Data = null };
        }
    }

    public InternalResponse<Answer> Delete(int id)
    {
        try
        {
            var answer = db.Answers.FirstOrDefault(a => a.Id == id);
            if (answer == null)
            {
                return new InternalResponse<Answer> { Success = false, Message = "Answer not found" };
            }
            db.Answers.Remove(answer);
            db.SaveChanges();
            logger.LogInformation($"The answer with id = {id} is deleted succesfully");
            return new InternalResponse<Answer> { Success = true, Data = answer };
            
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"ERROR AT REPOSITORY: Error deleting answer with id {id}");
            return new InternalResponse<Answer> { Success = false, Message = $"Error deleting answer with id {id}", Data = null };
        }
    }
}