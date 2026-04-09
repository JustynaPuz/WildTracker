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
        Identifier = DomainGuard.RequiredString(identifier, nameof(identifier), 50);
        Name = DomainGuard.RequiredString(name, nameof(name), 100);
        Species = species;
        HealthStatus = healthStatus;
        Description = DomainGuard.OptionalString(description, nameof(description), 1000);
    }

    public void UpdateDetails(string name, Species species, AnimalHealthStatus healthStatus, string? description)
    {
        Name = DomainGuard.RequiredString(name, nameof(name), 100);
        Species = species;
        HealthStatus = healthStatus;
        Description = DomainGuard.OptionalString(description, nameof(description), 1000);
        MarkAsUpdated();
    }

    public void MarkSeen(DateTime seenAtUtc)
    {
        LastSeenAtUtc = seenAtUtc;
        MarkAsUpdated();
    }
}
