namespace Data.Common;

public interface IHelperModel
{
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public bool IsDeleted { get; set; }
}
/*
 * ModifiedBy
 */