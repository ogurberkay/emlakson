using Data.Entities.Models;
using Newtonsoft.Json;

namespace Data.Entities.DataTransferObjects.Response;

public class AdvertGetDto
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public HouseTypeEnum? HouseType { get; set; }

    public string HouseTypeString
    {
        get { return HouseType?.ToString(); }
        
    }
    public LocationEnum? Location { get; set; }

    public string LocationString
    {
        get
        {
            return Location?.ToString();
        }
    }
    public string? District { get; set; }
    public AdvertTypeEnum? AdvertType { get; set; }
    [JsonIgnore]
    public string AdvertTypeString
    {
        get { return AdvertType?.ToString(); }
    }
    public int? BedroomNumber { get; set; }
    public int? BathroomNumber { get; set; }
    public int? Meters { get; set; }
    public decimal? Price { get; set; }
    public string? ExtraAttributes { get; set; }
    public bool IsFeatured { get; set; } = false;
    public DateTime CreatedDate { get; set; }
    public string CreatedDateString { get; set; }
    public string? ImageWebPath { get; set; }
}