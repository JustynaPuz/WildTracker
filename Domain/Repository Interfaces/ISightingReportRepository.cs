using WildTracker.Domain.Entities;
using WildTracker.Domain.Queries;

namespace WildTracker.Domain.Repositories;

public interface ISightingReportRepository
{
    Task<SightingReport?> GetByIdAsync(Guid id);
    Task<(IReadOnlyList<SightingReport> Items, int TotalCount)> SearchAsync(SightingReportFilter filter);
    Task AddAsync(SightingReport entity);
    Task UpdateAsync(SightingReport entity);
    Task DeleteAsync(SightingReport entity);
}
