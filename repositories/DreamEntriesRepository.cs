public class DreamEntriesRepository : IRepository<DreamEntry>
{
    private readonly AppDbContext db;
    private readonly ILogger<DreamEntriesRepository> logger;

    public DreamEntriesRepository(AppDbContext db, ILogger<DreamEntriesRepository> logger)
    {
        this.db = db;
        this.logger = logger;
    }

    public InternalResponse<List<DreamEntry>> GetAll()
    {
        try
        {
            var dreamEntries = db.DreamEntries.ToList();
            logger.LogInformation($"All dream entries were retrived from the database; count: {dreamEntries.Count}");
            return new InternalResponse<List<DreamEntry>> { Success = true, Data = dreamEntries };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching all dream entries");
            return new InternalResponse<List<DreamEntry>> { Success = false, Message = "Error fetching all dream entries", Data = null };
        }
    }
    public InternalResponse<DreamEntry> GetById(int id)
    {
        try
         {
             var dreamEntry = db.DreamEntries.FirstOrDefault(de => de.Id == id);
             logger.LogInformation($"Dream entry with id = {id} was retrived from the database");
             return new InternalResponse<DreamEntry> { Success = true, Data = dreamEntry };
         }
         catch (Exception ex)         
         {
            logger.LogError(ex, $"Error fetching dream entry with id {id}");
            return new InternalResponse<DreamEntry> { Success = false, Message = $"Error fetching dream entry with id {id}", Data = null };
         }
    }

    public InternalResponse<DreamEntry> Add(DreamEntry entity)
    {
        try
        {
            db.DreamEntries.Add(entity);
            db.SaveChanges();
            logger.LogInformation($"Dream entry with id = {entity.Id} was added to the database");
            return new InternalResponse<DreamEntry> { Success = true, Data = entity };   
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error adding new dream entry");
            return new InternalResponse<DreamEntry> { Success = false, Message = "Error adding new dream entry", Data = null };
        }
    }

    public InternalResponse<DreamEntry> Update(DreamEntry entity)
    {
        try
        {
            var existingDreamEntry = db.DreamEntries.FirstOrDefault(de => de.Id == entity.Id);
            if (existingDreamEntry == null)
            {
                logger.LogWarning($"Dream entry with id = {entity.Id} was not found for update");
                return new InternalResponse<DreamEntry> { Success = false, Message = $"Dream entry with id {entity.Id} not found for update" };  
            }
            existingDreamEntry.Content = entity.Content;
            existingDreamEntry.IsLucid = entity.IsLucid;
            db.SaveChanges();
            logger.LogInformation($"Dream entry with id = {entity.Id} was updated in the database");
            return new InternalResponse<DreamEntry> { Success = true, Data = existingDreamEntry };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Error updating dream entry with id {entity.Id}");
            return new InternalResponse<DreamEntry> { Success = false, Message = $"Error updating dream entry with id {entity.Id}", Data = null };
        }
    }

    public InternalResponse<DreamEntry> Delete(int id)
    {
        try
        {
            var existingDreamEntry = db.DreamEntries.FirstOrDefault(de => de.Id == id);
            if (existingDreamEntry == null)
            {
                logger.LogWarning($"Dream entry with id = {id} was not found for deletion");
                return new InternalResponse<DreamEntry> { Success = false, Message = $"Dream entry with id {id} not found for deletion", Data = null };
            }
            db.DreamEntries.Remove(existingDreamEntry);
            db.SaveChanges();
            logger.LogInformation($"Dream entry with id = {id} was deleted from the database");
            return new InternalResponse<DreamEntry> { Success = true, Data = existingDreamEntry };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Error deleting dream entry with id {id}");
            return new InternalResponse<DreamEntry> { Success = false, Message = $"Error deleting dream entry with id {id}", Data = null };
        }
    }
}