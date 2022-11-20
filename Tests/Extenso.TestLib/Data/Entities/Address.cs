namespace Extenso.TestLib.Data.Entities;

/// <summary>
/// Street address information for customers, employees, and vendors.
/// </summary>
public partial class Address
{
    /// <summary>
    /// Primary key for Address records.
    /// </summary>
    public int AddressId { get; set; }

    /// <summary>
    /// First street address line.
    /// </summary>
    public string AddressLine1 { get; set; }

    /// <summary>
    /// Second street address line.
    /// </summary>
    public string AddressLine2 { get; set; }

    /// <summary>
    /// Name of the city.
    /// </summary>
    public string City { get; set; }

    /// <summary>
    /// Unique identification number for the state or province. Foreign key to StateProvince table.
    /// </summary>
    public int StateProvinceId { get; set; }

    /// <summary>
    /// Postal code for the street address.
    /// </summary>
    public string PostalCode { get; set; }

    /// <summary>
    /// ROWGUIDCOL number uniquely identifying the record. Used to support a merge replication sample.
    /// </summary>
    public Guid Rowguid { get; set; }

    /// <summary>
    /// Date and time the record was last updated.
    /// </summary>
    public DateTime ModifiedDate { get; set; }

    public virtual ICollection<BusinessEntityAddress> BusinessEntityAddresses { get; } = new List<BusinessEntityAddress>();

    public virtual ICollection<SalesOrderHeader> SalesOrderHeaderBillToAddresses { get; } = new List<SalesOrderHeader>();

    public virtual ICollection<SalesOrderHeader> SalesOrderHeaderShipToAddresses { get; } = new List<SalesOrderHeader>();

    public virtual StateProvince StateProvince { get; set; }
}