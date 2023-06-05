using Data.Entities.Models;

public class CustomerExtraAttributes
{
    public int ID { get; set; }
    public int CustomerId { get; set; }
    public Customer Customer { get; set; }
    public int ExtraAttributeId { get; set; }
    public ExtraAttribute  ExtraAttribute{ get; set; }
}
