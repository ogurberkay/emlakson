using Data.Entities.Configuration;
using Data.Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DataAccess.Context;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    DbSet<Advert> Adverts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new AdvertConfiguration());
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=test;User Id=postgres;Password=123123;");
    }   
    /*

Further Updates

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, int clientId = 0, int userId = 0,
        int impersonatedById = 0)
        : this(options)
    {
        this.ClientId = clientId;
        this.UserId = userId;
        this.ImpersonatedById = impersonatedById;
    }







public readonly int ClientId;
public readonly int UserId;
public readonly int ImpersonatedById;


public override int SaveChanges(bool acceptAllChangesOnSuccess)
{
OnBeforeSaving();
return base.SaveChanges(acceptAllChangesOnSuccess);
}

public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
CancellationToken cancellationToken = default)
{
OnBeforeSaving();
return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
}
private void OnBeforeSaving()
{
foreach (var entry in ChangeTracker.Entries()
             .Where(x => x.State == EntityState.Added || x.State == EntityState.Modified))
{
    switch (entry.State)
    {
        case EntityState.Added:
            SetFieldIfExists(entry, "DateCreated", DateTime.Now);

            SetFieldIfExists(entry, "CreatedById", this.UserId);
            SetFieldIfExists(entry, "CreatedByImpId", this.ImpersonatedById);
            break;
        case EntityState.Modified:
            SetFieldIfExists(entry, "DateModified", DateTime.Now);
            SetFieldIfExists(entry, "ModifiedById", this.UserId);
            SetFieldIfExists(entry, "ModifiedByImpId", this.ImpersonatedById);
            break;
    }
}
}
private void SetFieldIfExists(EntityEntry entry, String fieldName, Object value)
{
if (entry.CurrentValues.Properties.Any(c => c.Name == fieldName))
{
    entry.CurrentValues[fieldName] = value;
}
}
*/
}