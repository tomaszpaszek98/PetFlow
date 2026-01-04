using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PetFlow.Persistence.Configurations;

public class PetEventConfiguration : IEntityTypeConfiguration<PetEvent>
{
    public void Configure(EntityTypeBuilder<PetEvent> builder)
    {
        builder.HasKey(pe => pe.Id);
        builder.HasQueryFilter(pe => pe.StatusId == 1);
    }
}

