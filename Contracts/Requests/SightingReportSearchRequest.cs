using WildTracker.Domain.Enums;

namespace WildTracker.Contracts.Requests;

/// <summary>
/// Query parameters for the sighting report search endpoint.
/// Mapping to the domain filter is intentionally kept in the Application layer
/// (SightingReportService) to avoid a Contracts → Domain.Queries dependency.
/// </summary>
public record SightingReportSearchRequest
{
    public Guid? AnimalId { get; init; }
    public Guid? ReportedByUserId { get; init; }
    public ReportStatus? Status { get; init; }
    public DateTime? From { get; init; }
    public DateTime? To { get; init; }
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;
}
