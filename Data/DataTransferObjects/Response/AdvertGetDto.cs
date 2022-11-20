using Data.Entities.Models;

namespace Data.Entities.DataTransferObjects.Response;

public class AdvertGetDto
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
    public decimal? Price { get; set; }
    public List<ExtraAttributeEnum>? ExtraAttributes { get; set; }
    public bool IsFeatured { get; set; } = false;
}