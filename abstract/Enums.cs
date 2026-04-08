public enum TodoStatus 
{
    NotStarted = 0,
    InProgress = 1,
    Completed = 2,
    Failed = 3
}

public enum TodoScope
{
    Daily = 0,
    MiddleTerm = 1,
    LongTerm = 2
}

public class InternalResponse
{
    public bool Success { get; set; }
    public string? Message { get; set; }
}

public class InternalResponse<T>
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }
}
