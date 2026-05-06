using WildTracker.Application.Interfaces;
using WildTracker.Contracts.DTOs;
using WildTracker.Domain.Enums;
using WildTracker.Domain.Repositories;

namespace WildTracker.Application.Services;

public class StatsService : IStatsService
{
    private readonly IStatsRepository _repo;

    public StatsService(IStatsRepository repo) => _repo = repo;

    public async Task<StatsSummaryDto> GetSummaryAsync()
    {
        // Run all counts concurrently — independent queries
        var animals  = _repo.CountAnimalsAsync();
        var users    = _repo.CountUsersAsync();
        var notes    = _repo.CountNotesAsync();
        var pending  = _repo.CountReportsByStatusAsync(ReportStatus.Pending);
        var verified = _repo.CountReportsByStatusAsync(ReportStatus.Verified);
        var rejected = _repo.CountReportsByStatusAsync(ReportStatus.Rejected);
        var resolved = _repo.CountReportsByStatusAsync(ReportStatus.Resolved);

        await Task.WhenAll(animals, users, notes, pending, verified, rejected, resolved);

        return new StatsSummaryDto
        {
            TotalAnimals    = animals.Result,
            TotalUsers      = users.Result,
            TotalNotes      = notes.Result,
            PendingReports  = pending.Result,
            VerifiedReports = verified.Result,
            RejectedReports = rejected.Result,
            ResolvedReports = resolved.Result,
            TotalReports    = pending.Result + verified.Result + rejected.Result + resolved.Result,
        };
    }

    public async Task<IReadOnlyList<SightingsBySpeciesDto>> GetBySpeciesAsync()
    {
        var rows = await _repo.GetReportsBySpeciesAsync();
        return rows.Select(r => new SightingsBySpeciesDto { Species = r.Species, Count = r.Count })
                   .ToList();
    }

    public async Task<IReadOnlyList<SightingsByMonthDto>> GetByMonthAsync(int months = 12)
    {
        var rows = await _repo.GetReportsByMonthAsync(Math.Clamp(months, 1, 36));
        return rows.Select(r => new SightingsByMonthDto { Year = r.Year, Month = r.Month, Count = r.Count })
                   .ToList();
    }
}
