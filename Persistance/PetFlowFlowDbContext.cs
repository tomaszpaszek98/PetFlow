using System.Reflection;
using Application.Common.Interfaces;
using Domain.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace PetFlow.Persistence;

public class PetFlowDbContext : DbContext, IPetDbContext
{
    public DbSet<Event> Events { get; set; }
    public DbSet<MedicalNote> MedicalNotes { get; set; }
    public DbSet<Pet> Pets { get; set; }
    public DbSet<PetEvent> PetEvents { get; set; }
    
    private readonly IDateTime _dateTime;
    private readonly ICurrentUserService _userService;
    
    public PetFlowDbContext(DbContextOptions<PetFlowDbContext> options) : base(options)
    {
    }
    
    public PetFlowDbContext(DbContextOptions<PetFlowDbContext> options, IDateTime dateTime, ICurrentUserService userService ) : base(options)
    {
        _dateTime = dateTime;
        _userService = userService;
    }

    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder.SeedData();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            switch(entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = _userService.Email;
                    entry.Entity.Created = _dateTime.Now;
                    entry.Entity.StatusId = 1;
                    break;
                case EntityState.Modified:
                    entry.Entity.ModifiedBy = _userService.Email;
                    entry.Entity.Modified = _dateTime.Now;
                    break;
                case EntityState.Deleted:
                    entry.Entity.ModifiedBy = _userService.Email;
                    entry.Entity.Modified = _dateTime.Now;
                    entry.Entity.Inactivated = _dateTime.Now;
                    entry.Entity.InactivatedBy = _userService.Email;
                    entry.Entity.StatusId = 0;
                    entry.State = EntityState.Modified;
                    break;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}