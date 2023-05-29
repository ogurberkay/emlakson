using Data.Common;

namespace Data.Entities.Models;

public class GalleryImage : BaseEntity
{
    public Image ImageFile { get; set; }
    public string ImageName { get; set; }
    public decimal? Size { get; set; }

    public string CreatedDateString => CreatedDate.ToString("dddd, dd MMMM yyyy HH:mm:ss");
}