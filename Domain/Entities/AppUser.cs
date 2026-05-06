using WildTracker.Domain.Common;
using WildTracker.Domain.Enums;

namespace WildTracker.Domain.Entities;

public class AppUser : AuditableEntity
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public UserRole Role { get; private set; }
    public bool IsActive { get; private set; }

    private AppUser()
    {
        FirstName = string.Empty;
        LastName  = string.Empty;
        Email     = string.Empty;
    }

    public AppUser(string firstName, string lastName, string email, UserRole role)
    {
        FirstName = DomainGuard.RequiredString(firstName, nameof(firstName), 100);
        LastName  = DomainGuard.RequiredString(lastName,  nameof(lastName),  100);
        Email     = ValidateEmail(email);
        Role      = role;
        IsActive  = true;
    }

    public void UpdateProfile(string firstName, string lastName, string email)
    {
        FirstName = DomainGuard.RequiredString(firstName, nameof(firstName), 100);
        LastName  = DomainGuard.RequiredString(lastName,  nameof(lastName),  100);
        Email     = ValidateEmail(email);
        MarkAsUpdated();
    }

    public void ChangeRole(UserRole role)
    {
        Role = role;
        MarkAsUpdated();
    }

    public void Deactivate()
    {
        IsActive = false;
        MarkAsUpdated();
    }

    public void Activate()
    {
        IsActive = true;
        MarkAsUpdated();
    }

    /// <summary>
    /// Email has its own rule (must contain '@') that doesn't fit DomainGuard's generic pattern,
    /// so it stays as a dedicated private method.
    /// </summary>
    private static string ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty.", nameof(email));

        var normalised = email.Trim().ToLowerInvariant();

        if (normalised.Length > 200)
            throw new ArgumentException("Email cannot exceed 200 characters.", nameof(email));

        if (!normalised.Contains('@'))
            throw new ArgumentException("Email format is invalid.", nameof(email));

        return normalised;
    }
}
