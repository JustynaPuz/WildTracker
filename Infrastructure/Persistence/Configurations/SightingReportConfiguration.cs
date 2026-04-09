using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildTracker.Domain.Entities;

namespace WildTracker.Infrastructure.Persistence.Configurations;

public class SightingReportConfiguration : IEntityTypeConfiguration<SightingReport>
{
    public void Configure(EntityTypeBuilder<SightingReport> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.AnimalId).IsRequired();
        builder.Property(x => x.ReportedByUserId).IsRequired();
        builder.Property(x => x.ObservedAtUtc).IsRequired();

        builder.Property(x => x.ReportType)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(x => x.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(x => x.Source)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(x => x.Description)
            .HasMaxLength(2000);

        builder.OwnsOne(x => x.Location, location =>
        {
            location.OwnsOne(l => l.Coordinates, coord =>
            {
                coord.Property(c => c.Latitude)
                    .IsRequired()
                    .HasColumnName("Latitude");

                coord.Property(c => c.Longitude)
                    .IsRequired()
                    .HasColumnName("Longitude");
            });

            location.Property(l => l.Region)
                .HasMaxLength(100)
                .HasColumnName("Region");

            location.Property(l => l.ForestDistrict)
                .HasMaxLength(100)
                .HasColumnName("ForestDistrict");

            location.Property(l => l.Description)
                .HasMaxLength(300)
                .HasColumnName("LocationDescription");
        });

        builder.HasMany(x => x.Notes)
            .WithOne()
            .HasForeignKey(n => n.SightingReportId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.AnimalId);
        builder.HasIndex(x => x.ObservedAtUtc);
        builder.HasIndex(x => x.Status);
    }
}
