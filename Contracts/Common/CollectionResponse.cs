namespace WildTracker.Contracts.Common;

public record CollectionResponse<T>
{
    public IReadOnlyList<T> Items { get; init; } = [];
    public IReadOnlyList<Link> Links { get; init; } = [];
}
