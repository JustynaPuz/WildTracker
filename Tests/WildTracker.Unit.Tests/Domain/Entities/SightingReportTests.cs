using NUnit.Framework;
using WildTracker.Domain.Entities;
using WildTracker.Domain.Enums;
using WildTracker.Domain.ValueObjects;
using WildTracker.Unit.Tests.Builders;

namespace WildTracker.Unit.Tests.Domain.Entities;

[TestFixture]
public class SightingReportTests
{
    private const int MaxDescriptionLength = 2000;

    [Test]
    public void Constructor_WithValidData_SetsStatusToPending()
    {
        var report = new SightingReportBuilder().Build();

        Assert.That(report.Status, Is.EqualTo(ReportStatus.Pending));
    }

    [Test]
    public void Constructor_WithEmptyAnimalId_Throws()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
            new SightingReportBuilder().WithAnimalId(Guid.Empty).Build());

        Assert.That(ex!.Message, Does.Contain("Value cannot be empty."));
    }

    [Test]
    public void Constructor_WithEmptyReportedByUserId_Throws()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
            new SightingReportBuilder().WithUserId(Guid.Empty).Build());

        Assert.That(ex!.Message, Does.Contain("Value cannot be empty."));
    }

    [Test]
    public void Constructor_WithNullLocation_Throws()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new SightingReportBuilder().WithLocation(null!).Build());
    }

    [Test]
    public void Constructor_WithDescriptionExceeding2000Chars_Throws()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
            new SightingReportBuilder().WithDescription(new string('x', MaxDescriptionLength + 1)).Build());

        Assert.That(ex!.Message, Does.Contain($"Value cannot exceed {MaxDescriptionLength} characters."));
    }

    [Test]
    public void Approve_WhenPending_SetsStatusToVerified()
    {
        var report = new SightingReportBuilder().Build();

        report.Approve();

        Assert.That(report.Status, Is.EqualTo(ReportStatus.Verified));
    }

    [TestCase(ReportStatus.Rejected)]
    [TestCase(ReportStatus.Verified)]
    [TestCase(ReportStatus.Resolved)]
    public void Approve_WhenNotPending_Throws(ReportStatus initialStatus)
    {
        var report = InState(initialStatus);

        var ex = Assert.Throws<InvalidOperationException>(() => report.Approve());

        Assert.That(ex!.Message, Does.Contain("Cannot approve a report"));
    }

    [Test]
    public void Reject_WhenPending_SetsStatusToRejected()
    {
        var report = new SightingReportBuilder().Build();

        report.Reject();

        Assert.That(report.Status, Is.EqualTo(ReportStatus.Rejected));
    }

    [TestCase(ReportStatus.Verified)]
    [TestCase(ReportStatus.Rejected)]
    [TestCase(ReportStatus.Resolved)]
    public void Reject_WhenNotPending_Throws(ReportStatus initialStatus)
    {
        var report = InState(initialStatus);

        var ex = Assert.Throws<InvalidOperationException>(() => report.Reject());

        Assert.That(ex!.Message, Does.Contain("Cannot reject a report"));
    }

    [Test]
    public void Resolve_WhenVerified_SetsStatusToResolved()
    {
        var report = InState(ReportStatus.Verified);

        report.Resolve();

        Assert.That(report.Status, Is.EqualTo(ReportStatus.Resolved));
    }

    [TestCase(ReportStatus.Pending)]
    [TestCase(ReportStatus.Rejected)]
    [TestCase(ReportStatus.Resolved)]
    public void Resolve_WhenNotVerified_Throws(ReportStatus initialStatus)
    {
        var report = InState(initialStatus);

        var ex = Assert.Throws<InvalidOperationException>(() => report.Resolve());

        Assert.That(ex!.Message, Does.Contain("Cannot resolve a report"));
    }

    [Test]
    public void UpdateObservation_WithValidData_UpdatesProperties()
    {
        var report = new SightingReportBuilder().Build();
        var newLocation = new LocationDetails(new Coordinates(48.0, 21.0), region: "Mazury");
        var newTime = DateTime.UtcNow.AddHours(-1);

        report.UpdateObservation(newTime, newLocation, "updated desc", ReportType.Death, SightingSource.Drone);

        Assert.Multiple(() =>
        {
            Assert.That(report.Location, Is.EqualTo(newLocation));
            Assert.That(report.ObservedAtUtc, Is.EqualTo(newTime));
            Assert.That(report.Description, Is.EqualTo("updated desc"));
            Assert.That(report.ReportType, Is.EqualTo(ReportType.Death));
            Assert.That(report.Source, Is.EqualTo(SightingSource.Drone));
        });
    }

    [Test]
    public void UpdateObservation_WithNullLocation_Throws()
    {
        var report = new SightingReportBuilder().Build();

        Assert.Throws<ArgumentNullException>(() =>
            report.UpdateObservation(DateTime.UtcNow, null!, null, ReportType.Sighting, SightingSource.Manual));
    }

    private static SightingReport InState(ReportStatus status)
    {
        var report = new SightingReportBuilder().Build();
        switch (status)
        {
            case ReportStatus.Rejected: report.Reject(); break;
            case ReportStatus.Verified: report.Approve(); break;
            case ReportStatus.Resolved:
                report.Approve();
                report.Resolve();
                break;
        }

        return report;
    }
}
