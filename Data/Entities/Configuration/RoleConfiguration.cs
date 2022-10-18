using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Entities.Configuration;

public class RoleConfiguration: IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        builder.HasData(
            new IdentityRole()
            {
                Name = "Admin",
                NormalizedName = "ADMIN"
            },
            new IdentityRole()
            {
                Name = "SuperAdmin",
                NormalizedName = "SUPER_ADMIN"
            });
    }
}