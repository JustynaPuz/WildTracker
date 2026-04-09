using Microsoft.EntityFrameworkCore;
using WildTracker.Domain.Entities;
using WildTracker.Domain.Repositories;

namespace WildTracker.Infrastructure.Persistence.Repositories;

public class ObservationNoteRepository : IObservationNoteRepository
{
    private readonly AppDbContext _context;

    public ObservationNoteRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ObservationNote>> GetByReportIdAsync(Guid reportId)
        => await _context.ObservationNotes
            .AsNoTracking()
            .Where(x => x.SightingReportId == reportId)
            .OrderBy(x => x.CreatedAtUtc)
            .ToListAsync();

    public async Task AddAsync(ObservationNote entity)
    {
        await _context.ObservationNotes.AddAsync(entity);
        await _context.SaveChangesAsync();
    }
}
