using WildTracker.Application.Exceptions;
using WildTracker.Application.Interfaces;
using WildTracker.Application.Mappers;
using WildTracker.Contracts.DTOs;
using WildTracker.Contracts.Requests;
using WildTracker.Domain.Repositories;

namespace WildTracker.Application.Services;

public class AnimalService : IAnimalService
{
    private readonly IAnimalRepository _repo;

    public AnimalService(IAnimalRepository repo)
    {
        _repo = repo;
    }

    public async Task<AnimalDto> GetByIdAsync(Guid id)
    {
        var entity = await _repo.GetByIdAsync(id)
            ?? throw new NotFoundException($"Animal with id '{id}' was not found.");

        return AnimalMapper.ToDto(entity);
    }

    public async Task<IEnumerable<AnimalDto>> GetAllAsync()
    {
        var animals = await _repo.GetAllAsync();
        return animals.Select(AnimalMapper.ToDto);
    }

    public async Task<AnimalDto> CreateAsync(CreateAnimalRequest request)
    {
        var entity = AnimalMapper.ToEntity(request);
        await _repo.AddAsync(entity);
        return AnimalMapper.ToDto(entity);
    }

    public async Task<AnimalDto> UpdateAsync(Guid id, UpdateAnimalRequest request)
    {
        var entity = await _repo.GetByIdAsync(id)
            ?? throw new NotFoundException($"Animal with id '{id}' was not found.");

        entity.UpdateDetails(request.Name, request.Species, request.HealthStatus, request.Description);
        await _repo.UpdateAsync(entity);
        return AnimalMapper.ToDto(entity);
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _repo.GetByIdAsync(id)
            ?? throw new NotFoundException($"Animal with id '{id}' was not found.");

        await _repo.DeleteAsync(entity);
    }
}
