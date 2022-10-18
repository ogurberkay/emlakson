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
            new Advert() {Id = 1, AdvertDescription = "Description1"},
            new Advert() {Id = 2, AdvertDescription = "Description2"},
            new Advert() {Id = 3, AdvertDescription = "Description3"},
            new Advert() {Id = 4, AdvertDescription = "Description4"},
            new Advert() {Id = 5, AdvertDescription = "Description5"},
            new Advert() {Id = 6, AdvertDescription = "Description6"},
            new Advert() {Id = 7, AdvertDescription = "Description7"});
    }
}