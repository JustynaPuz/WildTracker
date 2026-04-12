namespace WildTracker.Contracts.Common;

/// <summary>Envelope for a non-paged list response with hypermedia links.</summary>
public record CollectionResponse<T>
{
    public IReadOnlyList<T> Items { get; init; } = [];
    public IReadOnlyList<Link> Links { get; init; } = [];
}
