namespace AzerIsiq.Repository.Interface;

public interface IGenericRepository<T> : IReadOnlyRepository<T> where T : class
{
    // IQueryable<T> GetAll();
    Task<T> CreateAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
}