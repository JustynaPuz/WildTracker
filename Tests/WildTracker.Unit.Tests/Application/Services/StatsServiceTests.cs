using NUnit.Framework;
using NSubstitute;
using WildTracker.Application.Services;
using WildTracker.Domain.Repositories;

namespace WildTracker.Unit.Tests.Application.Services;

// How to approach these tests:
// 1. Create a mock: Substitute.For<IStatsRepository>().
// 2. Build a new StatsService(_repo).
// 3. For GetSummaryAsync — configure each Count* method to return a specific number,
//    then verify that TotalReports equals their sum (pending + verified + rejected + resolved).
// 4. For GetByMonthAsync — configure GetReportsByMonthAsync to return rows and verify the mapping.
//    Also test that passing months=0 or months=37 clamps to the valid range (1–36).

[TestFixture]
public class StatsServiceTests
{
    // --- GetSummaryAsync ---

    [Test, Ignore("TODO")]
    public async Task GetSummaryAsync_ReturnsSummaryWithAllCountsFromRepository() { }

    [Test, Ignore("TODO")]
    public async Task GetSummaryAsync_TotalReports_IsSum_Of_AllStatusCounts() { }

    // --- GetBySpeciesAsync ---

    [Test, Ignore("TODO")]
    public async Task GetBySpeciesAsync_ReturnsMappedRows() { }

    // --- GetByMonthAsync ---

    [Test, Ignore("TODO")]
    public async Task GetByMonthAsync_ReturnsMappedRows() { }

    [Test, Ignore("TODO")]
    public async Task GetByMonthAsync_ClampsMonthsBelowOne_ToOne() { }

    [Test, Ignore("TODO")]
    public async Task GetByMonthAsync_ClampsMonthsAbove36_To36() { }
}
