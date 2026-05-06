using WildTracker.Contracts.Common;

namespace WildTracker.Contracts.DTOs;

public record ObservationNoteDto
{
    public Guid Id { get; init; }
    public Guid SightingReportId { get; init; }
    public Guid AuthorUserId { get; init; }
    public string Content { get; init; } = default!;
    public DateTime CreatedAtUtc { get; init; }
    public DateTime? UpdatedAtUtc { get; init; }
    public IReadOnlyList<Link> Links { get; init; } = [];
}
