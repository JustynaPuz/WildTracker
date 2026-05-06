namespace WildTracker.Contracts.Requests;

public record UpdateObservationNoteRequest
{
    public string Content { get; init; } = default!;
}
