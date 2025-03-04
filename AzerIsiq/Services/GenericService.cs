using AzerIsiq.Repository.Interface;

namespace AzerIsiq.Services;

public class GenericService<T> where T : class
{
    private readonly IGenericRepository<T> _repository;
    public GenericService(IGenericRepository<T> repository)
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
