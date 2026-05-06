namespace WildTracker.Contracts.DTOs;

public record SightingsByMonthDto
{
    public int Year { get; init; }
    public int Month { get; init; }
    public int Count { get; init; }
    public string Label => $"{Year:0000}-{Month:00}";
}
