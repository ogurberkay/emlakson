using Data.Common;

namespace Data.Entities.Models;

public class ContactUs:BaseEntity
{
    public string Name { get; set; }
    public decimal Number { get; set; }
    public string Text { get; set; }
}