using DTOs;
using Microsoft.AspNetCore.Mvc;

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
    public IActionResult UpdateDreamEntry(UpdateDreamEntryRequest request)
    {
        var updateEntry = new DreamEntry
        {
            Content = request.Content,
            IsLucid = request.IsLucid
        };
        var response = repo.Update(updateEntry);
        if (response.Success)
        {
            logger.LogInformation($"Dream entry with id = {response.Data!.Id} was successfully updated");
            return Ok(response.Data);
        }
        logger.LogError($"Failed to update dream entry with id = {request.DreamEntryId}");
        return BadRequest("Failed to update dream entry");
    }

    [HttpGet("by-date")]
    public IActionResult GetDreamEntriesByDate(DreamEntryByDateRequest request)
    {
        var (entries, success) = service.RetriveDreamEntriesByDate(request.Date);
        if (!success)
        {
            logger.LogError("Failed to retrieve dream entries by date");
            return BadRequest("Failed to retrieve dream entries by date");
        }
        logger.LogInformation($"Retrieved {entries.Count} dream entries for date {request.Date}");
        return Ok(entries);
    }
}