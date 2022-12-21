using Data.DataTransferObjects.Request;
using Data.DataTransferObjects.Response;
using Data.Entities.DataTransferObjects.Response;
using Data.Entities.Models;
using System.Xml.Linq;

namespace Business.Extensions;

public static class MappingExtensions
{
    public static AdvertGetDto ToDto(this Advert entity)
    {
        return new AdvertGetDto()
        {
            Id= entity.Id,
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
            CreatedDate = entity.CreatedDate,
            CreatedDateString = entity.CreatedDate.ToString("dddd, dd MMMM yyyy HH:mm:ss"),
            Title = entity.Title
        };
    }
    public static UserGetDto ToDto(this UserEntity entity)
    {
        return new UserGetDto()
        {
            Name = entity.Name,
            UserName = entity.UserName,
            Surname = entity.Surname,
            Email= entity.Email,
            PhoneNumber= entity.PhoneNumber,
            CreatedDate= entity.CreatedDate,
        };
    }
}