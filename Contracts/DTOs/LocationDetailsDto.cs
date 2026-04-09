namespace WildTracker.Contracts.DTOs;

public class LocationDetailsDto
{
    public double Latitude { get; init; }
    public double Longitude { get; init; }
    public string? Region { get; init; }
    public string? ForestDistrict { get; init; }
    public string? Description { get; init; }
}
