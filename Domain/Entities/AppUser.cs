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
        LastName = string.Empty;
        Email = string.Empty;
    }

    public AppUser(string firstName, string lastName, string email, UserRole role)
    {
        FirstName = ValidateRequired(firstName, nameof(firstName), 100);
        LastName = ValidateRequired(lastName, nameof(lastName), 100);
        Email = ValidateEmail(email);
        Role = role;
        IsActive = true;
    }

    public void UpdateProfile(string firstName, string lastName, string email)
    {
        FirstName = ValidateRequired(firstName, nameof(firstName), 100);
        LastName = ValidateRequired(lastName, nameof(lastName), 100);
        Email = ValidateEmail(email);
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

    private static string ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email cannot be empty.", nameof(email));
        }

        var normalized = email.Trim();

        if (normalized.Length > 200)
        {
            throw new ArgumentException("Email cannot exceed 200 characters.", nameof(email));
        }

        if (!normalized.Contains('@'))
        {
            throw new ArgumentException("Email format is invalid.", nameof(email));
        }

        return normalized;
    }
}