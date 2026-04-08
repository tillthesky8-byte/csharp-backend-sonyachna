public class TododsRepository : IRepository<Todo>
{
    private readonly AppDbContext db;
    private readonly ILogger<TododsRepository> logger;

    public TododsRepository(AppDbContext db, ILogger<TododsRepository> logger)
    {
        this.db = db;
        this.logger = logger;
    }

    public InternalResponse<Todo> GetById(int id)
    {
        try
        {
            var todo = db.Todos.FirstOrDefault(t => t.Id == id);
            logger.LogInformation($"Todo with id = {id} was retrived from the database");
            return new InternalResponse<Todo> { Success = true, Data = todo };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"ERROR AT REPOSITORY: Error fetching todo with id {id}");
            return new InternalResponse<Todo> { Success = false, Message = $"Error fetching todo with id {id}", Data = null };
        }
    }

    public InternalResponse<List<Todo>> GetAll()
    {
        try
        {
            var todos = db.Todos.ToList();
            logger.LogInformation($"All todos were retrived from the database; count: {todos.Count}");
            return new InternalResponse<List<Todo>> { Success = true, Data = todos };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "ERROR AT REPOSITORY: Error fetching all todos");
            return new InternalResponse<List<Todo>> { Success = false, Message = "Error fetching all todos", Data = null };
        }
    }

    public InternalResponse<Todo> Add(Todo entity)
    {
        try
        {
            db.Todos.Add(entity);
            db.SaveChanges();
            logger.LogInformation($"Todo with id = {entity.Id} was added to the database");
            return new InternalResponse<Todo> { Success = true, Data = entity };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "ERROR AT REPOSITORY: Error adding new todo");
            return new InternalResponse<Todo> { Success = false, Message = "Error adding new todo", Data = null };
        }
    }

    public InternalResponse<Todo> Update(Todo entity)
    {
        try
        {
            var existingTodo = db.Todos.FirstOrDefault(t => t.Id == entity.Id);
            if (existingTodo == null)
            {
                logger.LogWarning($"Todo with id = {entity.Id} was not found for update");
                return new InternalResponse<Todo> { Success = false, Message = $"Todo with id {entity.Id} not found", Data = null };
            }

            existingTodo.Description = entity.Description;
            existingTodo.Status = entity.Status;
            existingTodo.Status = entity.Status;
            existingTodo.DueAt = entity.DueAt;
            existingTodo.UpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            existingTodo.CompletedAt = entity.CompletedAt;

            db.SaveChanges();
            logger.LogInformation($"Todo with id = {entity.Id} was updated in the database");
            return new InternalResponse<Todo> { Success = true, Data = existingTodo };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"ERROR AT REPOSITORY: Error updating todo with id {entity.Id}");
            return new InternalResponse<Todo> { Success = false, Message = $"Error updating todo with id {entity.Id}", Data = null };
        }
    }
    public InternalResponse<Todo> Delete(int id)
    {
        try
        {
            var existingTodo = db.Todos.FirstOrDefault(t => t.Id == id);
            if (existingTodo == null)
            {
                logger.LogWarning($"Todo with id = {id} was not found for deletion");
                return new InternalResponse<Todo> { Success = false, Message = $"Todo with id {id} not found", Data = null };
            }

            db.Todos.Remove(existingTodo);
            db.SaveChanges();
            logger.LogInformation($"Todo with id = {id} was deleted from the database");
            return new InternalResponse<Todo> { Success = true, Data = existingTodo }; 
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"ERROR AT REPOSITORY: Error deleting todo with id {id}");
            return new InternalResponse<Todo> { Success = false, Message = $"Error deleting todo with id {id}", Data = null };
        }
    }
}