using NUnit.Framework;
using WildTracker.Application.Validators;
using WildTracker.Contracts.Requests;
using WildTracker.Domain.Enums;

namespace WildTracker.Unit.Tests.Application.Validators;

[TestFixture]
public class SightingReportValidatorTests
{
    private CreateSightingReportValidator _createValidator = null!;
    private UpdateSightingReportValidator _updateValidator = null!;

    [SetUp]
    public void SetUp()
    {
        _createValidator = new CreateSightingReportValidator();
        _updateValidator = new UpdateSightingReportValidator();
    }

    [Test]
    public void CreateReport_WithValidRequest_PassesValidation()
    {
        var request = new CreateSightingReportRequest
        {
            AnimalId      = Guid.NewGuid(),
            ObservedAtUtc = DateTime.UtcNow.AddHours(-1),
            ReportType    = ReportType.Sighting,
            Source        = SightingSource.Manual,
            Latitude      = 52.0,
            Longitude     = 19.0
        };

        Assert.That(_createValidator.Validate(request).IsValid, Is.True);
    }

    [Test]
    public void CreateReport_WithEmptyAnimalId_FailsValidation()
    {
        var request = new CreateSightingReportRequest
        {
            AnimalId      = Guid.Empty,
            ObservedAtUtc = DateTime.UtcNow.AddHours(-1),
            ReportType    = ReportType.Sighting,
            Source        = SightingSource.Manual,
            Latitude      = 52.0,
            Longitude     = 19.0
        };
        var result = _createValidator.Validate(request);

        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.Select(e => e.PropertyName), Does.Contain(nameof(CreateSightingReportRequest.AnimalId)));
    }

    [Test]
    public void CreateReport_WithFutureObservedAt_FailsValidation()
    {
        var request = new CreateSightingReportRequest
        {
            AnimalId      = Guid.NewGuid(),
            ObservedAtUtc = DateTime.UtcNow.AddDays(1),
            ReportType    = ReportType.Sighting,
            Source        = SightingSource.Manual,
            Latitude      = 52.0,
            Longitude     = 19.0
        };
        var result = _createValidator.Validate(request);

        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.Select(e => e.PropertyName), Does.Contain(nameof(CreateSightingReportRequest.ObservedAtUtc)));
    }

    [TestCase(-91.0)]
    [TestCase(91.0)]
    public void CreateReport_WithLatitudeOutOfRange_FailsValidation(double latitude)
    {
        var request = new CreateSightingReportRequest
        {
            AnimalId      = Guid.NewGuid(),
            ObservedAtUtc = DateTime.UtcNow.AddHours(-1),
            ReportType    = ReportType.Sighting,
            Source        = SightingSource.Manual,
            Latitude      = latitude,
            Longitude     = 19.0
        };
        var result = _createValidator.Validate(request);

        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.Select(e => e.PropertyName), Does.Contain(nameof(CreateSightingReportRequest.Latitude)));
    }

    [TestCase(-181.0)]
    [TestCase(181.0)]
    public void CreateReport_WithLongitudeOutOfRange_FailsValidation(double longitude)
    {
        var request = new CreateSightingReportRequest
        {
            AnimalId      = Guid.NewGuid(),
            ObservedAtUtc = DateTime.UtcNow.AddHours(-1),
            ReportType    = ReportType.Sighting,
            Source        = SightingSource.Manual,
            Latitude      = 52.0,
            Longitude     = longitude
        };
        var result = _createValidator.Validate(request);

        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.Select(e => e.PropertyName), Does.Contain(nameof(CreateSightingReportRequest.Longitude)));
    }

    [Test]
    public void CreateReport_WithRegionExceedingMaxLength_FailsValidation()
    {
        var request = new CreateSightingReportRequest
        {
            AnimalId      = Guid.NewGuid(),
            ObservedAtUtc = DateTime.UtcNow.AddHours(-1),
            ReportType    = ReportType.Sighting,
            Source        = SightingSource.Manual,
            Latitude      = 52.0,
            Longitude     = 19.0,
            Region        = new string('a', 101)
        };
        var result = _createValidator.Validate(request);

        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.Select(e => e.PropertyName), Does.Contain(nameof(CreateSightingReportRequest.Region)));
    }

    [Test]
    public void CreateReport_WithDescriptionExceedingMaxLength_FailsValidation()
    {
        var request = new CreateSightingReportRequest
        {
            AnimalId      = Guid.NewGuid(),
            ObservedAtUtc = DateTime.UtcNow.AddHours(-1),
            ReportType    = ReportType.Sighting,
            Source        = SightingSource.Manual,
            Latitude      = 52.0,
            Longitude     = 19.0,
            Description   = new string('a', 2001)
        };
        var result = _createValidator.Validate(request);

        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.Select(e => e.PropertyName), Does.Contain(nameof(CreateSightingReportRequest.Description)));
    }

    [Test]
    public void UpdateReport_WithValidRequest_PassesValidation()
    {
        var request = new UpdateSightingReportRequest
        {
            ObservedAtUtc = DateTime.UtcNow.AddHours(-1),
            ReportType    = ReportType.Sighting,
            Source        = SightingSource.Manual,
            Latitude      = 52.0,
            Longitude     = 19.0
        };

        Assert.That(_updateValidator.Validate(request).IsValid, Is.True);
    }

    [Test]
    public void UpdateReport_WithFutureObservedAt_FailsValidation()
    {
        var request = new UpdateSightingReportRequest
        {
            ObservedAtUtc = DateTime.UtcNow.AddDays(1),
            ReportType    = ReportType.Sighting,
            Source        = SightingSource.Manual,
            Latitude      = 52.0,
            Longitude     = 19.0
        };
        var result = _updateValidator.Validate(request);

        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.Select(e => e.PropertyName), Does.Contain(nameof(UpdateSightingReportRequest.ObservedAtUtc)));
    }

    [TestCase(-91.0)]
    [TestCase(91.0)]
    public void UpdateReport_WithLatitudeOutOfRange_FailsValidation(double latitude)
    {
        var request = new UpdateSightingReportRequest
        {
            ObservedAtUtc = DateTime.UtcNow.AddHours(-1),
            ReportType    = ReportType.Sighting,
            Source        = SightingSource.Manual,
            Latitude      = latitude,
            Longitude     = 19.0
        };
        var result = _updateValidator.Validate(request);

        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.Select(e => e.PropertyName), Does.Contain(nameof(UpdateSightingReportRequest.Latitude)));
    }
}
