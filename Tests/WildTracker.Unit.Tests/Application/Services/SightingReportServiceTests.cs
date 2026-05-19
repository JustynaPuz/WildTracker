using NUnit.Framework;
using NSubstitute;
using WildTracker.Application.Exceptions;
using WildTracker.Application.Services;
using WildTracker.Domain.Entities;
using WildTracker.Domain.Repositories;
using WildTracker.Unit.Tests.Builders;

namespace WildTracker.Unit.Tests.Application.Services;

// How to approach these tests:
// 1. Create a mock: Substitute.For<ISightingReportRepository>().
// 2. Build a new SightingReportService(_reportRepo).
// 3. For status transition tests — the service wraps domain exceptions in ConflictException.
//    Build a SightingReport in the required starting state (use SightingReportBuilder +
//    Approve()/Reject() as needed) and return it from the mock.
// 4. Then call ApproveAsync/RejectAsync/ResolveAsync and assert the correct exception type.

[TestFixture]
public class SightingReportServiceTests
{
    // --- GetByIdAsync ---

    [Test, Ignore("TODO")]
    public async Task GetByIdAsync_WhenReportNotFound_ThrowsNotFoundException() { }

    [Test, Ignore("TODO")]
    public async Task GetByIdAsync_WhenReportFound_ReturnsMappedDto() { }

    // --- SearchAsync ---

    [Test, Ignore("TODO")]
    public async Task SearchAsync_ReturnsMappedPagedResult() { }

    // --- CreateAsync ---

    [Test, Ignore("TODO")]
    public async Task CreateAsync_CreatesReportAndReturnsDto() { }

    // --- UpdateAsync ---

    [Test, Ignore("TODO")]
    public async Task UpdateAsync_WhenReportNotFound_ThrowsNotFoundException() { }

    [Test, Ignore("TODO")]
    public async Task UpdateAsync_WhenReportFound_UpdatesAndReturnsDto() { }

    // --- ApproveAsync ---

    [Test, Ignore("TODO")]
    public async Task ApproveAsync_WhenReportNotFound_ThrowsNotFoundException() { }

    [Test, Ignore("TODO")]
    public async Task ApproveAsync_WhenReportIsPending_ApprovesAndReturnsDto() { }

    [Test, Ignore("TODO")]
    public async Task ApproveAsync_WhenReportIsNotPending_ThrowsConflictException() { }

    // --- RejectAsync ---

    [Test, Ignore("TODO")]
    public async Task RejectAsync_WhenReportIsPending_RejectsAndReturnsDto() { }

    [Test, Ignore("TODO")]
    public async Task RejectAsync_WhenReportIsNotPending_ThrowsConflictException() { }

    // --- ResolveAsync ---

    [Test, Ignore("TODO")]
    public async Task ResolveAsync_WhenReportIsVerified_ResolvesAndReturnsDto() { }

    [Test, Ignore("TODO")]
    public async Task ResolveAsync_WhenReportIsNotVerified_ThrowsConflictException() { }

    // --- DeleteAsync ---

    [Test, Ignore("TODO")]
    public async Task DeleteAsync_WhenReportNotFound_ThrowsNotFoundException() { }

    [Test, Ignore("TODO")]
    public async Task DeleteAsync_WhenReportFound_DeletesReport() { }
}
