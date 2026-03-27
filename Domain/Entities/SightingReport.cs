using WildTracker.Domain.Common;
using WildTracker.Domain.Enums;
using WildTracker.Domain.ValueObjects;

namespace WildTracker.Domain.Entities;

public class SightingReport : AuditableEntity
{
    public Guid AnimalId { get; private set; }
    public Guid ReportedByUserId { get; private set; }
    public DateTime ObservedAtUtc { get; private set; }
    public ReportType ReportType { get; private set; }
    public ReportStatus Status { get; private set; }
    public SightingSource Source { get; private set; }
    public LocationDetails Location { get; private set; }
    public string? Description { get; private set; }

    private readonly List<ObservationNote> _notes = new();
    public IReadOnlyCollection<ObservationNote> Notes => _notes.AsReadOnly();

    private SightingReport()
    {
        Location = null!;
    }

    public SightingReport(
        Guid animalId,
        Guid reportedByUserId,
        DateTime observedAtUtc,
        ReportType reportType,
        SightingSource source,
        LocationDetails location,
        string? description = null)
    {
        if (animalId == Guid.Empty)
        {
            throw new ArgumentException("AnimalId cannot be empty.", nameof(animalId));
        }

        if (reportedByUserId == Guid.Empty)
        {
            throw new ArgumentException("ReportedByUserId cannot be empty.", nameof(reportedByUserId));
        }

        AnimalId = animalId;
        ReportedByUserId = reportedByUserId;
        ObservedAtUtc = observedAtUtc;
        ReportType = reportType;
        Source = source;
        Location = location ?? throw new ArgumentNullException(nameof(location));
        Description = Normalize(description, 2000);
        Status = ReportStatus.Pending;
    }

    public void UpdateObservation(DateTime observedAtUtc, LocationDetails location, string? description, ReportType reportType, SightingSource source)
    {
        ObservedAtUtc = observedAtUtc;
        Location = location ?? throw new ArgumentNullException(nameof(location));
        Description = Normalize(description, 2000);
        ReportType = reportType;
        Source = source;
        MarkAsUpdated();
    }

    public void Verify()
    {
        Status = ReportStatus.Verified;
        MarkAsUpdated();
    }

    public void Reject()
    {
        Status = ReportStatus.Rejected;
        MarkAsUpdated();
    }

    public void Resolve()
    {
        Status = ReportStatus.Resolved;
        MarkAsUpdated();
    }

    public ObservationNote AddNote(Guid authorUserId, string content)
    {
        var note = new ObservationNote(Id, authorUserId, content);
        _notes.Add(note);
        MarkAsUpdated();
        return note;
    }

    private static string? Normalize(string? value, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        var normalized = value.Trim();

        if (normalized.Length > maxLength)
        {
            throw new ArgumentException($"Value cannot exceed {maxLength} characters.");
        }

        return normalized;
    }
}