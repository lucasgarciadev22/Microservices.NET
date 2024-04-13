namespace Ordering.Core.Common;

public class EntityBase
{
    public int Id { get; protected set; }

    //Audit properties
    public string? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string? LastModifiedBy { get; set; }
    public DateTime? LastModifiedDate { get; set; }
}
