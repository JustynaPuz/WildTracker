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
        {
            throw new ArgumentException("SightingReportId cannot be empty.", nameof(sightingReportId));
        }

        if (authorUserId == Guid.Empty)
        {
            throw new ArgumentException("AuthorUserId cannot be empty.", nameof(authorUserId));
        }

        SightingReportId = sightingReportId;
        AuthorUserId = authorUserId;
        Content = ValidateContent(content);
    }

    public void UpdateContent(string content)
    {
        Content = ValidateContent(content);
        MarkAsUpdated();
    }

    private static string ValidateContent(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            throw new ArgumentException("Content cannot be empty.", nameof(content));
        }

        var normalized = content.Trim();

        if (normalized.Length > 1000)
        {
            throw new ArgumentException("Content cannot exceed 1000 characters.", nameof(content));
        }

        return normalized;
    }
}