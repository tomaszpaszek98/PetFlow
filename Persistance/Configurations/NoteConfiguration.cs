using Domain.Constants;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PetFlow.Persistence.Configurations;

public class NoteConfiguration : IEntityTypeConfiguration<Note>
{
    public void Configure(EntityTypeBuilder<Note> builder)
    {
        builder.HasKey(n => n.Id);
        builder.HasQueryFilter(n => n.StatusId == 1);
        
        builder.Property(n => n.Content)
            .IsRequired()
            .HasMaxLength(EntityConstants.Note.MaxContentLength);
        builder.Property(n => n.Type)
            .IsRequired()
            .HasMaxLength(EntityConstants.Note.MaxTypeLength);
    }
}