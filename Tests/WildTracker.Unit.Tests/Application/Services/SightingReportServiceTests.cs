using NUnit.Framework;
using NSubstitute;
using WildTracker.Application.Exceptions;
using WildTracker.Application.Services;
using WildTracker.Contracts.Requests;
using WildTracker.Domain.Entities;
using WildTracker.Domain.Enums;
using WildTracker.Domain.Queries;
using WildTracker.Domain.Repositories;
using WildTracker.Unit.Tests.Builders;

namespace WildTracker.Unit.Tests.Application.Services;

[TestFixture]
public class SightingReportServiceTests
{
    private ISightingReportRepository _reportRepo = null!;
    private SightingReportService _service = null!;

    [SetUp]
    public void SetUp()
    {
        _reportRepo = Substitute.For<ISightingReportRepository>();
        _service    = new SightingReportService(_reportRepo);
    }

    [Test]
    public async Task GetByIdAsync_WhenReportNotFound_ThrowsNotFoundException()
    {
        _reportRepo.GetByIdAsync(Arg.Any<Guid>()).Returns((SightingReport?)null);

        Assert.That(
            async () => await _service.GetByIdAsync(Guid.NewGuid()),
            Throws.TypeOf<NotFoundException>());
    }

    [Test]
    public async Task GetByIdAsync_WhenReportFound_ReturnsMappedDto()
    {
        var report = new SightingReportBuilder().Build();
        _reportRepo.GetByIdAsync(Arg.Any<Guid>()).Returns(report);

        var result = await _service.GetByIdAsync(report.Id);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(report.Id));
    }

    [Test]
    public async Task SearchAsync_ReturnsMappedPagedResult()
    {
        var report  = new SightingReportBuilder().Build();
        var request = new SightingReportSearchRequest { Page = 1, PageSize = 10 };
        _reportRepo.SearchAsync(Arg.Any<SightingReportFilter>())
                   .Returns((new List<SightingReport> { report }, 1));

        var result = await _service.SearchAsync(request);

        Assert.That(result.Items,      Has.Count.EqualTo(1));
        Assert.That(result.TotalCount, Is.EqualTo(1));
        Assert.That(result.Page,       Is.EqualTo(1));
        Assert.That(result.PageSize,   Is.EqualTo(10));
    }

    [Test]
    public async Task CreateAsync_CreatesReportAndReturnsDto()
    {
        var userId  = Guid.NewGuid();
        var request = new CreateSightingReportRequest
        {
            AnimalId      = Guid.NewGuid(),
            ObservedAtUtc = new DateTime(2025, 6, 1, 12, 0, 0, DateTimeKind.Utc),
            ReportType    = ReportType.Sighting,
            Source        = SightingSource.Manual,
            Latitude      = 52.0,
            Longitude     = 19.0
        };

        var result = await _service.CreateAsync(request, userId);

        await _reportRepo.Received(1).AddAsync(Arg.Any<SightingReport>());
        Assert.Multiple(() =>
        {
            Assert.That(result.ReportedByUserId, Is.EqualTo(userId));
            Assert.That(result.ReportType,       Is.EqualTo(ReportType.Sighting));
            Assert.That(result.Status,           Is.EqualTo(ReportStatus.Pending));
        });
    }

    [Test]
    public async Task UpdateAsync_WhenReportNotFound_ThrowsNotFoundException()
    {
        _reportRepo.GetByIdAsync(Arg.Any<Guid>()).Returns((SightingReport?)null);

        Assert.That(
            async () => await _service.UpdateAsync(Guid.NewGuid(), new UpdateSightingReportRequest
            {
                ObservedAtUtc = DateTime.UtcNow.AddDays(-1),
                ReportType    = ReportType.Sighting,
                Source        = SightingSource.Manual,
                Latitude      = 52.0,
                Longitude     = 19.0
            }),
            Throws.TypeOf<NotFoundException>());
    }

    [Test]
    public async Task UpdateAsync_WhenReportFound_UpdatesAndReturnsDto()
    {
        var report  = new SightingReportBuilder().Build();
        var request = new UpdateSightingReportRequest
        {
            ObservedAtUtc = new DateTime(2025, 6, 1, 12, 0, 0, DateTimeKind.Utc),
            ReportType    = ReportType.Death,
            Source        = SightingSource.Drone,
            Latitude      = 48.0,
            Longitude     = 21.0,
            Region        = "Tatry"
        };
        _reportRepo.GetByIdAsync(Arg.Any<Guid>()).Returns(report);

        var result = await _service.UpdateAsync(report.Id, request);

        await _reportRepo.Received(1).UpdateAsync(report);
        Assert.Multiple(() =>
        {
            Assert.That(result.Location.Latitude,  Is.EqualTo(48.0));
            Assert.That(result.Location.Longitude, Is.EqualTo(21.0));
            Assert.That(result.Location.Region,    Is.EqualTo("Tatry"));
            Assert.That(result.ReportType,         Is.EqualTo(ReportType.Death));
            Assert.That(result.Source,             Is.EqualTo(SightingSource.Drone));
        });
    }

    [Test]
    public async Task ApproveAsync_WhenReportNotFound_ThrowsNotFoundException()
    {
        _reportRepo.GetByIdAsync(Arg.Any<Guid>()).Returns((SightingReport?)null);

        Assert.That(
            async () => await _service.ApproveAsync(Guid.NewGuid()),
            Throws.TypeOf<NotFoundException>());
    }

    [Test]
    public async Task ApproveAsync_WhenReportIsPending_ApprovesAndReturnsDto()
    {
        var report = new SightingReportBuilder().Build();
        _reportRepo.GetByIdAsync(Arg.Any<Guid>()).Returns(report);

        var result = await _service.ApproveAsync(report.Id);

        await _reportRepo.Received(1).UpdateAsync(report);
        Assert.That(result.Status, Is.EqualTo(ReportStatus.Verified));
    }

    [Test]
    public async Task ApproveAsync_WhenReportIsNotPending_ThrowsConflictException()
    {
        var report = new SightingReportBuilder().Build();
        report.Reject();
        _reportRepo.GetByIdAsync(Arg.Any<Guid>()).Returns(report);

        Assert.That(
            async () => await _service.ApproveAsync(report.Id),
            Throws.TypeOf<ConflictException>());
    }

    [Test]
    public async Task RejectAsync_WhenReportIsPending_RejectsAndReturnsDto()
    {
        var report = new SightingReportBuilder().Build();
        _reportRepo.GetByIdAsync(Arg.Any<Guid>()).Returns(report);

        var result = await _service.RejectAsync(report.Id);

        await _reportRepo.Received(1).UpdateAsync(report);
        Assert.That(result.Status, Is.EqualTo(ReportStatus.Rejected));
    }

    [Test]
    public async Task RejectAsync_WhenReportIsNotPending_ThrowsConflictException()
    {
        var report = new SightingReportBuilder().Build();
        report.Approve();
        _reportRepo.GetByIdAsync(Arg.Any<Guid>()).Returns(report);

        Assert.That(
            async () => await _service.RejectAsync(report.Id),
            Throws.TypeOf<ConflictException>());
    }

    [Test]
    public async Task ResolveAsync_WhenReportIsVerified_ResolvesAndReturnsDto()
    {
        var report = new SightingReportBuilder().Build();
        report.Approve();
        _reportRepo.GetByIdAsync(Arg.Any<Guid>()).Returns(report);

        var result = await _service.ResolveAsync(report.Id);

        await _reportRepo.Received(1).UpdateAsync(report);
        Assert.That(result.Status, Is.EqualTo(ReportStatus.Resolved));
    }

    [Test]
    public async Task ResolveAsync_WhenReportIsNotVerified_ThrowsConflictException()
    {
        var report = new SightingReportBuilder().Build();
        _reportRepo.GetByIdAsync(Arg.Any<Guid>()).Returns(report);

        Assert.That(
            async () => await _service.ResolveAsync(report.Id),
            Throws.TypeOf<ConflictException>());
    }

    [Test]
    public async Task DeleteAsync_WhenReportNotFound_ThrowsNotFoundException()
    {
        _reportRepo.GetByIdAsync(Arg.Any<Guid>()).Returns((SightingReport?)null);

        Assert.That(
            async () => await _service.DeleteAsync(Guid.NewGuid()),
            Throws.TypeOf<NotFoundException>());
    }

    [Test]
    public async Task DeleteAsync_WhenReportFound_DeletesReport()
    {
        var report = new SightingReportBuilder().Build();
        _reportRepo.GetByIdAsync(Arg.Any<Guid>()).Returns(report);

        await _service.DeleteAsync(report.Id);

        await _reportRepo.Received(1).DeleteAsync(report);
    }
}
