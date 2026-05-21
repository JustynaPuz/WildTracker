using NUnit.Framework;
using NSubstitute;
using WildTracker.Application.Exceptions;
using WildTracker.Application.Services;
using WildTracker.Domain.Entities;
using WildTracker.Domain.Repositories;
using WildTracker.Unit.Tests.Builders;

namespace WildTracker.Unit.Tests.Application.Services;

[TestFixture]
public class ObservationNoteServiceTests
{
    private IObservationNoteRepository _noteRepo = null!;
    private ISightingReportRepository _reportRepo = null!;
    private ObservationNoteService _service = null!;

    [SetUp]
    public void SetUp()
    {
        _noteRepo   = Substitute.For<IObservationNoteRepository>();
        _reportRepo = Substitute.For<ISightingReportRepository>();
        _service    = new ObservationNoteService(_noteRepo, _reportRepo);
    }

    [Test]
    public async Task GetByReportIdAsync_WhenReportNotFound_ThrowsNotFoundException()
    {
        _reportRepo.GetByIdAsync(Arg.Any<Guid>()).Returns((SightingReport?)null);

        Assert.That(
            async () => await _service.GetByReportIdAsync(Guid.NewGuid()),
            Throws.TypeOf<NotFoundException>());
    }

    [Test]
    public async Task GetByReportIdAsync_WhenReportExists_ReturnsMappedNotes()
    {
        var reportId = Guid.NewGuid();
        var note     = new ObservationNoteBuilder().WithContent("test content").Build();
        _reportRepo.GetByIdAsync(reportId).Returns(new SightingReportBuilder().Build());
        _noteRepo.GetByReportIdAsync(reportId).Returns(new List<ObservationNote> { note });

        var result = await _service.GetByReportIdAsync(reportId);

        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Content, Is.EqualTo("test content"));
    }

    [Test]
    public async Task GetByIdAsync_WhenNoteNotFound_ReturnsNull()
    {
        _noteRepo.GetByIdAsync(Arg.Any<Guid>()).Returns((ObservationNote?)null);

        var result = await _service.GetByIdAsync(Guid.NewGuid(), Guid.NewGuid());

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetByIdAsync_WhenNoteBelongsToDifferentReport_ReturnsNull()
    {
        var note = new ObservationNoteBuilder().Build();
        _noteRepo.GetByIdAsync(Arg.Any<Guid>()).Returns(note);

        var result = await _service.GetByIdAsync(Guid.NewGuid(), note.Id);

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetByIdAsync_WhenNoteFound_ReturnsMappedDto()
    {
        var reportId = Guid.NewGuid();
        var note     = new ObservationNoteBuilder().WithReportId(reportId).WithContent("test content").Build();
        _noteRepo.GetByIdAsync(Arg.Any<Guid>()).Returns(note);

        var result = await _service.GetByIdAsync(reportId, note.Id);

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Content, Is.EqualTo("test content"));
    }

    [Test]
    public async Task CreateAsync_WhenReportNotFound_ThrowsNotFoundException()
    {
        _reportRepo.GetByIdAsync(Arg.Any<Guid>()).Returns((SightingReport?)null);

        Assert.That(
            async () => await _service.CreateAsync(Guid.NewGuid(), "content", Guid.NewGuid()),
            Throws.TypeOf<NotFoundException>());
    }

    [Test]
    public async Task CreateAsync_WhenReportExists_CreatesNoteAndReturnsDto()
    {
        var reportId = Guid.NewGuid();
        var authorId = Guid.NewGuid();
        _reportRepo.GetByIdAsync(reportId).Returns(new SightingReportBuilder().Build());

        var result = await _service.CreateAsync(reportId, "test content", authorId);

        await _noteRepo.Received(1).AddAsync(Arg.Any<ObservationNote>());
        Assert.Multiple(() =>
        {
            Assert.That(result.Content,          Is.EqualTo("test content"));
            Assert.That(result.SightingReportId, Is.EqualTo(reportId));
            Assert.That(result.AuthorUserId,     Is.EqualTo(authorId));
        });
    }

    [Test]
    public async Task UpdateAsync_WhenNoteNotFound_ThrowsNotFoundException()
    {
        _noteRepo.GetByIdAsync(Arg.Any<Guid>()).Returns((ObservationNote?)null);

        Assert.That(
            async () => await _service.UpdateAsync(Guid.NewGuid(), Guid.NewGuid(), "content"),
            Throws.TypeOf<NotFoundException>());
    }

    [Test]
    public async Task UpdateAsync_WhenNoteBelongsToDifferentReport_ThrowsNotFoundException()
    {
        var note = new ObservationNoteBuilder().Build();
        _noteRepo.GetByIdAsync(Arg.Any<Guid>()).Returns(note);

        Assert.That(
            async () => await _service.UpdateAsync(Guid.NewGuid(), note.Id, "content"),
            Throws.TypeOf<NotFoundException>());
    }

    [Test]
    public async Task UpdateAsync_WhenNoteFound_UpdatesAndReturnsDto()
    {
        var reportId = Guid.NewGuid();
        var note     = new ObservationNoteBuilder().WithReportId(reportId).Build();
        _noteRepo.GetByIdAsync(Arg.Any<Guid>()).Returns(note);

        var result = await _service.UpdateAsync(reportId, note.Id, "updated content");

        await _noteRepo.Received(1).UpdateAsync(note);
        Assert.That(result.Content, Is.EqualTo("updated content"));
    }

    [Test]
    public async Task DeleteAsync_WhenNoteNotFound_ThrowsNotFoundException()
    {
        _noteRepo.GetByIdAsync(Arg.Any<Guid>()).Returns((ObservationNote?)null);

        Assert.That(
            async () => await _service.DeleteAsync(Guid.NewGuid(), Guid.NewGuid()),
            Throws.TypeOf<NotFoundException>());
    }

    [Test]
    public async Task DeleteAsync_WhenNoteBelongsToDifferentReport_ThrowsNotFoundException()
    {
        var note = new ObservationNoteBuilder().Build();
        _noteRepo.GetByIdAsync(Arg.Any<Guid>()).Returns(note);

        Assert.That(
            async () => await _service.DeleteAsync(Guid.NewGuid(), note.Id),
            Throws.TypeOf<NotFoundException>());
    }

    [Test]
    public async Task DeleteAsync_WhenNoteFound_DeletesNote()
    {
        var reportId = Guid.NewGuid();
        var note     = new ObservationNoteBuilder().WithReportId(reportId).Build();
        _noteRepo.GetByIdAsync(Arg.Any<Guid>()).Returns(note);

        await _service.DeleteAsync(reportId, note.Id);

        await _noteRepo.Received(1).DeleteAsync(note);
    }
}
