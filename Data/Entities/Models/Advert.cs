using Data.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using Data.Enum;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Data.Entities.Models;

public class Advert : BaseEntity
{
    public string? Title { get; set; }
    public bool IsSelectedForMail { get; set; }
    public string? Description { get; set; }
    public HouseTypeEnum? HouseType { get; set; }
    public LocationEnum? Location { get; set; }
    public string? District { get; set; }
    public AdvertTypeEnum? AdvertType { get; set; }
    public int? BedroomNumber { get; set; }
    public int? BathroomNumber { get; set; }
    public int? Meters { get; set; } = 0;
    public decimal? Price { get; set; }
    public virtual ICollection<AdvertExtraAttributes> AdvertExtraAttributes { get; set; }
    public bool IsFeatured { get; set; } = false;
    public Image? ImageFile { get; set; }
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