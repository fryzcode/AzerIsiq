namespace AzerIsiq.Repository.Interface;

public interface IGenericRepository<T> : IReadOnlyRepository<T> where T : class
{
    Task<T> CreateAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
}