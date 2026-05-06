using WildTracker.Contracts.DTOs;
using WildTracker.Domain.Enums;

namespace WildTracker.Application.Interfaces;

public interface IUserService
{
    Task<IReadOnlyList<AppUserDto>> GetAllAsync();
    Task<AppUserDto> GetByIdAsync(Guid id);
    Task<AppUserDto> ChangeRoleAsync(Guid id, UserRole role);
    Task<AppUserDto> ActivateAsync(Guid id);
    Task<AppUserDto> DeactivateAsync(Guid id);
}
