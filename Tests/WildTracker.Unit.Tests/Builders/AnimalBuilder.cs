using WildTracker.Domain.Entities;
using WildTracker.Domain.Enums;

namespace WildTracker.Unit.Tests.Builders;

internal sealed class AnimalBuilder
{
    private string _identifier = "WOLF-001";
    private string _name = "Grey Wolf";
    private Species _species = Species.Wolf;
    private AnimalHealthStatus _healthStatus = AnimalHealthStatus.Healthy;
    private string? _description = null;

    internal AnimalBuilder WithIdentifier(string id)              { _identifier = id;     return this; }
    internal AnimalBuilder WithName(string name)                  { _name = name;         return this; }
    internal AnimalBuilder WithSpecies(Species s)                 { _species = s;         return this; }
    internal AnimalBuilder WithHealthStatus(AnimalHealthStatus h) { _healthStatus = h;    return this; }
    internal AnimalBuilder WithDescription(string? d)             { _description = d;     return this; }

    internal Animal Build() =>
        new(_identifier, _name, _species, _healthStatus, _description);
}
