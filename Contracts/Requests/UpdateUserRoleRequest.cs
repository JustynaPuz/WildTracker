using WildTracker.Domain.Enums;

namespace WildTracker.Contracts.Requests;

public record UpdateUserRoleRequest
{
    public UserRole Role { get; init; }
}
