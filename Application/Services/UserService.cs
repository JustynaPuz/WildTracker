using WildTracker.Application.Exceptions;
using WildTracker.Application.Interfaces;
using WildTracker.Application.Mappers;
using WildTracker.Contracts.DTOs;
using WildTracker.Domain.Enums;
using WildTracker.Domain.Repositories;

namespace WildTracker.Application.Services;

public class UserService : IUserService
{
    private readonly IAppUserRepository _repo;

    public UserService(IAppUserRepository repo) => _repo = repo;

    public async Task<IEnumerable<AppUserDto>> GetAllAsync()
    {
        var users = await _repo.GetAllAsync();
        return users.Select(UserMapper.ToDto);
    }

    public async Task<AppUserDto> GetByIdAsync(Guid id)
    {
        var user = await _repo.GetByIdAsync(id)
            ?? throw new NotFoundException($"User with id '{id}' was not found.");

        return UserMapper.ToDto(user);
    }

    public async Task<AppUserDto> ChangeRoleAsync(Guid id, UserRole role)
    {
        var user = await _repo.GetByIdAsync(id)
            ?? throw new NotFoundException($"User with id '{id}' was not found.");

        user.ChangeRole(role);
        await _repo.UpdateAsync(user);
        return UserMapper.ToDto(user);
    }
}
