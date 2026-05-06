using WildTracker.Domain.Enums;

namespace WildTracker.Contracts.Requests;

public record CreateAnimalRequest
{
    public string Identifier { get; init; } = default!;
    public string Name { get; init; } = default!;
    public Species Species { get; init; }
    public AnimalHealthStatus HealthStatus { get; init; }
    public string? Description { get; init; }
}

public record UpdateAnimalRequest
{
    public string Name { get; init; } = default!;
    public Species Species { get; init; }
    public AnimalHealthStatus HealthStatus { get; init; }
    public string? Description { get; init; }
}
