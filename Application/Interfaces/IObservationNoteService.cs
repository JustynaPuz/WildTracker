using WildTracker.Contracts.DTOs;

namespace WildTracker.Application.Interfaces;

public interface IObservationNoteService
{
    Task<IReadOnlyList<ObservationNoteDto>> GetByReportIdAsync(Guid reportId);
    Task<ObservationNoteDto> CreateAsync(Guid reportId, string content, Guid authorUserId);
    Task<ObservationNoteDto> UpdateAsync(Guid reportId, Guid noteId, string content);
    Task DeleteAsync(Guid reportId, Guid noteId);
}
