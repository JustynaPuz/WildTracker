using WildTracker.Domain.Enums;

namespace WildTracker.Domain.Queries;

public class SightingReportFilter
{
    public Guid? AnimalId { get; init; }
    public Species? Species { get; init; }
    public Guid? ReportedByUserId { get; init; }
    public ReportStatus? Status { get; init; }
    public DateTime? From { get; init; }
    public DateTime? To { get; init; }
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;
}
