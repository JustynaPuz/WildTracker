using WildTracker.Domain.Common;
using WildTracker.Domain.Enums;

namespace WildTracker.Domain.Entities;

public class Animal : AuditableEntity
{
    public string Identifier { get; private set; }
    public string Name { get; private set; }
    public Species Species { get; private set; }
    public AnimalHealthStatus HealthStatus { get; private set; }
    public string? Description { get; private set; }
    public DateTime? LastSeenAtUtc { get; private set; }

    private Animal()
    {
        Identifier = string.Empty;
        Name = string.Empty;
    }

    public Animal(string identifier, string name, Species species, AnimalHealthStatus healthStatus, string? description = null)
    {
        Identifier = ValidateRequired(identifier, nameof(identifier), 50);
        Name = ValidateRequired(name, nameof(name), 100);
        Species = species;
        HealthStatus = healthStatus;
        Description = Normalize(description, 1000);
    }

    public void UpdateDetails(string name, Species species, AnimalHealthStatus healthStatus, string? description)
    {
        Name = ValidateRequired(name, nameof(name), 100);
        Species = species;
        HealthStatus = healthStatus;
        Description = Normalize(description, 1000);
        MarkAsUpdated();
    }

    public void MarkSeen(DateTime seenAtUtc)
    {
        LastSeenAtUtc = seenAtUtc;
        MarkAsUpdated();
    }

    private static string ValidateRequired(string value, string paramName, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value cannot be empty.", paramName);
        }

        var normalized = value.Trim();

        if (normalized.Length > maxLength)
        {
            throw new ArgumentException($"Value cannot exceed {maxLength} characters.", paramName);
        }

        return normalized;
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