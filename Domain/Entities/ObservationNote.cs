using WildTracker.Domain.Common;

namespace WildTracker.Domain.Entities;

public class ObservationNote : AuditableEntity
{
    public Guid SightingReportId { get; private set; }
    public Guid AuthorUserId { get; private set; }
    public string Content { get; private set; }

    private ObservationNote()
    {
        Content = string.Empty;
    }

    public ObservationNote(Guid sightingReportId, Guid authorUserId, string content)
    {
        SightingReportId = DomainGuard.RequiredGuid(sightingReportId, nameof(sightingReportId));
        AuthorUserId     = DomainGuard.RequiredGuid(authorUserId, nameof(authorUserId));
        Content          = DomainGuard.RequiredString(content, nameof(content), 1000);
    }

    public void UpdateContent(string content)
    {
        Content = DomainGuard.RequiredString(content, nameof(content), 1000);
        MarkAsUpdated();
    }
}
