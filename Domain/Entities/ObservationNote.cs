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
        if (sightingReportId == Guid.Empty)
            throw new ArgumentException("SightingReportId cannot be empty.", nameof(sightingReportId));

        if (authorUserId == Guid.Empty)
            throw new ArgumentException("AuthorUserId cannot be empty.", nameof(authorUserId));

        SightingReportId = sightingReportId;
        AuthorUserId     = authorUserId;
        Content          = DomainGuard.RequiredString(content, nameof(content), 1000);
    }

    public void UpdateContent(string content)
    {
        Content = DomainGuard.RequiredString(content, nameof(content), 1000);
        MarkAsUpdated();
    }
}
