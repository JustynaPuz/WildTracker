using WildTracker.Contracts.DTOs;
using WildTracker.Domain.Entities;

namespace WildTracker.Application.Mappers;

public static class UserMapper
{
    public static AppUserDto ToDto(AppUser entity) => new()
    {
        Id           = entity.Id,
        FirstName    = entity.FirstName,
        LastName     = entity.LastName,
        Email        = entity.Email,
        Role         = entity.Role,
        IsActive     = entity.IsActive,
        CreatedAtUtc = entity.CreatedAtUtc,
    };
}
