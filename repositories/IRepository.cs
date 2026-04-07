public interface IRepository<T>
{
    RespositoryResponse<T> GetById(int id);
    RespositoryResponse<List<T>> GetAll();
    RespositoryResponse<T> Add(T entity);
    RespositoryResponse<T> Update(T entity);
    RespositoryResponse<T> Delete(int id);
}

public class RespositoryResponse<T>
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }
}