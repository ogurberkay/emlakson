using Data.DataTransferObjects.Request;
using Data.Entities.DataTransferObjects.Response;
using Data.Entities.Models;

namespace Business.Extensions;

public static class MappingExtensions
{
    public static AdvertGetDto ToDto(this Advert entity)
    {
        return new AdvertGetDto()
        {
            AdvertType = entity.AdvertType,
            BathroomNumber = entity.BathroomNumber,
            BedroomNumber = entity.BedroomNumber,
            Description = entity.Description,
            District = entity.District,
            ExtraAttributes = entity.ExtraAttributes,
            HouseType = entity.HouseType,
            Location = entity.Location,
            Meters = entity.Meters,
            Price = entity.Price,
            Title = entity.Title
        };
    }
}