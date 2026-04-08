public interface IRepository<T>
{
    InternalResponse<T> GetById(int id);
    InternalResponse<List<T>> GetAll();
    InternalResponse<T> Add(T entity);
    InternalResponse<T> Update(T entity);
    InternalResponse<T> Delete(int id);
}

