using WildTracker.Domain.Entities;
using WildTracker.Domain.Queries;

namespace WildTracker.Domain.Repositories;

public interface ISightingReportRepository
{
    Task<SightingReport?> GetByIdAsync(Guid id);
    Task<(IEnumerable<SightingReport> Items, int TotalCount)> SearchAsync(SightingReportFilter filter);
    Task<IEnumerable<SightingReport>> GetMovementAsync(Guid animalId, int limit);
    Task AddAsync(SightingReport entity);
    Task UpdateAsync(SightingReport entity);
    Task DeleteAsync(SightingReport entity);
}
