using Microsoft.EntityFrameworkCore;
using WildTracker.Domain.Enums;
using WildTracker.Domain.Repositories;

namespace WildTracker.Infrastructure.Persistence.Repositories;

public class StatsRepository : IStatsRepository
{
    private readonly AppDbContext _context;

    public StatsRepository(AppDbContext context) => _context = context;

    public Task<int> CountAnimalsAsync()
        => _context.Animals.CountAsync();

    public Task<int> CountUsersAsync()
        => _context.Users.CountAsync();

    public Task<int> CountNotesAsync()
        => _context.ObservationNotes.CountAsync();

    public Task<int> CountReportsByStatusAsync(ReportStatus status)
        => _context.SightingReports.CountAsync(r => r.Status == status);

    public async Task<IReadOnlyList<(string Species, int Count)>> GetReportsBySpeciesAsync()
    {
        var rows = await _context.SightingReports
            .AsNoTracking()
            .Join(_context.Animals,
                  r => r.AnimalId,
                  a => a.Id,
                  (_, a) => a.Species)
            .GroupBy(s => s)
            .Select(g => new { Species = g.Key.ToString(), Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .ToListAsync();

        return rows.Select(x => (x.Species, x.Count)).ToList();
    }

    public async Task<IReadOnlyList<(int Year, int Month, int Count)>> GetReportsByMonthAsync(int months)
    {
        var since = DateTime.UtcNow.AddMonths(-months);

        var rows = await _context.SightingReports
            .AsNoTracking()
            .Where(r => r.ObservedAtUtc >= since)
            .GroupBy(r => new { r.ObservedAtUtc.Year, r.ObservedAtUtc.Month })
            .Select(g => new { g.Key.Year, g.Key.Month, Count = g.Count() })
            .OrderBy(x => x.Year).ThenBy(x => x.Month)
            .ToListAsync();

        return rows.Select(x => (x.Year, x.Month, x.Count)).ToList();
    }
}
