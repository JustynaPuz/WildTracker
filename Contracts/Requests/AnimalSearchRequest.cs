using WildTracker.Domain.Enums;
using WildTracker.Domain.Queries;

namespace WildTracker.Contracts.Requests;

public class AnimalSearchRequest
{
    public Species? Species { get; init; }
    public AnimalHealthStatus? HealthStatus { get; init; }
    public string? SearchTerm { get; init; }
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;

    public AnimalFilter ToFilter() => new()
    {
        Species      = Species,
        HealthStatus = HealthStatus,
        SearchTerm   = SearchTerm,
        Page         = Page,
        PageSize     = PageSize,
    };
}
