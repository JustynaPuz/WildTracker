using WildTracker.Contracts.Common;
using WildTracker.Domain.Enums;

namespace WildTracker.Contracts.DTOs;

public record MovementPointDto
{
    public Guid ReportId { get; init; }
    public DateTime ObservedAtUtc { get; init; }
    public double Latitude { get; init; }
    public double Longitude { get; init; }
    public string? Region { get; init; }
    public string? ForestDistrict { get; init; }
    public ReportType ReportType { get; init; }
    public ReportStatus Status { get; init; }
    public IReadOnlyList<Link> Links { get; init; } = [];
}
