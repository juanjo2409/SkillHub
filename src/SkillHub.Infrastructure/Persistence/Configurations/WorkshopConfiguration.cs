using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillHub.Domain.Entities;
using SkillHub.Domain.ValueObjects;

namespace SkillHub.Infrastructure.Persistence.Configurations;

public class WorkshopConfiguration : IEntityTypeConfiguration<Workshop>
{
    public void Configure(EntityTypeBuilder<Workshop> builder)
    {
        builder.ToTable("Workshops");

        builder.HasKey(w => w.Id);

        builder.Property(w => w.Code)
            .HasConversion(
                code => code.Value,
                value => new WorkshopCode(value))
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(w => w.Code)
            .IsUnique();

        builder.Property(w => w.Title)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(w => w.Description)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(w => w.MaxCapacity)
            .IsRequired();

        builder.OwnsOne(w => w.Schedule, schedule =>
        {
            schedule.Property(s => s.StartDate)
                .HasColumnName("StartDate")
                .IsRequired();

            schedule.Property(s => s.EndDate)
                .HasColumnName("EndDate")
                .IsRequired();
        });

        builder.HasMany(w => w.Enrollments)
            .WithOne(e => e.Workshop)
            .HasForeignKey(e => e.WorkshopId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
