using DTOs;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class TodosController : ControllerBase
{
    private readonly ILogger<TodosController> logger;
    private readonly IRepository<Todo> repo;
    private readonly TodoService service;

    public TodosController(IRepository<Todo> repo, TodoService service, ILogger<TodosController> logger)
    {
        this.repo = repo;
        this.service = service;
        this.logger = logger;
    }

    [HttpGet()]
    public IActionResult GetAllTodos()
    {
        var response = repo.GetAll();
        if (response.Success)
        {
            logger.LogInformation($"Successfully retrieved all todos; count: {response.Data!.Count}");
            return Ok(response.Data);
        }
        logger.LogError("Failed to retrieve todos");
        return BadRequest("Failed to retrieve todos");
    }

    [HttpPost("")]
    public IActionResult AddTodo(CreateTodoRequest request)
    {
        var newTodo = new Todo
        {
            Description = request.Description,
            Scope = request.Scope,
            DueAt = request.DueAt,
            CreatedAt = DateTime.UtcNow,
            Status = TodoStatus.InProgress
        };
        var response = repo.Add(newTodo);
        if (response.Success)        {
            logger.LogInformation($"Todo with id = {response.Data!.Id} was successfully created");
            return Ok(new CreateTodoResponse { Success = true, Message = "Todo created successfully", TodoId = response.Data.Id });
        }
        logger.LogError("Failed to create todo");
        return BadRequest(new CreateTodoResponse { Success = false, Message = "Failed to create todo" });
    }

    [HttpPut("{id}")]
    public IActionResult UpdateTodo(UpdateTodoRequest request)
    {
        var updateTodo = new Todo
        {
            Description = request.Description,
            Scope = (TodoScope)request.Scope!,
            Status = (TodoStatus)request.Status!,
            DueAt = request.DueAt,
        };
        var response = repo.Update(updateTodo);
        if (response.Success)        {
            logger.LogInformation($"Todo with id = {response.Data!.Id} was successfully updated");
            return Ok(new UpdateTodoResponse { Success = true, Message = "Todo updated successfully" });
        }
        logger.LogError($"Failed to update todo with id = {request.TodoId}");
        return BadRequest(new UpdateTodoResponse { Success = false, Message = $"Failed to update todo with id {request.TodoId}" });

    }

    [HttpPatch("{id}")]
    public IActionResult MarkTodoAsCompleted(MarkTodoDoneRequest request)
    {
        var result = service.MarkTodoCompleted(request.TodoId);
        if (result)       
        {
            logger.LogInformation($"Todo with id = {request.TodoId} was marked as completed");
            return Ok(new MarkTodoDoneResponse { Success = true, Message = "Todo marked as completed successfully" });
        }
        logger.LogError($"Failed to mark todo with id = {request.TodoId} as completed");
        return BadRequest(new MarkTodoDoneResponse { Success = false, Message = $"Failed to mark todo with id {request.TodoId} as completed" });
    }

    public IActionResult MarkTodoAsFailed(MarkTodoFailedRequest request)
    {
        var result = service.MarkTodoFailed(request.TodoId);
        if (result)        {
            logger.LogInformation($"Todo with id = {request.TodoId} was marked as failed");
            return Ok(new MarkTodoFailedResponse { Success = true, Message = "Todo marked as failed successfully" });
        }
        logger.LogError($"Failed to mark todo with id = {request.TodoId} as failed");
        return BadRequest(new MarkTodoFailedResponse { Success = false, Message = $"Failed to mark todo with id {request.TodoId} as failed" });
    }
}