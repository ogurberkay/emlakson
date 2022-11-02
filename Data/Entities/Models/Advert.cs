using Data.Common;

namespace Data.Entities.Models;

public class Advert : BaseEntity
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
    public List<string>? ExtraAttributes { get; set; }
    public string AdvertDescription { get; set; }
}

public enum AdvertTypeEnum
{
    Satilik = 1,
    Kiralik = 2
}

public enum LocationEnum
{
    Istanbul = 1,
    Izmir = 2,
    Mersin = 3,
}

public enum HouseTypeEnum
{
    Mustakil = 1,
    Apartman = 2
}

/*
İlan Türü
ilanın parası
ilan Kayıt eden kisi
ilan linki
ilan Açıklaması
ilan konumu(il ilçe açıkadress:string)

 */