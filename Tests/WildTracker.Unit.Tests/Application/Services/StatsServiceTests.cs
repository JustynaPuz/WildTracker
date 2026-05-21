using NUnit.Framework;
using NSubstitute;
using WildTracker.Application.Services;
using WildTracker.Domain.Enums;
using WildTracker.Domain.Repositories;

namespace WildTracker.Unit.Tests.Application.Services;

[TestFixture]
public class StatsServiceTests
{
    private IStatsRepository _repo = null!;
    private StatsService _service = null!;

    [SetUp]
    public void SetUp()
    {
        _repo    = Substitute.For<IStatsRepository>();
        _service = new StatsService(_repo);
    }

    [Test]
    public async Task GetSummaryAsync_ReturnsSummaryWithAllCountsFromRepository()
    {
        _repo.CountAnimalsAsync().Returns(10);
        _repo.CountUsersAsync().Returns(5);
        _repo.CountNotesAsync().Returns(20);
        _repo.CountReportsByStatusAsync(ReportStatus.Pending).Returns(3);
        _repo.CountReportsByStatusAsync(ReportStatus.Verified).Returns(4);
        _repo.CountReportsByStatusAsync(ReportStatus.Rejected).Returns(2);
        _repo.CountReportsByStatusAsync(ReportStatus.Resolved).Returns(1);

        var result = await _service.GetSummaryAsync();

        Assert.Multiple(() =>
        {
            Assert.That(result.TotalAnimals,    Is.EqualTo(10));
            Assert.That(result.TotalUsers,      Is.EqualTo(5));
            Assert.That(result.TotalNotes,      Is.EqualTo(20));
            Assert.That(result.PendingReports,  Is.EqualTo(3));
            Assert.That(result.VerifiedReports, Is.EqualTo(4));
            Assert.That(result.RejectedReports, Is.EqualTo(2));
            Assert.That(result.ResolvedReports, Is.EqualTo(1));
        });
    }

    [Test]
    public async Task GetSummaryAsync_TotalReports_IsSum_Of_AllStatusCounts()
    {
        _repo.CountAnimalsAsync().Returns(0);
        _repo.CountUsersAsync().Returns(0);
        _repo.CountNotesAsync().Returns(0);
        _repo.CountReportsByStatusAsync(ReportStatus.Pending).Returns(3);
        _repo.CountReportsByStatusAsync(ReportStatus.Verified).Returns(4);
        _repo.CountReportsByStatusAsync(ReportStatus.Rejected).Returns(2);
        _repo.CountReportsByStatusAsync(ReportStatus.Resolved).Returns(1);

        var result = await _service.GetSummaryAsync();

        Assert.That(result.TotalReports, Is.EqualTo(10));
    }

    [Test]
    public async Task GetBySpeciesAsync_ReturnsMappedRows()
    {
        IReadOnlyList<(string Species, int Count)> rows = [("Wolf", 5), ("Bear", 3)];
        _repo.GetReportsBySpeciesAsync().Returns(rows);

        var result = await _service.GetBySpeciesAsync();

        Assert.That(result, Has.Count.EqualTo(2));
        Assert.That(result.Select(r => r.Species), Does.Contain("Wolf").And.Contain("Bear"));
    }

    [Test]
    public async Task GetByMonthAsync_ReturnsMappedRows()
    {
        IReadOnlyList<(int Year, int Month, int Count)> rows = [(2025, 1, 10), (2025, 2, 5)];
        _repo.GetReportsByMonthAsync(Arg.Any<int>()).Returns(rows);

        var result = await _service.GetByMonthAsync(2);

        Assert.That(result, Has.Count.EqualTo(2));
        Assert.Multiple(() =>
        {
            Assert.That(result[0].Year,  Is.EqualTo(2025));
            Assert.That(result[0].Month, Is.EqualTo(1));
            Assert.That(result[0].Count, Is.EqualTo(10));
        });
    }

    [Test]
    public async Task GetByMonthAsync_ClampsMonthsBelowOne_ToOne()
    {
        _repo.GetReportsByMonthAsync(Arg.Any<int>()).Returns(new List<(int, int, int)>());

        await _service.GetByMonthAsync(0);

        await _repo.Received(1).GetReportsByMonthAsync(1);
    }

    [Test]
    public async Task GetByMonthAsync_ClampsMonthsAbove36_To36()
    {
        _repo.GetReportsByMonthAsync(Arg.Any<int>()).Returns(new List<(int, int, int)>());

        await _service.GetByMonthAsync(37);

        await _repo.Received(1).GetReportsByMonthAsync(36);
    }
}
