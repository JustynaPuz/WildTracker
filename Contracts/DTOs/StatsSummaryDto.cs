using WildTracker.Contracts.Common;

namespace WildTracker.Contracts.DTOs;

public record StatsSummaryDto
{
    public int TotalAnimals { get; init; }
    public int TotalReports { get; init; }
    public int PendingReports { get; init; }
    public int VerifiedReports { get; init; }
    public int RejectedReports { get; init; }
    public int ResolvedReports { get; init; }
    public int TotalNotes { get; init; }
    public int TotalUsers { get; init; }
    public IReadOnlyList<Link> Links { get; init; } = [];
}
