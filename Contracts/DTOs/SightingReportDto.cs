using WildTracker.Contracts.Common;
using WildTracker.Domain.Enums;

namespace WildTracker.Contracts.DTOs;

public record SightingReportDto
{
    public Guid Id { get; init; }
    public Guid AnimalId { get; init; }
    public Guid ReportedByUserId { get; init; }
    public DateTime ObservedAtUtc { get; init; }
    public ReportType ReportType { get; init; }
    public ReportStatus Status { get; init; }
    public SightingSource Source { get; init; }
    public LocationDetailsDto Location { get; init; } = default!;
    public string? Description { get; init; }
    public DateTime CreatedAtUtc { get; init; }
    public IReadOnlyList<Link> Links { get; init; } = [];
}
