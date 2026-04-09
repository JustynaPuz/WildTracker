using WildTracker.Application.Exceptions;
using WildTracker.Application.Interfaces;
using WildTracker.Application.Mappers;
using WildTracker.Contracts.Common;
using WildTracker.Contracts.DTOs;
using WildTracker.Contracts.Requests;
using WildTracker.Domain.Repositories;
using WildTracker.Domain.ValueObjects;

namespace WildTracker.Application.Services;

public class SightingReportService : ISightingReportService
{
    private readonly ISightingReportRepository _repo;

    public SightingReportService(ISightingReportRepository repo)
    {
        _repo = repo;
    }

    public async Task<SightingReportDto> GetByIdAsync(Guid id)
    {
        var entity = await _repo.GetByIdAsync(id)
            ?? throw new NotFoundException($"Sighting report with id '{id}' was not found.");

        return SightingReportMapper.ToDto(entity);
    }

    public async Task<PagedResult<SightingReportDto>> SearchAsync(SightingReportSearchRequest request)
    {
        var (items, total) = await _repo.SearchAsync(request.ToFilter());

        return new PagedResult<SightingReportDto>
        {
            Items = items.Select(SightingReportMapper.ToDto),
            TotalCount = total,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }

    public async Task<SightingReportDto> CreateAsync(CreateSightingReportRequest request, Guid reportedByUserId)
    {
        var entity = SightingReportMapper.ToEntity(request, reportedByUserId);
        await _repo.AddAsync(entity);
        return SightingReportMapper.ToDto(entity);
    }

    public async Task<SightingReportDto> UpdateAsync(Guid id, UpdateSightingReportRequest request)
    {
        var entity = await _repo.GetByIdAsync(id)
            ?? throw new NotFoundException($"Sighting report with id '{id}' was not found.");

        entity.UpdateObservation(
            request.ObservedAtUtc,
            new LocationDetails(
                new Coordinates(request.Latitude, request.Longitude),
                request.Region,
                request.ForestDistrict
                // location has no separate description field in the request
            ),
            request.Description,   // this is the report-level description only
            request.ReportType,
            request.Source
        );

        await _repo.UpdateAsync(entity);
        return SightingReportMapper.ToDto(entity);
    }

    public async Task ApproveAsync(Guid id)
    {
        var entity = await _repo.GetByIdAsync(id)
            ?? throw new NotFoundException($"Sighting report with id '{id}' was not found.");

        entity.Approve();
        await _repo.UpdateAsync(entity);
    }

    public async Task RejectAsync(Guid id)
    {
        var entity = await _repo.GetByIdAsync(id)
            ?? throw new NotFoundException($"Sighting report with id '{id}' was not found.");

        entity.Reject();
        await _repo.UpdateAsync(entity);
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _repo.GetByIdAsync(id)
            ?? throw new NotFoundException($"Sighting report with id '{id}' was not found.");

        await _repo.DeleteAsync(entity);
    }
}
