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
            AdvertDescription = entity.AdvertDescription
        };
    }
}