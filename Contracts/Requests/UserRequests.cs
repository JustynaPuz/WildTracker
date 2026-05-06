using WildTracker.Domain.Enums;

namespace WildTracker.Contracts.Requests;

public class UpdateUserRoleRequest
{
    public UserRole Role { get; init; }
}

public class UpdateUserStatusRequest
{
    public bool IsActive { get; init; }
}
