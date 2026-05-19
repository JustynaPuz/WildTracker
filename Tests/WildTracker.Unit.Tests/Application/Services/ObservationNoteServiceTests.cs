using NUnit.Framework;
using NSubstitute;
using WildTracker.Application.Exceptions;
using WildTracker.Application.Services;
using WildTracker.Domain.Repositories;
using WildTracker.Unit.Tests.Builders;

namespace WildTracker.Unit.Tests.Application.Services;

// How to approach these tests:
// 1. Create mocks: Substitute.For<IObservationNoteRepository>(), ISightingReportRepository.
// 2. Build a new ObservationNoteService(_noteRepo, _reportRepo).
// 3. For "report not found" tests — configure _reportRepo.GetByIdAsync(...).ReturnsNull().
// 4. For "note belongs to different report" tests — return a note whose SightingReportId
//    does NOT match the reportId argument; the service treats this as not found.

[TestFixture]
public class ObservationNoteServiceTests
{
    // --- GetByReportIdAsync ---

    [Test, Ignore("TODO")]
    public async Task GetByReportIdAsync_WhenReportNotFound_ThrowsNotFoundException() { }

    [Test, Ignore("TODO")]
    public async Task GetByReportIdAsync_WhenReportExists_ReturnsMappedNotes() { }

    // --- GetByIdAsync ---

    [Test, Ignore("TODO")]
    public async Task GetByIdAsync_WhenNoteNotFound_ReturnsNull() { }

    [Test, Ignore("TODO")]
    public async Task GetByIdAsync_WhenNoteBelongsToDifferentReport_ReturnsNull() { }

    [Test, Ignore("TODO")]
    public async Task GetByIdAsync_WhenNoteFound_ReturnsMappedDto() { }

    // --- CreateAsync ---

    [Test, Ignore("TODO")]
    public async Task CreateAsync_WhenReportNotFound_ThrowsNotFoundException() { }

    [Test, Ignore("TODO")]
    public async Task CreateAsync_WhenReportExists_CreatesNoteAndReturnsDto() { }

    // --- UpdateAsync ---

    [Test, Ignore("TODO")]
    public async Task UpdateAsync_WhenNoteNotFound_ThrowsNotFoundException() { }

    [Test, Ignore("TODO")]
    public async Task UpdateAsync_WhenNoteBelongsToDifferentReport_ThrowsNotFoundException() { }

    [Test, Ignore("TODO")]
    public async Task UpdateAsync_WhenNoteFound_UpdatesAndReturnsDto() { }

    // --- DeleteAsync ---

    [Test, Ignore("TODO")]
    public async Task DeleteAsync_WhenNoteNotFound_ThrowsNotFoundException() { }

    [Test, Ignore("TODO")]
    public async Task DeleteAsync_WhenNoteBelongsToDifferentReport_ThrowsNotFoundException() { }

    [Test, Ignore("TODO")]
    public async Task DeleteAsync_WhenNoteFound_DeletesNote() { }
}
