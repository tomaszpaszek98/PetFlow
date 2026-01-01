using Domain.Constants;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PetFlow.Persistence.Configurations;

public class MedicalNoteConfiguration : IEntityTypeConfiguration<MedicalNote>
{
    public void Configure(EntityTypeBuilder<MedicalNote> builder)
    {
        builder.HasKey(n => n.Id);
        builder.HasQueryFilter(n => n.StatusId == 1);
        
        builder.Property(n => n.Title)
            .IsRequired()
            .HasMaxLength(EntityConstants.MedicalNote.MaxTitleLength);
        builder.Property(n => n.Description)
            .IsRequired()
            .HasMaxLength(EntityConstants.MedicalNote.MaxDescriptionLength);
    }
}