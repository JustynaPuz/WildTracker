using WildTracker.Contracts.DTOs;
using WildTracker.Contracts.Requests;
using WildTracker.Domain.Entities;
using WildTracker.Domain.ValueObjects;

namespace WildTracker.Application.Mappers;

public static class SightingReportMapper
{
    public static SightingReportDto ToDto(SightingReport entity) => new()
    {
        Id               = entity.Id,
        AnimalId         = entity.AnimalId,
        ReportedByUserId = entity.ReportedByUserId,
        ObservedAtUtc    = entity.ObservedAtUtc,
        ReportType       = entity.ReportType,
        Status           = entity.Status,
        Source           = entity.Source,
        Description      = entity.Description,
        CreatedAtUtc     = entity.CreatedAtUtc,
        Location = new LocationDetailsDto
        {
            Latitude       = entity.Location.Coordinates.Latitude,
            Longitude      = entity.Location.Coordinates.Longitude,
            Region         = entity.Location.Region,
            ForestDistrict = entity.Location.ForestDistrict,
        },
    };

    public static MovementPointDto ToMovementPointDto(SightingReport entity) => new()
    {
        ReportId       = entity.Id,
        ObservedAtUtc  = entity.ObservedAtUtc,
        Latitude       = entity.Location.Coordinates.Latitude,
        Longitude      = entity.Location.Coordinates.Longitude,
        Region         = entity.Location.Region,
        ForestDistrict = entity.Location.ForestDistrict,
        ReportType     = entity.ReportType,
        Status         = entity.Status,
    };

    public static SightingReport ToEntity(CreateSightingReportRequest request, Guid reportedByUserId) => new(
        request.AnimalId,
        reportedByUserId,
        request.ObservedAtUtc,
        request.ReportType,
        request.Source,
        new LocationDetails(
            new Coordinates(request.Latitude, request.Longitude),
            request.Region,
            request.ForestDistrict),
        request.Description
    );
}
