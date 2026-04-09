using Microsoft.EntityFrameworkCore;
using WildTracker.Domain.Entities;

namespace WildTracker.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public DbSet<Animal> Animals => Set<Animal>();
    public DbSet<AppUser> Users => Set<AppUser>();
    public DbSet<ObservationNote> ObservationNotes => Set<ObservationNote>();
    public DbSet<SightingReport> SightingReports => Set<SightingReport>();

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}