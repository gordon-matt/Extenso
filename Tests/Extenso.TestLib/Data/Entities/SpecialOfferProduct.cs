namespace Extenso.TestLib.Data.Entities;

/// <summary>
/// Cross-reference table mapping products to special offer discounts.
/// </summary>
public partial class SpecialOfferProduct
{
    /// <summary>
    /// Primary key for SpecialOfferProduct records.
    /// </summary>
    public int SpecialOfferId { get; set; }

    /// <summary>
    /// Product identification number. Foreign key to Product.ProductID.
    /// </summary>
    public int ProductId { get; set; }

    /// <summary>
    /// ROWGUIDCOL number uniquely identifying the record. Used to support a merge replication sample.
    /// </summary>
    public Guid Rowguid { get; set; }

    /// <summary>
    /// Date and time the record was last updated.
    /// </summary>
    public DateTime ModifiedDate { get; set; }

    public virtual Product Product { get; set; }

    public virtual ICollection<SalesOrderDetail> SalesOrderDetails { get; } = [];

    public virtual SpecialOffer SpecialOffer { get; set; }
}