using Data.Common;
using Data.Enum;

namespace Data.Entities.Models;

public class Customer:BaseEntity
{
    public HouseTypeEnum? HouseType { get; set; }
    public LocationEnum? Location { get; set; }
    public string? District { get; set; }
    public AdvertTypeEnum? AdvertType { get; set; }
    public int? BedroomNumber { get; set; }
    public int? BathroomNumber { get; set; }
    public int? Meters { get; set; } = 0;
    public decimal? Price { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public virtual ICollection<CustomerExtraAttributes> CustomerExtraAttributes { get; set; }
    public bool IsSelectedForMail { get; set; }


}