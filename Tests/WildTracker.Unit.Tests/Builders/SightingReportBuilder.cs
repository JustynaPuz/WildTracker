using WildTracker.Domain.Entities;
using WildTracker.Domain.Enums;
using WildTracker.Domain.ValueObjects;

namespace WildTracker.Unit.Tests.Builders;

internal sealed class SightingReportBuilder
{
    private Guid _animalId = Guid.NewGuid();
    private Guid _userId   = Guid.NewGuid();
    private DateTime _observedAt = DateTime.UtcNow;
    private ReportType _reportType = ReportType.Sighting;
    private SightingSource _source = SightingSource.Manual;
    private LocationDetails _location = new(new Coordinates(52.0, 19.0));
    private string? _description = null;

    internal SightingReportBuilder WithAnimalId(Guid id)          { _animalId = id;       return this; }
    internal SightingReportBuilder WithUserId(Guid id)            { _userId = id;         return this; }
    internal SightingReportBuilder WithObservedAt(DateTime dt)    { _observedAt = dt;     return this; }
    internal SightingReportBuilder WithReportType(ReportType t)   { _reportType = t;      return this; }
    internal SightingReportBuilder WithSource(SightingSource s)   { _source = s;          return this; }
    internal SightingReportBuilder WithLocation(LocationDetails l){ _location = l;        return this; }
    internal SightingReportBuilder WithDescription(string? d)     { _description = d;     return this; }

    internal SightingReport Build() =>
        new(_animalId, _userId, _observedAt, _reportType, _source, _location, _description);
}
