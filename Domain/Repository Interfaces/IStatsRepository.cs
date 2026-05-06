using WildTracker.Domain.Enums;

namespace WildTracker.Domain.Repositories;

public interface IStatsRepository
{
    Task<int> CountAnimalsAsync();
    Task<int> CountUsersAsync();
    Task<int> CountNotesAsync();
    Task<int> CountReportsByStatusAsync(ReportStatus status);
    Task<IReadOnlyList<(string Species, int Count)>> GetReportsBySpeciesAsync();
    Task<IReadOnlyList<(int Year, int Month, int Count)>> GetReportsByMonthAsync(int months);
}
