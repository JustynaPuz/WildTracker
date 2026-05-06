using WildTracker.Contracts.DTOs;

namespace WildTracker.Application.Interfaces;

public interface IStatsService
{
    Task<StatsSummaryDto> GetSummaryAsync();
    Task<IReadOnlyList<SightingsBySpeciesDto>> GetBySpeciesAsync();
    Task<IReadOnlyList<SightingsByMonthDto>> GetByMonthAsync(int months = 12);
}
