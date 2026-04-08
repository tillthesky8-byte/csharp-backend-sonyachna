namespace DTOs
{
    //POST
    public class CreateTodoRequest
    {
        public string Description { get; set; } = string.Empty;
        public TodoScope Scope { get; set; }
        public long? DueAt { get; set; }

        //note for future: don't forget to check whether Todo Status is set to default value (which is 0) and if it is, set it to TodoStatus.Pending
    }

    public class CreateTodoResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
    }

    //GET 
    //request is handled by route parameter
    public class GetAllTodosResponse
    {
        public List<Todo> Todos { get; set; } = new List<Todo>();
    }

    //request is handled by route parameter
    public class GetTodoResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public Todo? Todo { get; set; }
    }

    //PUT
    public class UpdateTodoRequest
    {
        public int TodoId { get; set; }
        public string? Description { get; set; }
        public TodoScope? Scope { get; set; }
        public TodoStatus? Status { get; set; }
        public long? DueAt { get; set; }
    }

    public class UpdateTodoResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
    }

    //DELETE
    //request is handled by route parameter
    
    public class DeleteTodoResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
    }
        
}