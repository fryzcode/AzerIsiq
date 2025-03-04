using AzerIsiq.Repository.Interface;

namespace AzerIsiq.Services;

public class ReadOnlyService<T> where T : class
{
    private readonly IReadOnlyRepository<T> _repository;
    public ReadOnlyService(IReadOnlyRepository<T> repository)
    {
        _repository = repository;
    }
    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }
    public async Task<T?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }
}