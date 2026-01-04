using Domain.Constants;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PetFlow.Persistence.Configurations;

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasQueryFilter(e => e.StatusId == 1);
        
        builder.Property(e => e.Title)
            .IsRequired()
            .HasMaxLength(EntityConstants.Event.MaxTitleLength);
        builder.Property(e => e.Description)
            .IsRequired()
            .HasMaxLength(EntityConstants.Event.MaxDescriptionLength);
        builder.Property(e => e.DateOfEvent)
            .IsRequired();
        builder.Property(e => e.Reminder)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasMany(e => e.PetEvents)
            .WithOne(pe => pe.Event)
            .HasForeignKey(pe => pe.EventId);
    }
}