using Xunit;
using WildTracker.Domain.Entities;
using WildTracker.Domain.Enums;
using WildTracker.Domain.ValueObjects;

namespace WildTracker.Tests.Domain;

public class SightingReportStatusTests
{
    [Fact]
    public void Approve_WhenRejected_Throws()
    {
        var report = new SightingReport(
            animalId:         Guid.NewGuid(),
            reportedByUserId: Guid.NewGuid(),
            observedAtUtc:    DateTime.UtcNow,
            reportType:       ReportType.Sighting,
            source:           SightingSource.Manual,
            location:         new LocationDetails(new Coordinates(52.0, 19.0)));

        report.Reject();

        Assert.Throws<InvalidOperationException>(() => report.Approve());
    }
}
