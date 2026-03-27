namespace WildTracker.Domain.Common;

public abstract class AuditableEntity : BaseEntity
{
    public DateTime CreatedAtUtc { get; protected set; }
    public DateTime? UpdatedAtUtc { get; protected set; }

    protected AuditableEntity()
    {
        CreatedAtUtc = DateTime.UtcNow;
    }

    protected void MarkAsUpdated()
    {
        UpdatedAtUtc = DateTime.UtcNow;
    }
}