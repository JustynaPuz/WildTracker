using Microsoft.EntityFrameworkCore;
using WildTracker.Domain.Entities;
using WildTracker.Domain.Queries;
using WildTracker.Domain.Repositories;

namespace WildTracker.Infrastructure.Persistence.Repositories;

public class AnimalRepository : IAnimalRepository
{
    private readonly AppDbContext _context;

    public AnimalRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Animal?> GetByIdAsync(Guid id)
        => await _context.Animals.FindAsync(id);

    public async Task<IReadOnlyList<Animal>> GetAllAsync()
        => await _context.Animals.AsNoTracking().OrderBy(a => a.Name).ToListAsync();

    public async Task<(IReadOnlyList<Animal> Items, int TotalCount)> SearchAsync(AnimalFilter filter)
    {
        var query = _context.Animals.AsNoTracking().AsQueryable();

        if (filter.Species.HasValue)
            query = query.Where(a => a.Species == filter.Species.Value);

        if (filter.HealthStatus.HasValue)
            query = query.Where(a => a.HealthStatus == filter.HealthStatus.Value);

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            var term = filter.SearchTerm.ToLower();
            query = query.Where(a => a.Name.ToLower().Contains(term) || a.Identifier.ToLower().Contains(term));
        }

        var total = await query.CountAsync();

        var pageSize = Math.Clamp(filter.PageSize, 1, 100);
        var page     = Math.Max(filter.Page, 1);

        var items = await query
            .OrderBy(a => a.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, total);
    }

    public async Task AddAsync(Animal entity)
    {
        await _context.Animals.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Animal entity)
    {
        _context.Animals.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Animal entity)
    {
        _context.Animals.Remove(entity);
        await _context.SaveChangesAsync();
    }
}
