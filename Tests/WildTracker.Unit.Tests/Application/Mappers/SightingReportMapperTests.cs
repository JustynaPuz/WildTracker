using NUnit.Framework;
using WildTracker.Application.Mappers;
using WildTracker.Contracts.Requests;
using WildTracker.Domain.Enums;
using WildTracker.Domain.ValueObjects;
using WildTracker.Unit.Tests.Builders;

namespace WildTracker.Unit.Tests.Application.Mappers;

[TestFixture]
public class SightingReportMapperTests
{

    [Test]
    public void ToDto_MapsAllProperties()
    {
        var location = new LocationDetails(new Coordinates(52.0, 19.0), region: "Mazury");
        var report   = new SightingReportBuilder()
            .WithLocation(location)
            .WithReportType(ReportType.Sighting)
            .WithSource(SightingSource.Manual)
            .Build();

        var result = SightingReportMapper.ToDto(report);

        Assert.Multiple(() =>
        {
            Assert.That(result.Id,               Is.EqualTo(report.Id));
            Assert.That(result.AnimalId,         Is.EqualTo(report.AnimalId));
            Assert.That(result.ReportedByUserId, Is.EqualTo(report.ReportedByUserId));
            Assert.That(result.ReportType,       Is.EqualTo(ReportType.Sighting));
            Assert.That(result.Status,           Is.EqualTo(ReportStatus.Pending));
            Assert.That(result.Source,           Is.EqualTo(SightingSource.Manual));
            Assert.That(result.Location.Latitude,  Is.EqualTo(52.0));
            Assert.That(result.Location.Longitude, Is.EqualTo(19.0));
            Assert.That(result.Location.Region,    Is.EqualTo("Mazury"));
        });
    }

    [Test]
    public void ToMovementPointDto_MapsAllProperties()
    {
        var observedAt = new DateTime(2025, 6, 1, 12, 0, 0, DateTimeKind.Utc);
        var location   = new LocationDetails(new Coordinates(52.0, 19.0), region: "Mazury", forestDistrict: "FD-01");
        var report     = new SightingReportBuilder()
            .WithObservedAt(observedAt)
            .WithLocation(location)
            .WithReportType(ReportType.Sighting)
            .Build();

        var result = SightingReportMapper.ToMovementPointDto(report);

        Assert.Multiple(() =>
        {
            Assert.That(result.ReportId,      Is.EqualTo(report.Id));
            Assert.That(result.ObservedAtUtc, Is.EqualTo(observedAt));
            Assert.That(result.Latitude,      Is.EqualTo(52.0));
            Assert.That(result.Longitude,     Is.EqualTo(19.0));
            Assert.That(result.Region,        Is.EqualTo("Mazury"));
            Assert.That(result.ForestDistrict, Is.EqualTo("FD-01"));
            Assert.That(result.ReportType,    Is.EqualTo(ReportType.Sighting));
            Assert.That(result.Status,        Is.EqualTo(ReportStatus.Pending));
        });
    }

    [Test]
    public void ToEntity_MapsAllProperties()
    {
        var animalId   = Guid.NewGuid();
        var userId     = Guid.NewGuid();
        var observedAt = new DateTime(2025, 6, 1, 12, 0, 0, DateTimeKind.Utc);
        var request    = new CreateSightingReportRequest
        {
            AnimalId      = animalId,
            ObservedAtUtc = observedAt,
            ReportType    = ReportType.Sighting,
            Source        = SightingSource.Manual,
            Latitude      = 52.0,
            Longitude     = 19.0,
            Region        = "Mazury"
        };

        var result = SightingReportMapper.ToEntity(request, userId);

        Assert.Multiple(() =>
        {
            Assert.That(result.AnimalId,         Is.EqualTo(animalId));
            Assert.That(result.ReportedByUserId, Is.EqualTo(userId));
            Assert.That(result.ObservedAtUtc,    Is.EqualTo(observedAt));
            Assert.That(result.ReportType,       Is.EqualTo(ReportType.Sighting));
            Assert.That(result.Source,           Is.EqualTo(SightingSource.Manual));
            Assert.That(result.Status,           Is.EqualTo(ReportStatus.Pending));
            Assert.That(result.Location.Coordinates.Latitude,  Is.EqualTo(52.0));
            Assert.That(result.Location.Coordinates.Longitude, Is.EqualTo(19.0));
            Assert.That(result.Location.Region,  Is.EqualTo("Mazury"));
        });
    }
}
