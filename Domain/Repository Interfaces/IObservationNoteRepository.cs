using WildTracker.Domain.Entities;

namespace WildTracker.Domain.Repositories;

public interface IObservationNoteRepository
{
    Task<IEnumerable<ObservationNote>> GetByReportIdAsync(Guid reportId);
    Task AddAsync(ObservationNote entity);
}
