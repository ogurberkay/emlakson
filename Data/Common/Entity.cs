using System.ComponentModel.DataAnnotations;

namespace Data.Common;

public class BaseEntity :IEntity
{
    public int Id { get; set; }

    public int ModifiedById { get; set; }
    public int? CreatedByImpId { get; set; }
    public int? ModifiedByImpId { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime? DateModified { get; set; }
    public bool IsDeleted { get; set; }
}