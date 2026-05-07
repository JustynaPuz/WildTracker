using WildTracker.Application.Exceptions;
using WildTracker.Application.Interfaces;
using WildTracker.Application.Mappers;
using WildTracker.Contracts.Common;
using WildTracker.Contracts.DTOs;
using WildTracker.Contracts.Requests;
using WildTracker.Domain.Entities;
using WildTracker.Domain.Queries;
using WildTracker.Domain.Repositories;
using WildTracker.Domain.ValueObjects;

namespace WildTracker.Application.Services;

public class SightingReportService : ISightingReportService
{
    private readonly ISightingReportRepository _reportRepository;

    public SightingReportService(ISightingReportRepository reportRepository)
    {
        _reportRepository = reportRepository;
    }

    public async Task<SightingReportDto> GetByIdAsync(Guid id)
    {
        var entity = await _reportRepository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Sighting report with id '{id}' was not found.");

        return SightingReportMapper.ToDto(entity);
    }

    public async Task<PagedResult<SightingReportDto>> SearchAsync(SightingReportSearchRequest request)
    {
        var (items, total) = await _reportRepository.SearchAsync(request.ToFilter());

        return new PagedResult<SightingReportDto>
        {
            Items      = items.Select(SightingReportMapper.ToDto).ToList(),
            TotalCount = total,
            Page       = request.Page,
            PageSize   = request.PageSize,
        };
    }

    public async Task<SightingReportDto> CreateAsync(CreateSightingReportRequest request, Guid reportedByUserId)
    {
        var entity = SightingReportMapper.ToEntity(request, reportedByUserId);
        await _reportRepository.AddAsync(entity);
        return SightingReportMapper.ToDto(entity);
    }

    public async Task<SightingReportDto> UpdateAsync(Guid id, UpdateSightingReportRequest request)
    {
        var entity = await _reportRepository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Sighting report with id '{id}' was not found.");

        entity.UpdateObservation(
            request.ObservedAtUtc,
            new LocationDetails(
                new Coordinates(request.Latitude, request.Longitude),
                request.Region,
                request.ForestDistrict),
            request.Description,
            request.ReportType,
            request.Source);

        await _reportRepository.UpdateAsync(entity);
        return SightingReportMapper.ToDto(entity);
    }

    public Task<SightingReportDto> ApproveAsync(Guid id) =>
        TransitionStatusAsync(id, r => r.Approve());

    public Task<SightingReportDto> RejectAsync(Guid id) =>
        TransitionStatusAsync(id, r => r.Reject());

    public Task<SightingReportDto> ResolveAsync(Guid id) =>
        TransitionStatusAsync(id, r => r.Resolve());

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _reportRepository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Sighting report with id '{id}' was not found.");

        await _reportRepository.DeleteAsync(entity);
    }

    private async Task<SightingReportDto> TransitionStatusAsync(Guid id, Action<SightingReport> transition)
    {
        var entity = await _reportRepository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Sighting report with id '{id}' was not found.");

        try { transition(entity); }
        catch (InvalidOperationException ex) { throw new ConflictException(ex.Message); }

        await _reportRepository.UpdateAsync(entity);
        return SightingReportMapper.ToDto(entity);
    }
}
