using WildTracker.Domain.Enums;

namespace WildTracker.Contracts.Requests;

public class CreateSightingReportRequest
{
    public Guid AnimalId { get; init; }
    public DateTime ObservedAtUtc { get; init; }
    public ReportType ReportType { get; init; }
    public SightingSource Source { get; init; }
    public double Latitude { get; init; }
    public double Longitude { get; init; }
    public string? Region { get; init; }
    public string? ForestDistrict { get; init; }
    public string? Description { get; init; }
}

public class UpdateSightingReportRequest
{
    public DateTime ObservedAtUtc { get; init; }
    public ReportType ReportType { get; init; }
    public SightingSource Source { get; init; }
    public double Latitude { get; init; }
    public double Longitude { get; init; }
    public string? Region { get; init; }
    public string? ForestDistrict { get; init; }
    public string? Description { get; init; }
}
