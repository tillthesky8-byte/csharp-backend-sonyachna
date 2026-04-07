namespace DTOs
{
    public class MarkTodoDoneRequest
    {
        public int TodoId { get; set; }
    }

    public class MarkTodoDoneResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
    }
}