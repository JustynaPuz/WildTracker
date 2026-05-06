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

public record SightingsBySpeciesDto
{
    public string Species { get; init; } = default!;
    public int Count { get; init; }
}

public record SightingsByMonthDto
{
    public int Year { get; init; }
    public int Month { get; init; }
    public int Count { get; init; }
    /// <summary>ISO label, e.g. "2025-03"</summary>
    public string Label => $"{Year:0000}-{Month:00}";
}
