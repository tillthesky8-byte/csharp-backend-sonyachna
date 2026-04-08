namespace DTOs
{
    //request is handled by route parameter

    public class MarkTodoDoneResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
    }
}