namespace WildTracker.Contracts.DTOs;

public class ObservationNoteDto
{
    public Guid Id { get; init; }
    public Guid SightingReportId { get; init; }
    public Guid AuthorUserId { get; init; }
    public string Content { get; init; } = default!;
    public DateTime CreatedAtUtc { get; init; }
}
