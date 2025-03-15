using AzerIsiq.Repository.Interface;
using AzerIsiq.Services.ILogic;

namespace AzerIsiq.Services;

public class ReadOnlyService<T> : IReadOnlyService<T> where T : class
{
    private readonly IReadOnlyRepository<T> _repository;

    public ReadOnlyService(IReadOnlyRepository<T> repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        if (id <= 0) throw new ArgumentException("ID must be greater than zero.", nameof(id));
        return await _repository.GetByIdAsync(id);
    }
}