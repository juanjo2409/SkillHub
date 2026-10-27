using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillHub.Domain.Entities;
using SkillHub.Domain.ValueObjects;

namespace SkillHub.Infrastructure.Persistence.Configurations;

public class InstructorConfiguration : IEntityTypeConfiguration<Instructor>
{
    public void Configure(EntityTypeBuilder<Instructor> builder)
    {
        builder.ToTable("Instructors");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.FullName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(i => i.Email)
            .HasConversion(
                email => email.Value,
                value => new Email(value))
            .IsRequired()
            .HasMaxLength(150);

        builder.HasIndex(i => i.Email)
            .IsUnique();

        builder.Property(i => i.Specialty)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(i => i.IdentityUserId)
            .HasMaxLength(450);

        builder.HasMany(i => i.Workshops)
            .WithOne(w => w.Instructor)
            .HasForeignKey(w => w.InstructorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
