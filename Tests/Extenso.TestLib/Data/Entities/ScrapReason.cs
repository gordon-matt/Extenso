namespace Extenso.TestLib.Data.Entities;

/// <summary>
/// Manufacturing failure reasons lookup table.
/// </summary>
public partial class ScrapReason
{
    /// <summary>
    /// Primary key for ScrapReason records.
    /// </summary>
    public short ScrapReasonId { get; set; }

    /// <summary>
    /// Failure description.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Date and time the record was last updated.
    /// </summary>
    public DateTime ModifiedDate { get; set; }

    public virtual ICollection<WorkOrder> WorkOrders { get; } = new List<WorkOrder>();
}