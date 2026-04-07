namespace DTOS
{
    // post operation
    public class CreateTodoRequest
    {
        public string Description { get; set; } = string.Empty;
        public TodoScope Scope { get; set; }
        public DateTime? DueAt { get; set; }

        //note for future: don't forget to check whether Todo Status is set to default value (which is 0) and if it is, set it to TodoStatus.Pending
    }

    public class CreateTodoResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public int? TodoId { get; set; }
    }

    //get operation
    public class GetAllTodosResponse
    {
        public List<Todo> Todos { get; set; } = new List<Todo>();
    }

    public class GetTodoReqest
    {
        public int TodoId { get; set; }
    }
    public class GetTodoResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public Todo? Todo { get; set; }
    }

    //put operation
    public class UpdateTodoRequest
    {
        public int TodoId { get; set; }
        public string? Description { get; set; }
        public TodoScope? Scope { get; set; }
        public DateTime? DueAt { get; set; }
    }

    public class UpdateTodoResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
    }

    //delete operation
    public class DeleteTodoRequest
    {
        public int TodoId { get; set; }
    }
    
    public class DeleteTodoResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
    }
        
}