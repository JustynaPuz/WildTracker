namespace WildTracker.Contracts.DTOs;

public record SightingsBySpeciesDto
{
    public string Species { get; init; } = default!;
    public int Count { get; init; }
}
