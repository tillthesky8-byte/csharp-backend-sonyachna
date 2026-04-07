using System.Security.Cryptography.X509Certificates;

public class TodoService
{
    private readonly IRepository<Todo> repo;
    private readonly ILogger<TodoService> logger;

    public TodoService(IRepository<Todo> repo, ILogger<TodoService> logger)
    {
        this.repo = repo;
        this.logger = logger;
    }

    public bool MarkTodoCompleted(int todoId)
    {
        try
        {
            var getResponse = repo.GetById(todoId);
            if (!getResponse.Success || getResponse.Data == null)
            {
                logger.LogWarning($"Todo with id {todoId} not found");
                return false;
            }

            var todo = getResponse.Data;
            if (todo.Status == TodoStatus.Completed)
            {
                logger.LogInformation($"Todo with id {todoId} is already marked as completed");
                return true;
            }

            todo.Status = TodoStatus.Completed;
            var updateResponse = repo.Update(todo);
            if (!updateResponse.Success)
            {
                logger.LogError($"Failed to update todo with id {todoId}: {updateResponse.Message}");
                return false;
            }

            logger.LogInformation($"Todo with id {todoId} was marked as completed");
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Error marking todo with id {todoId} as completed");
            return false;
        }

    }
    public bool MarkTodoFailed(int todoId)
    {
        try
        {
            var getResponse = repo.GetById(todoId);
            if (!getResponse.Success || getResponse.Data == null)
            {
                logger.LogWarning($"Todo with id {todoId} not found");
                return false;
            }

            var todo = getResponse.Data;
            if (todo.Status == TodoStatus.Failed)
            {
                logger.LogInformation($"Todo with id {todoId} is already marked as failed");
                return true; 
            }

            todo.Status = TodoStatus.Failed;
            var updateResponse = repo.Update(todo);
            if (!updateResponse.Success)
            {
                logger.LogError($"Failed to update todo with id {todoId}: {updateResponse.Message}");
                return false;
            }

            logger.LogInformation($"Todo with id {todoId} was marked as failed");
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Error marking todo with id {todoId} as failed");
            return false;
        }
    }
}   