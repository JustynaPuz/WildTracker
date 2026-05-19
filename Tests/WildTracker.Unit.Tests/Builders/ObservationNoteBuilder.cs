using WildTracker.Domain.Entities;

namespace WildTracker.Unit.Tests.Builders;

internal sealed class ObservationNoteBuilder
{
    private Guid _reportId  = Guid.NewGuid();
    private Guid _authorId  = Guid.NewGuid();
    private string _content = "Test observation content.";

    internal ObservationNoteBuilder WithReportId(Guid id)  { _reportId = id;   return this; }
    internal ObservationNoteBuilder WithAuthorId(Guid id)  { _authorId = id;   return this; }
    internal ObservationNoteBuilder WithContent(string c)  { _content = c;     return this; }

    internal ObservationNote Build() =>
        new(_reportId, _authorId, _content);
}
