using System.ComponentModel.DataAnnotations;

namespace Data.Common;

public interface IEntity : IHelperModel
{
    [Key] public int Id { get; set; }
}