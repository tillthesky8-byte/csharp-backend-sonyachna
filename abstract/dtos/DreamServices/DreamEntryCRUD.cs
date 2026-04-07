namespace DTOs
{
    // post operations
    public class CreateDreamEntryRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }
    }

    public class CreateDreamEntryResponse
    {
        public int DreamEntryId { get; set; }
        public DateTime CreationDate { get; set; }
    }

    // put operations
    public class UpdateDreamEntryRequest
    {
        public int DreamEntryId { get; set; }
        public string Content { get; set; } = string.Empty;
        public bool IsLucid { get; set; }
    }

    public class UpdateDreamEntryResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
    }

    // get operations
    public class GetAllDreamEntriesResponse
    {
        public List<DreamEntry> DreamEntries { get; set; } = new List<DreamEntry>();
    }
    public class GetDreamEntryRequest
    {
        public int DreamEntryId { get; set; }
    }

    public class GetDreamEntryResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public DreamEntry? DreamEntry { get; set; }
    }

    // delete operations
    public class DeleteDreamEntryRequest
    {
        public int DreamEntryId { get; set; }
    }
    public class DeleteDreamEntryResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
    }
}