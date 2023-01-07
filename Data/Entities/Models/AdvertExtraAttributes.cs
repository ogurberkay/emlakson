using Data.Entities.Models;

public class AdvertExtraAttributes
{
    public int ID { get; set; }
    public int AdvertId { get; set; }
    public Advert Advert { get; set; }
    public int ExtraAttributeId { get; set; }
    public ExtraAttribute  ExtraAttribute{ get; set; }
}
