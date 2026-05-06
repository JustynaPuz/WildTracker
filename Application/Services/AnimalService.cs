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
    private readonly ISightingReportRepository _reportRepo;

    public AnimalService(IAnimalRepository repo, ISightingReportRepository reportRepo)
    {
        _repo = repo;
        _reportRepo = reportRepo;
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

    public async Task<IEnumerable<MovementPointDto>> GetMovementAsync(Guid id, int limit = 50)
    {
        _ = await _repo.GetByIdAsync(id)
            ?? throw new NotFoundException($"Animal with id '{id}' was not found.");

        var reports = await _reportRepo.GetMovementAsync(id, limit);
        return reports.Select(r => new MovementPointDto
        {
            ReportId        = r.Id,
            ObservedAtUtc   = r.ObservedAtUtc,
            Latitude        = r.Location.Coordinates.Latitude,
            Longitude       = r.Location.Coordinates.Longitude,
            Region          = r.Location.Region,
            ForestDistrict  = r.Location.ForestDistrict,
            ReportType      = r.ReportType,
            Status          = r.Status,
        });
    }
}
