using WildTracker.Domain.Entities;
using WildTracker.Domain.Queries;

namespace WildTracker.Domain.Repositories;

public interface IAnimalRepository
{
    Task<Animal?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<Animal>> GetAllAsync();
    Task<(IReadOnlyList<Animal> Items, int TotalCount)> SearchAsync(AnimalFilter filter);
    Task AddAsync(Animal entity);
    Task UpdateAsync(Animal entity);
    Task DeleteAsync(Animal entity);
}
