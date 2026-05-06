using WildTracker.Application.Exceptions;
using WildTracker.Application.Interfaces;
using WildTracker.Application.Mappers;
using WildTracker.Contracts.Common;
using WildTracker.Contracts.DTOs;
using WildTracker.Contracts.Requests;
using WildTracker.Domain.Queries;
using WildTracker.Domain.Repositories;

namespace WildTracker.Application.Services;

public class AnimalService : IAnimalService
{
    private readonly IAnimalRepository _animalRepository;
    private readonly ISightingReportRepository _reportRepository;

    public AnimalService(IAnimalRepository animalRepository, ISightingReportRepository reportRepository)
    {
        _animalRepository = animalRepository;
        _reportRepository = reportRepository;
    }

    public async Task<AnimalDto> GetByIdAsync(Guid id)
    {
        var entity = await _animalRepository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Animal with id '{id}' was not found.");

        return AnimalMapper.ToDto(entity);
    }

    public async Task<IReadOnlyList<AnimalDto>> GetAllAsync()
    {
        var animals = await _animalRepository.GetAllAsync();
        return animals.Select(AnimalMapper.ToDto).ToList();
    }

    public async Task<PagedResult<AnimalDto>> SearchAsync(AnimalSearchRequest request)
    {
        var (items, total) = await _animalRepository.SearchAsync(request.ToFilter());

        return new PagedResult<AnimalDto>
        {
            Items      = items.Select(AnimalMapper.ToDto).ToList(),
            TotalCount = total,
            Page       = request.Page,
            PageSize   = request.PageSize,
        };
    }

    public async Task<IReadOnlyList<MovementPointDto>> GetMovementAsync(Guid id, int limit)
    {
        _ = await _animalRepository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Animal with id '{id}' was not found.");

        var (reports, _) = await _reportRepository.SearchAsync(new SightingReportFilter
        {
            AnimalId = id,
            Page     = 1,
            PageSize = Math.Clamp(limit, 1, 500),
        });

        return reports.Select(SightingReportMapper.ToMovementPointDto).ToList();
    }

    public async Task<AnimalDto> CreateAsync(CreateAnimalRequest request)
    {
        var entity = AnimalMapper.ToEntity(request);
        await _animalRepository.AddAsync(entity);
        return AnimalMapper.ToDto(entity);
    }

    public async Task<AnimalDto> UpdateAsync(Guid id, UpdateAnimalRequest request)
    {
        var entity = await _animalRepository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Animal with id '{id}' was not found.");

        entity.UpdateDetails(request.Name, request.Species, request.HealthStatus, request.Description);
        await _animalRepository.UpdateAsync(entity);
        return AnimalMapper.ToDto(entity);
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _animalRepository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Animal with id '{id}' was not found.");

        await _animalRepository.DeleteAsync(entity);
    }
}
