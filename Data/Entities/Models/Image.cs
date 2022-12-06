using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

public class Image
{
    [Key]
    public int ImageId { get; set; }

    [Column(TypeName = "varchar(50)")]
    public string Title { get; set; }

    [Column(TypeName = "varchar(100)")]
    [DisplayName("Image Name")]
    public string ImageName { get; set; }

    [NotMapped]
    [DisplayName("Upload File")]
    public IFormFile ImageFile { get; set; }
}