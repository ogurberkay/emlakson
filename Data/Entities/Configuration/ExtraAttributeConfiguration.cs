using Data.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Entities.Configuration;

public class ExtraAttributeConfiguration : IEntityTypeConfiguration<ExtraAttribute>
{
    public void Configure(EntityTypeBuilder<ExtraAttribute> builder)
    {
        builder.Property(b => b.AttributeName).IsRequired();

        builder.HasData(
            new ExtraAttribute { Id = 1, AttributeName = "Kombi" },
            new ExtraAttribute { Id = 2, AttributeName = "Havuz" },
            new ExtraAttribute { Id = 3, AttributeName = "MerkeziIsıtma" },
            new ExtraAttribute { Id = 4, AttributeName = "Alarm" },
            new ExtraAttribute { Id = 5, AttributeName = "Wifi" },
            new ExtraAttribute { Id = 6, AttributeName = "Garaj" },
            new ExtraAttribute { Id = 7, AttributeName = "Jakuzi" },
            new ExtraAttribute { Id = 8, AttributeName = "Somine" }
            // Bu şekilde devam edebilirsiniz
        );
    }
}