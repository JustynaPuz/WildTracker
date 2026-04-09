using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildTracker.Domain.Entities;

namespace WildTracker.Infrastructure.Persistence.Configurations;

public class ObservationNoteConfiguration : IEntityTypeConfiguration<ObservationNote>
{
    public void Configure(EntityTypeBuilder<ObservationNote> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.SightingReportId).IsRequired();
        builder.Property(x => x.AuthorUserId).IsRequired();

        builder.Property(x => x.Content)
            .IsRequired()
            .HasMaxLength(1000);

        builder.HasIndex(x => x.SightingReportId);
    }
}
