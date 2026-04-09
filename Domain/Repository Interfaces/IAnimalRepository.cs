using WildTracker.Domain.Entities;

namespace WildTracker.Domain.Repositories;

public interface IAnimalRepository
{
    Task<Animal?> GetByIdAsync(Guid id);
    Task<IEnumerable<Animal>> GetAllAsync();
    Task AddAsync(Animal entity);
    Task UpdateAsync(Animal entity);
    Task DeleteAsync(Animal entity);
}
