namespace DTOs
{
    public class DreamEntryByDateRequest
    {
        public DateOnly Date { get; set; }
    }

    public class DreamEntryByDateResponse
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateOnly Date { get; set; }
    }
}