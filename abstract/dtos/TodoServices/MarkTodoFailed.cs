namespace DTOs
{
    public class MarkTodoFailedRequest
    {
        public int TodoId { get; set; }
    }

    public class MarkTodoFailedResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
    }
}