using WildTracker.Domain.Entities;

namespace WildTracker.Domain.Repositories;

public interface IObservationNoteRepository
{
    Task<ObservationNote?> GetByIdAsync(Guid id);
    Task<IEnumerable<ObservationNote>> GetByReportIdAsync(Guid reportId);
    Task AddAsync(ObservationNote entity);
    Task UpdateAsync(ObservationNote entity);
    Task DeleteAsync(ObservationNote entity);
}
