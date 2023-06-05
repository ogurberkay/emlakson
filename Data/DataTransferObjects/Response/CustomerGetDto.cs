using Data.Entities.Models;
using Data.Enum;
using Newtonsoft.Json;

namespace Data.DataTransferObjects.Response;


public class CustomerGetDto
{
    public int Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }

    public string Name { get; set; }
    public string Surname { get; set; }
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
    public AdvertTypeEnum? AdvertType { get; set; }
    [JsonIgnore]
    public string AdvertTypeString
    {
        get { return AdvertType?.ToString(); }
    }
    public string? District { get; set; }
    public int? BedroomNumber { get; set; }
    public int? BathroomNumber { get; set; }
    public int? Meters { get; set; } = 0;
    public decimal? Price { get; set; }
    public string? ExtraAttributes { get; set; }
    public string CreatedDateString { get; set; }
}
