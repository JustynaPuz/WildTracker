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
        // Sequential — EF Core's scoped DbContext does not support concurrent operations
        var totalAnimals = await _repo.CountAnimalsAsync();
        var totalUsers   = await _repo.CountUsersAsync();
        var totalNotes   = await _repo.CountNotesAsync();
        var pending      = await _repo.CountReportsByStatusAsync(ReportStatus.Pending);
        var verified     = await _repo.CountReportsByStatusAsync(ReportStatus.Verified);
        var rejected     = await _repo.CountReportsByStatusAsync(ReportStatus.Rejected);
        var resolved     = await _repo.CountReportsByStatusAsync(ReportStatus.Resolved);

        return new StatsSummaryDto
        {
            TotalAnimals    = totalAnimals,
            TotalUsers      = totalUsers,
            TotalNotes      = totalNotes,
            PendingReports  = pending,
            VerifiedReports = verified,
            RejectedReports = rejected,
            ResolvedReports = resolved,
            TotalReports    = pending + verified + rejected + resolved,
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
