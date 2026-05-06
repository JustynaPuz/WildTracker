using WildTracker.Contracts.Common;
using WildTracker.Domain.Enums;

namespace WildTracker.Contracts.DTOs;

public record AppUserDto
{
    public Guid Id { get; init; }
    public string FirstName { get; init; } = default!;
    public string LastName { get; init; } = default!;
    public string Email { get; init; } = default!;
    public UserRole Role { get; init; }
    public bool IsActive { get; init; }
    public DateTime CreatedAtUtc { get; init; }
    public IReadOnlyList<Link> Links { get; init; } = [];
}
