using Microsoft.EntityFrameworkCore;
using WildTracker.Domain.Entities;
using WildTracker.Domain.Queries;
using WildTracker.Domain.Repositories;

namespace WildTracker.Infrastructure.Persistence.Repositories;

public class SightingReportRepository : ISightingReportRepository
{
    private readonly AppDbContext _context;

    public SightingReportRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<SightingReport?> GetByIdAsync(Guid id)
        => await _context.SightingReports.FindAsync(id);

    public async Task<(IReadOnlyList<SightingReport> Items, int TotalCount)> SearchAsync(SightingReportFilter filter)
    {
        var query = _context.SightingReports.AsNoTracking().AsQueryable();

        if (filter.AnimalId.HasValue)
            query = query.Where(x => x.AnimalId == filter.AnimalId);

        if (!string.IsNullOrWhiteSpace(filter.AnimalSearchTerm))
        {
            var term = filter.AnimalSearchTerm.ToLower();
            query = query.Where(x => _context.Animals.Any(a =>
                a.Id == x.AnimalId && (a.Name.ToLower().Contains(term) || a.Identifier.ToLower().Contains(term))));
        }

        if (filter.ReportedByUserId.HasValue)
            query = query.Where(x => x.ReportedByUserId == filter.ReportedByUserId);

        if (filter.Species.HasValue)
        {
            var species = filter.Species.Value;
            query = query.Where(x => _context.Animals.Any(a => a.Id == x.AnimalId && a.Species == species));
        }

        if (filter.Status.HasValue)
            query = query.Where(x => x.Status == filter.Status);

        if (filter.From.HasValue)
            query = query.Where(x => x.ObservedAtUtc >= filter.From);

        if (filter.To.HasValue)
            query = query.Where(x => x.ObservedAtUtc <= filter.To);

        var total = await query.CountAsync();

        var pageSize = Math.Clamp(filter.PageSize, 1, 100);
        var page     = Math.Max(filter.Page, 1);

        var items = await query
            .OrderByDescending(x => x.ObservedAtUtc)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, total);
    }

    public async Task AddAsync(SightingReport entity)
    {
        await _context.SightingReports.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(SightingReport entity)
    {
        _context.SightingReports.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(SightingReport entity)
    {
        _context.SightingReports.Remove(entity);
        await _context.SaveChangesAsync();
    }
}
