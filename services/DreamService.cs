public class DreamService
{
    private readonly IRepository<DreamEntry> dreamEntryRepository;
    private readonly ILogger<DreamService> logger;

    public DreamService(IRepository<DreamEntry> dreamEntryRepository,ILogger<DreamService> logger)
    {

        this.dreamEntryRepository = dreamEntryRepository;
        this.logger = logger;
    }

    public InternalResponse<List<DreamEntry>> RetriveDreamEntriesByDate(DateOnly date)
    {
        try
        {
            var repoResponse = dreamEntryRepository.GetAll();
            if (!repoResponse.Success)
            {
                return new InternalResponse<List<DreamEntry>> { Success = false };
            }

            var entries = repoResponse.Data;
            var filteredEntries = entries!.Where(e => e.Date == date).ToList();
            return new InternalResponse<List<DreamEntry>> { Success = true, Data = filteredEntries };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "ERROR AT SERVICE: Error retrieving dream entries by date");
            return new InternalResponse<List<DreamEntry>> { Success = false };
        }
    }
}