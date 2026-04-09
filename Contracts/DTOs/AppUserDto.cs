using WildTracker.Domain.Enums;

namespace WildTracker.Contracts.DTOs;

public class AppUserDto
{
    public Guid Id { get; init; }
    public string FirstName { get; init; } = default!;
    public string LastName { get; init; } = default!;
    public string Email { get; init; } = default!;
    public UserRole Role { get; init; }
}
