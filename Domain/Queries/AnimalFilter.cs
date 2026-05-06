using WildTracker.Domain.Enums;

namespace WildTracker.Domain.Queries;

public class AnimalFilter
{
    public Species? Species { get; init; }
    public AnimalHealthStatus? HealthStatus { get; init; }
    public string? SearchTerm { get; init; }
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;
}
