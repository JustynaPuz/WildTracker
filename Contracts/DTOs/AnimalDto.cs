using WildTracker.Contracts.Common;
using WildTracker.Domain.Enums;

namespace WildTracker.Contracts.DTOs;

public record AnimalDto
{
    public Guid Id { get; init; }
    public string Identifier { get; init; } = default!;
    public string Name { get; init; } = default!;
    public Species Species { get; init; }
    public AnimalHealthStatus HealthStatus { get; init; }
    public string? Description { get; init; }
    public DateTime? LastSeenAtUtc { get; init; }
    public DateTime CreatedAtUtc { get; init; }
    public IReadOnlyList<Link> Links { get; init; } = [];
}
