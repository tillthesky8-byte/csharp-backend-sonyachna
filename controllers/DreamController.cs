using DTOs;
using Microsoft.AspNetCore.Mvc;

// The controller is tested and ready for use
// NOTES are set

[ApiController]
[Route("api/[controller]")]
public class DreamEntriesController : ControllerBase
{
    // CONFIGURATION
    private readonly ILogger<DreamEntriesController> logger;
    private readonly IRepository<DreamEntry> repo;
    private readonly DreamService service;

    public DreamEntriesController(IRepository<DreamEntry> repo, DreamService service, ILogger<DreamEntriesController> logger)
    {
        this.repo = repo;
        this.service = service;
        this.logger = logger;
    }

    // ENDPOINTS
    [HttpGet("")]
    public IActionResult GetAllDreamEntries()
    {
        var response = repo.GetAll();
        if (response.Success)
        {
            logger.LogInformation($"Successfully retrieved all dream entries; count: {response.Data!.Count}");
            return Ok(response.Data);
        }
        logger.LogError("Failed to retrieve dream entries");
        return BadRequest("Failed to retrieve dream entries");
    }
    // curl -X GET http://localhost:5155/api/dreamentries
    // Request for getting all dream entries available. 
    // Note: the endpoint is not paginated, fix in future versions.


    [HttpPost("")]
    public IActionResult AddDreamEntry(CreateDreamEntryRequest request)
    {
        var newEntry = new DreamEntry
        {
            Content = request.Content,
            Date = DateOnly.FromDateTime(DateTime.UtcNow),
            IsLucid = request.IsLucid
        };
        var response = repo.Add(newEntry);
        if (response.Success)
        {
            logger.LogInformation("Successfully created dream entry");
            return Ok(response.Data);
        }
        logger.LogError("Failed to create dream entry");
        return BadRequest("Failed to create dream entry");
    }
    // curl -X POST http://localhost:5155/api/dreamentries -H "Content-Type: application/json" -d '{"Content": "I had a dream about flying", "IsLucid": true}'
    // Request for adding a new dream entry.
    // Note: the endpoint creates and object, which should be handled by the deeper layers.


    [HttpPut("{id}")]
    public IActionResult UpdateDreamEntry(int id, UpdateDreamEntryRequest request)
    {
        var updateEntry = new DreamEntry
        {
            Id = id,
            Content = request.Content,
            IsLucid = request.IsLucid
        };
        var response = repo.Update(updateEntry);
        if (response.Success)
        {
            logger.LogInformation($"Dream entry with id = {response.Data!.Id} was successfully updated");
            return Ok(response.Data);
        }
        logger.LogError($"Failed to update dream entry with id = {id}");
        return BadRequest("Failed to update dream entry");
    }
    // curl -X PUT http://localhost:5155/api/dreamentries/{id} -H "Content-Type: application/json" -d '{"Content": "I had a dream about flying over mountains", "IsLucid": false}'
    // Request for updating an existing dream entry.
    // Note: the endpoint updates the whole object, which should be handled by the deeper layers

    [HttpGet("{date}")]
    public IActionResult GetDreamEntriesByDate(DateOnly date)
    {
        var serviceResponse = service.RetriveDreamEntriesByDate(date);
        if (serviceResponse.Success)
        {
            logger.LogInformation($"Retrieved {serviceResponse.Data!.Count} dream entries for date {date}");
            return Ok(serviceResponse.Data);
        }
        logger.LogError("Failed to retrieve dream entries by date");
        return BadRequest("Failed to retrieve dream entries by date");
    }
    // curl -X GET "http://localhost:5155/api/dreamentries/{date}"
    // Request for retrieving dream entries by date.

    [HttpDelete("{id}")]
    public IActionResult DeleteDreamEntry(int id)
    {
        var response = repo.Delete(id);
        if (response.Success)
        {
            logger.LogInformation($"Dream entry with id = {id} was successfully deleted");
            return Ok();
        }
        logger.LogError($"Failed to delete dream entry with id = {id}");
        return BadRequest("Failed to delete dream entry");
    }
    // curl -X DELETE http://localhost:5155/api/dreamentries/{id}
    // Request for deleting a dream entry by id.

}

