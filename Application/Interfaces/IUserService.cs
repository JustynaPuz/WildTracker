using WildTracker.Contracts.DTOs;
using WildTracker.Domain.Enums;

namespace WildTracker.Application.Interfaces;

public interface IUserService
{
    Task<IEnumerable<AppUserDto>> GetAllAsync();
    Task<AppUserDto> GetByIdAsync(Guid id);
    Task<AppUserDto> ChangeRoleAsync(Guid id, UserRole role);
}
