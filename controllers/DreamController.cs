using DTOs;
using Microsoft.AspNetCore.Mvc;
// The controller is tested and ready for use
/* 
Notes for future improvements:
1. Optimize Update method to only update fields that are provided in the request.

*/
[ApiController]
[Route("api/[controller]")]
public class DreamEntriesController : ControllerBase
{
    private readonly ILogger<DreamEntriesController> logger;
    private readonly IRepository<DreamEntry> repo;
    private readonly DreamService service;

    public DreamEntriesController(IRepository<DreamEntry> repo, DreamService service, ILogger<DreamEntriesController> logger)
    {
        this.repo = repo;
        this.service = service;
        this.logger = logger;
    }

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

    [HttpGet("by-date")]
    public IActionResult GetDreamEntriesByDate(DreamEntryByDateRequest request)
    {
        var entries = service.RetriveDreamEntriesByDate(request.Date);
        logger.LogInformation($"Retrieved {entries.Count} dream entries for date {request.Date}");
        return Ok(entries);
    }

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

}

/*
1. Get all dream entries: (tested)
curl -X GET http://localhost:5155/api/dreamentries  
2. Add a new dream entry: (tested)
curl -X POST http://localhost:5155/api/dreamentries -H "Content-Type: application/json" -d '{"Content": "I had a dream about flying", "IsLucid": true}'
3. Update an existing dream entry (replace {id} with the actual id of the dream entry you want to update): (tested))
curl -X PUT http://localhost:5155/api/dreamentries/{id} -H "Content-Type: application/json" -d '{"Content": "I had a dream about flying over mountains", "IsLucid": false}'
4. Get dream entries by date: (tested)
curl -X GET "http://localhost:5155/api/dreamentries/by-date" -H "Content-Type: application/json" -d '{"Date": "2024-06-01"}'
5. Delete a dream entry (replace {id} with the actual id of the dream entry you want to delete): (tested)
curl -X DELETE http://localhost:5155/api/dreamentries/{id}
*/          