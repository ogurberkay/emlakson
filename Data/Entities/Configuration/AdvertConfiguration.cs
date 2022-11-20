using Data.Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Entities.Configuration;

public class AdvertConfiguration : IEntityTypeConfiguration<Advert>
{
    public void Configure(EntityTypeBuilder<Advert> builder)
    {
        builder.HasData(
            new Advert() { Id = 1, AdvertType = AdvertTypeEnum.Kiralik, BathroomNumber = 2, BedroomNumber = 1, Title = "Ev title", Price = 230000, HouseType = HouseTypeEnum.Mustakil, Description = "testtest", District = "District", Location = LocationEnum.Istanbul, ExtraAttributes = new List<ExtraAttributeEnum>() { ExtraAttributeEnum.Kombi, ExtraAttributeEnum.Havuz, ExtraAttributeEnum.MerkeziIsıtma }, Meters = 500, IsDeleted = false },
            new Advert() { Id = 2, AdvertType = AdvertTypeEnum.Kiralik, BathroomNumber = 2, BedroomNumber = 1, Title = "Ev title", Price = 230000, HouseType = HouseTypeEnum.Mustakil, Description = "testtest", District = "District", Location = LocationEnum.Istanbul, ExtraAttributes = new List<ExtraAttributeEnum>() { ExtraAttributeEnum.Kombi, ExtraAttributeEnum.Havuz, ExtraAttributeEnum.MerkeziIsıtma }, Meters = 500, IsDeleted = false },
            new Advert() { Id = 3, AdvertType = AdvertTypeEnum.Kiralik, BathroomNumber = 2, BedroomNumber = 1, Title = "Ev title", Price = 230000, HouseType = HouseTypeEnum.Mustakil, Description = "testtest", District = "District", Location = LocationEnum.Istanbul, ExtraAttributes = new List<ExtraAttributeEnum>() { ExtraAttributeEnum.Kombi, ExtraAttributeEnum.Havuz, ExtraAttributeEnum.MerkeziIsıtma }, Meters = 500, IsDeleted = false },
            new Advert() { Id = 4, AdvertType = AdvertTypeEnum.Kiralik, BathroomNumber = 2, BedroomNumber = 1, Title = "Ev title", Price = 230000, HouseType = HouseTypeEnum.Mustakil, Description = "testtest", District = "District", Location = LocationEnum.Istanbul, ExtraAttributes = new List<ExtraAttributeEnum>() { ExtraAttributeEnum.Kombi, ExtraAttributeEnum.Havuz, ExtraAttributeEnum.MerkeziIsıtma }, Meters = 500, IsDeleted = false },
            new Advert() { Id = 5, AdvertType = AdvertTypeEnum.Kiralik, BathroomNumber = 2, BedroomNumber = 1, Title = "Ev title", Price = 230000, HouseType = HouseTypeEnum.Mustakil, Description = "testtest", District = "District", Location = LocationEnum.Istanbul, ExtraAttributes = new List<ExtraAttributeEnum>() { ExtraAttributeEnum.Kombi, ExtraAttributeEnum.Havuz, ExtraAttributeEnum.MerkeziIsıtma }, Meters = 500, IsDeleted = false },
            new Advert() { Id = 6, AdvertType = AdvertTypeEnum.Kiralik, BathroomNumber = 2, BedroomNumber = 1, Title = "Ev title", Price = 230000, HouseType = HouseTypeEnum.Mustakil, Description = "testtest", District = "District", Location = LocationEnum.Istanbul, ExtraAttributes = new List<ExtraAttributeEnum>() { ExtraAttributeEnum.Kombi, ExtraAttributeEnum.Havuz, ExtraAttributeEnum.MerkeziIsıtma }, Meters = 500, IsDeleted = false },
            new Advert() { Id = 7, AdvertType = AdvertTypeEnum.Kiralik, BathroomNumber = 2, BedroomNumber = 1, Title = "Ev title", Price = 230000, HouseType = HouseTypeEnum.Mustakil, Description = "testtest", District = "District", Location = LocationEnum.Istanbul, ExtraAttributes = new List<ExtraAttributeEnum>() { ExtraAttributeEnum.Kombi, ExtraAttributeEnum.Havuz, ExtraAttributeEnum.MerkeziIsıtma }, Meters = 500, IsDeleted = false },
            new Advert() { Id = 8, AdvertType = AdvertTypeEnum.Kiralik, BathroomNumber = 2, BedroomNumber = 1, Title = "Ev title", Price = 230000, HouseType = HouseTypeEnum.Mustakil, Description = "testtest", District = "District", Location = LocationEnum.Istanbul, ExtraAttributes = new List<ExtraAttributeEnum>() { ExtraAttributeEnum.Kombi, ExtraAttributeEnum.Havuz, ExtraAttributeEnum.MerkeziIsıtma }, Meters = 500, IsDeleted = false });
    }
}