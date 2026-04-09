using WildTracker.Contracts.DTOs;
using WildTracker.Contracts.Requests;

namespace WildTracker.Application.Interfaces;

public interface IObservationNoteService
{
    Task<IEnumerable<ObservationNoteDto>> GetByReportIdAsync(Guid reportId);
    Task<ObservationNoteDto> CreateAsync(CreateObservationNoteRequest request, Guid authorUserId);
}
