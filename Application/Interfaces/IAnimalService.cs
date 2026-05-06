using WildTracker.Contracts.Common;
using WildTracker.Contracts.DTOs;
using WildTracker.Contracts.Requests;

namespace WildTracker.Application.Interfaces;

public interface IAnimalService
{
    Task<AnimalDto> GetByIdAsync(Guid id);
    Task<IReadOnlyList<AnimalDto>> GetAllAsync();
    Task<PagedResult<AnimalDto>> SearchAsync(AnimalSearchRequest request);
    Task<IReadOnlyList<MovementPointDto>> GetMovementAsync(Guid id, int limit);
    Task<AnimalDto> CreateAsync(CreateAnimalRequest request);
    Task<AnimalDto> UpdateAsync(Guid id, UpdateAnimalRequest request);
    Task DeleteAsync(Guid id);
}
