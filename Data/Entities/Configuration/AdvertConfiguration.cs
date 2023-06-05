using Data.Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using Data.Enum;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion; // Bu satırı ekleyin

namespace Data.Entities.Configuration;

public class AdvertConfiguration : IEntityTypeConfiguration<Advert>
{
    public void Configure(EntityTypeBuilder<Advert> builder)
    {
        builder.HasData(
            new Advert
            {
                Id = 1, Title = "Advert 1", Description = "Advert 1 Description", Price = 1000,
                AdvertType = AdvertTypeEnum.Kiralik
            }
        );
        
        builder
            .Property(e => e.AdvertType)
            .HasConversion(new EnumToStringConverter<AdvertTypeEnum>());
        builder
            .Property(e => e.Location)
            .HasConversion(new EnumToStringConverter<LocationEnum>());
        builder
            .Property(e => e.HouseType)
            .HasConversion(new EnumToStringConverter<HouseTypeEnum>());
        builder
            .Property(e => e.Meters)
            .HasDefaultValue(0);
        builder
            .Property(e => e.Price)
            .HasDefaultValue(0m);

    }
}