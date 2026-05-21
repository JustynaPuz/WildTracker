using NUnit.Framework;
using WildTracker.Application.Mappers;
using WildTracker.Unit.Tests.Builders;

namespace WildTracker.Unit.Tests.Application.Mappers;

[TestFixture]
public class ObservationNoteMapperTests
{

    [Test]
    public void ToDto_MapsAllProperties()
    {
        var note = new ObservationNoteBuilder()
            .WithContent("test observation content")
            .Build();

        var result = ObservationNoteMapper.ToDto(note);

        Assert.Multiple(() =>
        {
            Assert.That(result.Id,               Is.EqualTo(note.Id));
            Assert.That(result.SightingReportId, Is.EqualTo(note.SightingReportId));
            Assert.That(result.AuthorUserId,     Is.EqualTo(note.AuthorUserId));
            Assert.That(result.Content,          Is.EqualTo("test observation content"));
            Assert.That(result.CreatedAtUtc,     Is.EqualTo(note.CreatedAtUtc));
            Assert.That(result.UpdatedAtUtc,     Is.EqualTo(note.UpdatedAtUtc));
        });
    }
}
