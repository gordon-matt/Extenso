namespace Extenso.TestLib.Data.Entities;

/// <summary>
/// Individual products associated with a specific purchase order. See PurchaseOrderHeader.
/// </summary>
public partial class PurchaseOrderDetail
{
    /// <summary>
    /// Primary key. Foreign key to PurchaseOrderHeader.PurchaseOrderID.
    /// </summary>
    public int PurchaseOrderId { get; set; }

    /// <summary>
    /// Primary key. One line number per purchased product.
    /// </summary>
    public int PurchaseOrderDetailId { get; set; }

    /// <summary>
    /// Date the product is expected to be received.
    /// </summary>
    public DateTime DueDate { get; set; }

    /// <summary>
    /// Quantity ordered.
    /// </summary>
    public short OrderQty { get; set; }

    /// <summary>
    /// Product identification number. Foreign key to Product.ProductID.
    /// </summary>
    public int ProductId { get; set; }

    /// <summary>
    /// Vendor&apos;s selling price of a single product.
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Per product subtotal. Computed as OrderQty * UnitPrice.
    /// </summary>
    public decimal LineTotal { get; set; }

    /// <summary>
    /// Quantity actually received from the vendor.
    /// </summary>
    public decimal ReceivedQty { get; set; }

    /// <summary>
    /// Quantity rejected during inspection.
    /// </summary>
    public decimal RejectedQty { get; set; }

    /// <summary>
    /// Quantity accepted into inventory. Computed as ReceivedQty - RejectedQty.
    /// </summary>
    public decimal StockedQty { get; set; }

    /// <summary>
    /// Date and time the record was last updated.
    /// </summary>
    public DateTime ModifiedDate { get; set; }

    public virtual Product Product { get; set; }

    public virtual PurchaseOrderHeader PurchaseOrder { get; set; }
}