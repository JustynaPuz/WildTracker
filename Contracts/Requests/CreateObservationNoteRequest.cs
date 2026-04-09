namespace WildTracker.Contracts.Requests;

public class CreateObservationNoteRequest
{
    public Guid SightingReportId { get; init; }
    public string Content { get; init; } = default!;
}
