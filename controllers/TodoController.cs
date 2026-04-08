using DTOs;
using Microsoft.AspNetCore.Mvc;

// The controller is tested and ready for use
// NOTES are set
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

    [HttpGet("")]
    public IActionResult GetAllTodos()
    {
        var repoResponse = repo.GetAll();
        if (repoResponse.Success)
        {
            logger.LogInformation($"Successfully retrieved all todos; count: {repoResponse.Data!.Count}");
            return Ok(repoResponse.Data);
        }
        logger.LogError("Failed to retrieve todos");
        return BadRequest("Failed to retrieve todos");
    }
    // curl -X GET http://localhost:5155/api/todos
    // Request for getting all todos available. 
    // Note: the endpoint is not paginated, fix in future versions.

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
        var repoResponse = repo.Add(newTodo);
        if (repoResponse.Success)        {
            logger.LogInformation($"Todo with id = {repoResponse.Data!.Id} was successfully created");
            return Ok(new CreateTodoResponse { Success = true, Message = "Todo created successfully", TodoId = repoResponse.Data.Id });
        }
        logger.LogError("Failed to create todo");
        return BadRequest(new CreateTodoResponse { Success = false, Message = "Failed to create todo" });
    }
    // curl -X POST http://localhost:5155/api/todos -H "Content-Type: application/json" -d '{"description":"Finish the project","scope":1,"dueAt":"2024-06-30T23:59:59Z"}'
    // Request for adding a new todo.
    // Note: the endpoint creates and object, which should be handled by the deeper layers.


    [HttpPut("{id}")]
    public IActionResult UpdateTodo(int id, UpdateTodoRequest request)
    {
        var updateTodo = new Todo
        {
            Id = id,
            Description = request.Description,
            Scope = (TodoScope)request.Scope!,
            Status = (TodoStatus)request.Status!,
            DueAt = request.DueAt,
        };
        var repoResponse = repo.Update(updateTodo);
        if (repoResponse.Success)        {
            logger.LogInformation($"Todo with id = {repoResponse.Data!.Id} was successfully updated");
            return Ok(new UpdateTodoResponse { Success = true, Message = "Todo updated successfully" });
        }
        logger.LogError($"Failed to update todo with id = {id}");
        return BadRequest(new UpdateTodoResponse { Success = false, Message = $"Failed to update todo with id {id}" });

    }
    // curl -X PUT http://localhost:5155/api/todos/{id} -H "Content-Type: application/json" -d '{"todoId":{id},"description":"Finish the project with updates","scope":2,"status":1,"dueAt":"2024-07-15T23:59:59Z"}'
    // Request for updating an existing todo.
    // Note: the endpoint updates the whole object, which should be handled by the deeper layers


    [HttpPatch("mark-completed/{id}")]
    public IActionResult MarkTodoAsCompleted(int id)
    {
        var serviceResponse = service.MarkTodoCompleted(id);
        if (serviceResponse.Success)       
        {
            logger.LogInformation($"Todo with id = {id} was marked as completed");
            return Ok(new MarkTodoDoneResponse { Success = true, Message = "Todo marked as completed successfully" });
        }
        logger.LogError($"Failed to mark todo with id = {id} as completed");
        return BadRequest(new MarkTodoDoneResponse { Success = false, Message = $"Failed to mark todo with id {id} as completed" });
    }
    // curl -X PATCH http://localhost:5155/api/todos/mark-completed/{id}
    // Request for marking a todo as completed by id.
    // Note: also marking an already completed or failed todo as completed again returns success, needs fixing in service layer (should return false if the todo is already completed or failed)


    [HttpPatch("mark-failed/{id}")]
    public IActionResult MarkTodoAsFailed(int id)
    {
        var serviceResponse = service.MarkTodoFailed(id);
        if (serviceResponse.Success)        {
            logger.LogInformation($"Todo with id = {id} was marked as failed");
            return Ok(new MarkTodoFailedResponse { Success = true, Message = "Todo marked as failed successfully" });
        }
        logger.LogError($"Failed to mark todo with id = {id} as failed");
        return BadRequest(new MarkTodoFailedResponse { Success = false, Message = $"Failed to mark todo with id {id} as failed" });
    }
    // curl -X PATCH http://localhost:5155/api/todos/mark-failed/{id}
    // Request for marking a todo as failed by id.
    // Note: also marking an already completed or failed todo as failed again returns success, needs fixing in service layer (should return false if the todo is already completed or failed)
}
