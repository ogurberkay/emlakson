using Data.Common;
using Data.Constants;
using Data.Entities.Configuration;
using Data.Entities.Identity;
using Data.Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using System.Runtime.Serialization;

namespace DataAccess.Context;

public class ApplicationDbContext : IdentityDbContext<UserEntity,IdentityRole<int>,int>
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<Advert> Adverts { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<ExtraAttribute> ExtraAttributess { get; set; }
    public DbSet<AdvertExtraAttributes> AdvertExtraAttributess { get; set; }
    public DbSet<GalleryImage> GalleryImages { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //modelBuilder.Entity<Advert>().HasQueryFilter(p => !p.IsDeleted);
        //modelBuilder.Entity<UserEntity>().HasQueryFilter(p => !p.IsDeleted);



        modelBuilder.ApplyConfiguration(new AdvertConfiguration());
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new ExtraAttributeConfiguration());

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

        //optionsBuilder.UseSqlServer("Data Source=DESKTOP-SVIS1U8;Initial Catalog=TestDb;Integrated Security=true");
        optionsBuilder.UseSqlServer("Data Source=DESKTOP-9SDA81J;Initial Catalog=TestDb;Integrated Security=true");
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;

        foreach (var changedEntity in ChangeTracker.Entries())
        {
            if (changedEntity.Entity is IEntity entity)
            {
                switch (changedEntity.State)
                {
                    case EntityState.Added:
                        entity.CreatedDate = now;
                        entity.UpdatedDate = now;
                        break;

                    case EntityState.Modified:
                        entity.UpdatedDate = now;
                        break;
                }
            }
        }

        return base.SaveChangesAsync();
    }
    public override int SaveChanges()
    {
        var now = DateTime.UtcNow;

        foreach (var changedEntity in ChangeTracker.Entries())
        {
            if (changedEntity.Entity is IEntity entity)
            {
                switch (changedEntity.State)
                {
                    case EntityState.Added:
                        entity.CreatedDate = now;
                        entity.UpdatedDate = now;
                        break;

                    case EntityState.Modified:
                        Entry(entity).Property(x => x.CreatedDate).IsModified = false;
                        entity.UpdatedDate = now;
                        break;
                }
            }
        }

        return base.SaveChanges();
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

