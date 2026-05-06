using WildTracker.Contracts.DTOs;
using WildTracker.Contracts.Requests;

namespace WildTracker.Application.Interfaces;

public interface IAnimalService
{
    Task<AnimalDto> GetByIdAsync(Guid id);
    Task<IEnumerable<AnimalDto>> GetAllAsync();
    Task<AnimalDto> CreateAsync(CreateAnimalRequest request);
    Task<AnimalDto> UpdateAsync(Guid id, UpdateAnimalRequest request);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<MovementPointDto>> GetMovementAsync(Guid id, int limit = 50);
}
