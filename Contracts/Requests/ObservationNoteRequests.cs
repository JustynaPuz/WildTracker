namespace WildTracker.Contracts.Requests;

public class CreateObservationNoteRequest
{
    public string Content { get; init; } = default!;
}

public class UpdateObservationNoteRequest
{
    public string Content { get; init; } = default!;
}
