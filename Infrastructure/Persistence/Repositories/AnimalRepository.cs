using Microsoft.EntityFrameworkCore;
using WildTracker.Domain.Entities;
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

    public async Task<IEnumerable<Animal>> GetAllAsync()
        => await _context.Animals.AsNoTracking().OrderBy(a => a.Name).ToListAsync();

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
