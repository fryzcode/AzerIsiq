namespace AzerIsiq.Services.ILogic;

public interface IReadOnlyService<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
}