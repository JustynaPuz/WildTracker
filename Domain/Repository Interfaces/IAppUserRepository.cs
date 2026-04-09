using WildTracker.Domain.Entities;

namespace WildTracker.Domain.Repositories;

public interface IAppUserRepository
{
    Task<AppUser?> GetByIdAsync(Guid id);
    Task<AppUser?> GetByEmailAsync(string email);
    Task AddAsync(AppUser entity);
}
