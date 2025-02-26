using System.Linq.Expressions;
using AzerIsiq.Dtos;

namespace AzerIsiq.Repository.Interface;

public interface IReadOnlyRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task<PagedResultDto<T>> GetPagedAsync(int page, int pageSize);
    // public Task<IEnumerable<T>> FilterAsync(Expression<Func<T, bool>> predicate);
}
