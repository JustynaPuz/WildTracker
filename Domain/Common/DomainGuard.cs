namespace WildTracker.Domain.Common;

internal static class DomainGuard
{
    internal static string RequiredString(string value, string paramName, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Value cannot be empty.", paramName);

        var trimmed = value.Trim();

        if (trimmed.Length > maxLength)
            throw new ArgumentException($"Value cannot exceed {maxLength} characters.", paramName);

        return trimmed;
    }

    internal static string? OptionalString(string? value, string paramName, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        var trimmed = value.Trim();

        if (trimmed.Length > maxLength)
            throw new ArgumentException($"Value cannot exceed {maxLength} characters.", paramName);

        return trimmed;
    }
}
