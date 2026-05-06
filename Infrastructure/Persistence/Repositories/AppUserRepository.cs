using Microsoft.EntityFrameworkCore;
using WildTracker.Domain.Entities;
using WildTracker.Domain.Repositories;

namespace WildTracker.Infrastructure.Persistence.Repositories;

public class AppUserRepository : IAppUserRepository
{
    private readonly AppDbContext _context;

    public AppUserRepository(AppDbContext context) => _context = context;

    public async Task<AppUser?> GetByIdAsync(Guid id)
        => await _context.Users.FindAsync(id);

    public async Task<AppUser?> GetByEmailAsync(string email)
        => await _context.Users.FirstOrDefaultAsync(u => u.Email == email.ToLowerInvariant());

    public async Task<IEnumerable<AppUser>> GetAllAsync()
        => await _context.Users
            .AsNoTracking()
            .OrderBy(u => u.LastName)
            .ThenBy(u => u.FirstName)
            .ToListAsync();

    public async Task AddAsync(AppUser entity)
    {
        await _context.Users.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(AppUser entity)
    {
        _context.Users.Update(entity);
        await _context.SaveChangesAsync();
    }
}
