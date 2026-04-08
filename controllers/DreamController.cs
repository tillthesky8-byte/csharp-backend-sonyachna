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
        var repoResponse = repo.GetAll();
        if (repoResponse.Success)
        {
            logger.LogInformation($"SENT; count: {repoResponse.Data!.Count}");
            return Ok(repoResponse.Data);
        }
        return BadRequest("Request failed");
    }
    // curl -X GET http://localhost:5155/api/dreamentries
    // Request for getting all dream entries available. 
    // Note: the endpoint is not paginated, fix in future versions.


    [HttpPost("")]
    public IActionResult AddDreamEntry(CreateDreamEntryRequest request)
    {
        // Find a way to handle creaction in deeper layers.
        var newEntry = new DreamEntry
        {
            Content = request.Content,
            Date = DateOnly.FromDateTime(DateTime.UtcNow),
            IsLucid = request.IsLucid
        };
        //--------------------------------------------------------------

        var repoResponse = repo.Add(newEntry);
        if (repoResponse.Success)
        {
            logger.LogInformation($"CREATED; id: {repoResponse.Data!.Id}");
            return Ok(repoResponse.Data);
        }
        return BadRequest("Request failed");
    }
    // curl -X POST http://localhost:5155/api/dreamentries -H "Content-Type: application/json" -d '{"Content": "I had a dream about flying", "IsLucid": true}'
    // Request for adding a new dream entry.
    // Note: the endpoint creates and object, which should be handled by the deeper layers.


    [HttpPut("{id}")]
    public IActionResult UpdateDreamEntry(int id, UpdateDreamEntryRequest request)
    {
        //find a way to handle update in deeper layers.
        var updateEntry = new DreamEntry
        {
            Id = id,
            Content = request.Content,
            IsLucid = request.IsLucid
        };
        //--------------------------------------------------------------

        var repoResponse = repo.Update(updateEntry);
        if (repoResponse.Success)
        {
            logger.LogInformation($"UPDATED; id: {repoResponse.Data!.Id}");
            return Ok(repoResponse.Data);
        }
        return BadRequest("Request failed");
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
            logger.LogInformation($"SENT; count: {serviceResponse.Data!.Count}");
            return Ok(serviceResponse.Data);
        }
        return BadRequest("Request failed");
    }
    // curl -X GET "http://localhost:5155/api/dreamentries/{date}"
    // Request for retrieving dream entries by date.

    [HttpDelete("{id}")]
    public IActionResult DeleteDreamEntry(int id)
    {
        var response = repo.Delete(id);
        if (response.Success)
        {
            logger.LogInformation($"DELETED; id: {id}");
            return Ok();
        }
        return BadRequest("Request failed");
    }
    // curl -X DELETE http://localhost:5155/api/dreamentries/{id}
    // Request for deleting a dream entry by id.

}

