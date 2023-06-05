using Data.Entities.Models;
using Data.Enum;

namespace Data.DataTransferObjects.Request;

public class AddAdvertRequestDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public HouseTypeEnum? HouseType { get; set; }
    public LocationEnum? Location { get; set; }
    public string? District { get; set; }
    public AdvertTypeEnum? AdvertType { get; set; }
    public int? BedroomNumber { get; set; }
    public int? BathroomNumber { get; set; }
    public int? Meters { get; set; }
    public int? Price { get; set; }
    public List<ExtraAttributeEnum>? ExtraAttributes { get; set; }

}