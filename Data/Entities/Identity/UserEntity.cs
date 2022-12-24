using Data.Common;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities.Models;

public class UserEntity:IdentityUser, IEntity
{
    //TODO make nullable
    public int Id { get; set; }
    [Required] public string Name { get; set; }
    [Required] public string Surname { get; set; }
    public int ModifiedById { get; set; }
    public int? CreatedById { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public bool IsDeleted { get; set; }
}