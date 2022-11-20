namespace Extenso.TestLib.Data.Entities;

/// <summary>
/// Cross-reference table mapping sales orders to sales reason codes.
/// </summary>
public partial class SalesOrderHeaderSalesReason
{
    /// <summary>
    /// Primary key. Foreign key to SalesOrderHeader.SalesOrderID.
    /// </summary>
    public int SalesOrderId { get; set; }

    /// <summary>
    /// Primary key. Foreign key to SalesReason.SalesReasonID.
    /// </summary>
    public int SalesReasonId { get; set; }

    /// <summary>
    /// Date and time the record was last updated.
    /// </summary>
    public DateTime ModifiedDate { get; set; }

    public virtual SalesOrderHeader SalesOrder { get; set; }

    public virtual SalesReason SalesReason { get; set; }
}