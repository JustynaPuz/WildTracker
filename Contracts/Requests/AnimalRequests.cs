using WildTracker.Domain.Enums;

namespace WildTracker.Contracts.Requests;

public class CreateAnimalRequest
{
    public string Identifier { get; init; } = default!;
    public string Name { get; init; } = default!;
    public Species Species { get; init; }
    public AnimalHealthStatus HealthStatus { get; init; }
    public string? Description { get; init; }
}

public class UpdateAnimalRequest
{
    public string Name { get; init; } = default!;
    public Species Species { get; init; }
    public AnimalHealthStatus HealthStatus { get; init; }
    public string? Description { get; init; }
}
