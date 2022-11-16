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
    public List<ExtraAttributeEnum>? ExtraAttributes { get; set; }
    public string AdvertDescription { get; set; }
}

public enum ExtraAttributeEnum
{
    Kombi,
    Havuz,
    MerkeziIsıtma,
    Alarm,
    Wifi,
    Garaj,
    Jakuzi,
    Somine,
    EbeveynBanyosu,
    CocukOdasi
}

public enum AdvertTypeEnum
{
    Satilik = 2,
    Kiralik = 3
}

public enum LocationEnum
{
    Istanbul = 2,
    Izmir = 3,
    Mersin = 4,
}

public enum HouseTypeEnum
{
    Mustakil = 2,
    Apartman = 3
}

/*
İlan Türü
ilanın parası
ilan Kayıt eden kisi
ilan linki
ilan Açıklaması
ilan konumu(il ilçe açıkadress:string)

 */