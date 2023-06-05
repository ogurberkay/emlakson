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
            new ExtraAttribute { Id = 8, AttributeName = "Somine" },
            new ExtraAttribute { Id = 9, AttributeName = "Balkon" },
            new ExtraAttribute { Id = 10, AttributeName = "Çamaşır Makinesi" },
            new ExtraAttribute { Id = 11, AttributeName = "Bulaşık Makinesi" },
            new ExtraAttribute { Id = 12, AttributeName = "Bahçe" },
            new ExtraAttribute { Id = 13, AttributeName = "Arazi" },
            new ExtraAttribute { Id = 14, AttributeName = "Asansör" },
            new ExtraAttribute { Id = 15, AttributeName = "Güvenlik Kamerası" },
            new ExtraAttribute { Id = 16, AttributeName = "Fitness Salonu" },
            new ExtraAttribute { Id = 17, AttributeName = "Otopark" },
            new ExtraAttribute { Id = 18, AttributeName = "Ebeveyn Banyosu" },
            new ExtraAttribute { Id = 19, AttributeName = "Şömine" },
            new ExtraAttribute { Id = 20, AttributeName = "Manzara" },
            new ExtraAttribute { Id = 21, AttributeName = "Güvenlik Sistemi" },
            new ExtraAttribute { Id = 22, AttributeName = "Çift Cam" },
            new ExtraAttribute { Id = 23, AttributeName = "Jeneratör" },
            new ExtraAttribute { Id = 24, AttributeName = "Tenis Kortu" },
            new ExtraAttribute { Id = 25, AttributeName = "Kapalı Yüzme Havuzu" },
            new ExtraAttribute { Id = 26, AttributeName = "Klima" },
            new ExtraAttribute { Id = 27, AttributeName = "Müstakil" },
            new ExtraAttribute { Id = 28, AttributeName = "Deniz Manzarası" },
            new ExtraAttribute { Id = 29, AttributeName = "Dağ Manzarası" },
            new ExtraAttribute { Id = 30, AttributeName = "Göl Manzarası" },
            new ExtraAttribute { Id = 31, AttributeName = "Şehir Manzarası" }

        );
    }
}