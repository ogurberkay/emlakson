namespace Data.Common;

public interface IHelperModel
{
    public DateTime DateCreated { get; set; }
    public DateTime? DateModified { get; set; }
    
    public bool IsDeleted { get; set; }
}
/*
 * ModifiedBy
 */