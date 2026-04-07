public class DreamService
{
    private readonly IRepository<DreamEntry> dreamEntryRepository;
    private readonly ILogger<DreamService> logger;

    public DreamService(
        IRepository<DreamEntry> dreamEntryRepository,
        ILogger<DreamService> logger)
    {

        this.dreamEntryRepository = dreamEntryRepository;
        this.logger = logger;
    }

    public List<DreamEntry> RetriveDreamEntriesByDate(DateOnly date)
    {
        try
        {
            var allEntriesResponse = dreamEntryRepository.GetAll();
            if (!allEntriesResponse.Success)
            {
                logger.LogError("Failed to retrieve dream entries: {Message}", allEntriesResponse.Message);
                return new List<DreamEntry>();
            }

            var entries = allEntriesResponse.Data;
            var filteredEntries = entries!.Where(e => e.Date == date).ToList();
            logger.LogInformation($"Retrieved {filteredEntries.Count} dream entries for date {date}");
            return filteredEntries;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving dream entries by date");
            return new List<DreamEntry>();
        }
    }
}