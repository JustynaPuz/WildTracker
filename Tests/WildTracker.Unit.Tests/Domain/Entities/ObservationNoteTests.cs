using NUnit.Framework;
using WildTracker.Domain.Entities;
using WildTracker.Unit.Tests.Builders;

namespace WildTracker.Unit.Tests.Domain.Entities;

[TestFixture]
public class ObservationNoteTests
{
    private const int MaxContentLength = 1000;

    private static IEnumerable<TestCaseData> InvalidContents =>
    [
        new TestCaseData("", "Value cannot be empty."),
        new TestCaseData(new string('a', MaxContentLength + 1), $"Value cannot exceed {MaxContentLength} characters."),
    ];

    // --- Constructor ---

    [Test]
    public void Constructor_WithValidData_SetsProperties()
    {
        var reportId = Guid.NewGuid();
        var authorId = Guid.NewGuid();

        var note = new ObservationNote(reportId, authorId, "content");

        Assert.Multiple(() =>
        {
            Assert.That(note.SightingReportId, Is.EqualTo(reportId));
            Assert.That(note.AuthorUserId, Is.EqualTo(authorId));
            Assert.That(note.Content, Is.EqualTo("content"));
        });
    }

    [Test]
    public void Constructor_WithEmptySightingReportId_ThrowsArgumentException()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
        {
            _ = new ObservationNote(Guid.Empty, Guid.NewGuid(), "content");
        });

        Assert.That(ex!.Message, Does.Contain("Value cannot be empty."));
    }

    [Test]
    public void Constructor_WithEmptyAuthorUserId_ThrowsArgumentException()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
        {
            _ = new ObservationNote(Guid.NewGuid(), Guid.Empty, "content");
        });

        Assert.That(ex!.Message, Does.Contain("Value cannot be empty."));
    }

    [TestCaseSource(nameof(InvalidContents))]
    public void Constructor_WithInvalidContent_ThrowsArgumentException(string content, string expectedMessage)
    {
        var ex = Assert.Throws<ArgumentException>(() =>
        {
            _ = new ObservationNote(Guid.NewGuid(), Guid.NewGuid(), content);
        });

        Assert.That(ex!.Message, Does.Contain(expectedMessage));
    }

    // --- UpdateContent ---

    [Test]
    public void UpdateContent_WithValidContent_UpdatesContent()
    {
        var note = new ObservationNoteBuilder().Build();

        note.UpdateContent("updated content");

        Assert.That(note.Content, Is.EqualTo("updated content"));
    }

    [TestCaseSource(nameof(InvalidContents))]
    public void UpdateContent_WithInvalidContent_ThrowsArgumentException(string content, string expectedMessage)
    {
        var note = new ObservationNoteBuilder().Build();

        var ex = Assert.Throws<ArgumentException>(() => note.UpdateContent(content));

        Assert.That(ex!.Message, Does.Contain(expectedMessage));
    }
}