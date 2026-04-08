using DTOs;
using Microsoft.AspNetCore.Mvc;


// the controller is working fine, but further improvements can be made such as:
// 1. Validation of request bodies (e.g., check for null or invalid values)
// 2. More specific error messages in case of failures (e.g., distinguish between not found and database errors)
// 3. Consider using async/await for better scalability if the repository methods support it
// 4. Remove redundant todoId from the request body in update and patch endpoints, as the id is already provided in the route
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

    /* Notes:
    1. Redundant todoId,
    2. Zero id problem, (Fixed)
    3. Should check whether an update is actually needed.
    */
    [HttpPut("{id}")]
    public IActionResult UpdateTodo(UpdateTodoRequest request)
    {
        var updateTodo = new Todo
        {
            Id = request.TodoId,
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

    /* Notes:
        1. Redundant todoId,
        2. Marking an already completed todo returns success.
    */
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

    /* Notes:
        1. Redundant todoId,
        2. Marking an already failed todo returns success.
    */
    [HttpPatch("{id}/mark-failed")]
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

//the curl requests to test all the endpoints of the todo controller for port 5155.
/*
1. Get all todos:
curl -X GET "http://localhost:5155/api/todos" //works
2. Add a new todo:
curl -X POST "http://localhost:5155/api/todos" -H "Content-Type: application/json" -d '{"description":"Finish the project","scope":1,"dueAt":"2024-06-30T23:59:59Z"}' // works
3. Update an existing todo (replace {id} with the actual todo id):
curl -X PUT "http://localhost:5155/api/todos/{id}" -H "Content-Type: application/json" -d '{"todoId":{id},"description":"Finish the project with updates","scope":2,"status":1,"dueAt":"2024-07-15T23:59:59Z"}' // zero id problem, redundant todoId in body, needs fixing
4. Mark a todo as completed (replace {id} with the actual todo id):
curl -X PATCH "http://localhost:5155/api/todos/{id}" -H "Content-Type: application/json" -d '{"todoId":{id}}' // redundant todoId in body, needs fixing // completing an already completed todo returns success, needs fixing in service layer (should return false if the todo is already completed or failed)
5. Mark a todo as failed (replace {id} with the actual todo id):
curl -X PATCH "http://localhost:5155/api/todos/{id}/mark-failed" -H "Content-Type: application/json" -d '{"todoId":{id}}' // redundant todoId in body, needs fixing // also marking an already failed todo as failed again returns success, needs fixing in service layer (should return false if the todo is already completed or failed)
*/  