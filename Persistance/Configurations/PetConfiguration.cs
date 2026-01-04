using Domain.Constants;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PetFlow.Persistence.Configurations;

public class PetConfiguration : IEntityTypeConfiguration<Pet>
{
    public void Configure(EntityTypeBuilder<Pet> builder)
    {
        builder.HasKey(p => p.Id);
        builder.HasQueryFilter(p => p.StatusId == 1);
        
        //TODO : Remove default value when authentication is implemented
        builder.Property(p => p.UserId)
            .IsRequired()
            .HasDefaultValue(1);
        
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(EntityConstants.Pet.MaxNameLength);
        builder.Property(p => p.Breed)
            .IsRequired()
            .HasMaxLength(EntityConstants.Pet.MaxBreedLength);
        builder.Property(p => p.Species)
            .IsRequired()
            .HasMaxLength(EntityConstants.Pet.MaxSpeciesLength);
        builder.Property(p => p.DateOfBirth)
            .IsRequired();
        builder.Property(p => p.PhotoUrl)
            .HasMaxLength(EntityConstants.Pet.MaxPhotoUrlLength);
        
        builder.HasMany(n => n.Notes)
            .WithOne(p => p.Pet)
            .HasForeignKey(p => p.PetId);
        
        builder.HasMany(n => n.MedicalNotes)
            .WithOne(p => p.Pet)
            .HasForeignKey(n => n.PetId);

        builder.HasMany(p => p.Events)
            .WithMany()
            .UsingEntity<PetEvent>(
                x => x
                    .HasOne(pe => pe.Event)
                    .WithMany(e => e.PetEvents)
                    .HasForeignKey(pe => pe.EventId),
                x => x
                    .HasOne(pe => pe.Pet)
                    .WithMany()
                    .HasForeignKey(pe => pe.PetId)
            );
    }
}