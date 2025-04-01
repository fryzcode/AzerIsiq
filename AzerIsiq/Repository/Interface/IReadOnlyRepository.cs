using System.Linq.Expressions;
using AzerIsiq.Dtos;

namespace AzerIsiq.Repository.Interface;

public interface IReadOnlyRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes);
    Task<PagedResultDto<T>> GetPagedAsync(int page, int pageSize);
    Task<PagedResultDto<T>> GetPageAsync(int page, int pageSize, Expression<Func<T, bool>>? filter = null);
    // Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
}
