using WildTracker.Domain.Entities;

namespace WildTracker.Domain.Repositories;

public interface IAppUserRepository
{
    Task<AppUser?> GetByIdAsync(Guid id);
    Task<AppUser?> GetByEmailAsync(string email);
    Task<IEnumerable<AppUser>> GetAllAsync();
    Task AddAsync(AppUser entity);
    Task UpdateAsync(AppUser entity);
}
