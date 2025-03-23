namespace Extenso.TestLib.Data.Entities;

/// <summary>
/// Source of the ID that connects vendors, customers, and employees with address and contact information.
/// </summary>
public partial class BusinessEntity
{
    /// <summary>
    /// Primary key for all customers, vendors, and employees.
    /// </summary>
    public int BusinessEntityId { get; set; }

    /// <summary>
    /// ROWGUIDCOL number uniquely identifying the record. Used to support a merge replication sample.
    /// </summary>
    public Guid Rowguid { get; set; }

    /// <summary>
    /// Date and time the record was last updated.
    /// </summary>
    public DateTime ModifiedDate { get; set; }

    public virtual ICollection<BusinessEntityAddress> BusinessEntityAddresses { get; } = [];

    public virtual ICollection<BusinessEntityContact> BusinessEntityContacts { get; } = [];

    public virtual Person Person { get; set; }

    public virtual Store Store { get; set; }

    public virtual Vendor Vendor { get; set; }
}