using WildTracker.Contracts.DTOs;

namespace WildTracker.Application.Interfaces;

public interface IObservationNoteService
{
    Task<IEnumerable<ObservationNoteDto>> GetByReportIdAsync(Guid reportId);
    Task<ObservationNoteDto> CreateAsync(Guid reportId, string content, Guid authorUserId);
}
