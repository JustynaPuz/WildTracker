using WildTracker.Domain.Enums;
using WildTracker.Domain.Queries;

namespace WildTracker.Contracts.Requests;

public class SightingReportSearchRequest
{
    public Guid? AnimalId { get; init; }
    public ReportStatus? Status { get; init; }
    public DateTime? From { get; init; }
    public DateTime? To { get; init; }
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;

    public SightingReportFilter ToFilter() => new()
    {
        AnimalId = AnimalId,
        Status = Status,
        From = From,
        To = To,
        Page = Page,
        PageSize = PageSize,
    };
}
