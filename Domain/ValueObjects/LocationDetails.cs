namespace WildTracker.Domain.ValueObjects;

public sealed class LocationDetails : IEquatable<LocationDetails>
{
    private LocationDetails() { } // required by EF Core

    public Coordinates Coordinates { get; private set; } = null!;
    public string? Region { get; private set; }
    public string? ForestDistrict { get; private set; }
    public string? Description { get; private set; }

    public LocationDetails(Coordinates coordinates, string? region = null, string? forestDistrict = null, string? description = null)
    {
        Coordinates = coordinates;
        Region = Normalize(region, 100);
        ForestDistrict = Normalize(forestDistrict, 100);
        Description = Normalize(description, 300);
    }

    public bool Equals(LocationDetails? other)
    {
        if (other is null)
        {
            return false;
        }

        return Coordinates.Equals(other.Coordinates)
            && Region == other.Region
            && ForestDistrict == other.ForestDistrict
            && Description == other.Description;
    }

    public override bool Equals(object? obj)
    {
        return obj is LocationDetails other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Coordinates, Region, ForestDistrict, Description);
    }

    private static string? Normalize(string? value, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        var normalized = value.Trim();

        if (normalized.Length > maxLength)
        {
            throw new ArgumentException($"Value cannot exceed {maxLength} characters.");
        }

        return normalized;
    }
}