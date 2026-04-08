
public class TodoService
{
    private readonly IRepository<Todo> repo;
    private readonly ILogger<TodoService> logger;

    public TodoService(IRepository<Todo> repo, ILogger<TodoService> logger)
    {
        this.repo = repo;
        this.logger = logger;
    }

    public InternalResponse<bool> MarkTodoCompleted(int todoId)
    {
        try
        {
            var getResponse = repo.GetById(todoId);
            if (!getResponse.Success || getResponse.Data == null)
            {
                logger.LogWarning($"Todo with id {todoId} not found");
                return new InternalResponse<bool> { Success = false, Message = "Todo not found" };
            }

            var todo = getResponse.Data;
            if (todo.Status == TodoStatus.Completed)
            {
                logger.LogInformation($"Todo with id {todoId} is already marked as completed");
                return new InternalResponse<bool> { Success = true, Message = "Todo is already completed" };
            }

            todo.Status = TodoStatus.Completed;
            var updateResponse = repo.Update(todo);
            if (!updateResponse.Success)
            {
                logger.LogError($"Failed to update todo with id {todoId}: {updateResponse.Message}");
                return new InternalResponse<bool> { Success = false, Message = "Failed to update todo" };
            }

            logger.LogInformation($"Todo with id {todoId} was marked as completed");
            return new InternalResponse<bool> { Success = true, Message = "Todo marked as completed successfully" };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"ERROR AT SERVICE: Error marking todo with id {todoId} as completed");
            return new InternalResponse<bool> { Success = false, Message = "Error marking todo as completed" };
        }

    }
    public InternalResponse<bool> MarkTodoFailed(int todoId)
    {
        try
        {
            var getResponse = repo.GetById(todoId);
            if (!getResponse.Success || getResponse.Data == null)
            {
                logger.LogWarning($"Todo with id {todoId} not found");
                return new InternalResponse<bool> { Success = false, Message = "Todo not found" };
            }

            var todo = getResponse.Data;
            if (todo.Status == TodoStatus.Failed)
            {
                logger.LogInformation($"Todo with id {todoId} is already marked as failed");
                return new InternalResponse<bool> { Success = true, Message = "Todo is already failed" };
            }

            todo.Status = TodoStatus.Failed;
            var updateResponse = repo.Update(todo);
            if (!updateResponse.Success)
            {
                logger.LogError($"Failed to update todo with id {todoId}: {updateResponse.Message}");
                return new InternalResponse<bool> { Success = false, Message = "Failed to update todo" };
            }

            logger.LogInformation($"Todo with id {todoId} was marked as failed");
            return new InternalResponse<bool> { Success = true, Message = "Todo marked as failed successfully" };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"ERROR AT SERVICE: Error marking todo with id {todoId} as failed");
            return new InternalResponse<bool> { Success = false, Message = "Error marking todo as failed" };
        }
    }
}   