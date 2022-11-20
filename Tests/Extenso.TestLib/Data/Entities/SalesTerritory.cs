namespace Extenso.TestLib.Data.Entities;

/// <summary>
/// Sales territory lookup table.
/// </summary>
public partial class SalesTerritory
{
    /// <summary>
    /// Primary key for SalesTerritory records.
    /// </summary>
    public int TerritoryId { get; set; }

    /// <summary>
    /// Sales territory description
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// ISO standard country or region code. Foreign key to CountryRegion.CountryRegionCode.
    /// </summary>
    public string CountryRegionCode { get; set; }

    /// <summary>
    /// Geographic area to which the sales territory belong.
    /// </summary>
    public string Group { get; set; }

    /// <summary>
    /// Sales in the territory year to date.
    /// </summary>
    public decimal SalesYtd { get; set; }

    /// <summary>
    /// Sales in the territory the previous year.
    /// </summary>
    public decimal SalesLastYear { get; set; }

    /// <summary>
    /// Business costs in the territory year to date.
    /// </summary>
    public decimal CostYtd { get; set; }

    /// <summary>
    /// Business costs in the territory the previous year.
    /// </summary>
    public decimal CostLastYear { get; set; }

    /// <summary>
    /// ROWGUIDCOL number uniquely identifying the record. Used to support a merge replication sample.
    /// </summary>
    public Guid Rowguid { get; set; }

    /// <summary>
    /// Date and time the record was last updated.
    /// </summary>
    public DateTime ModifiedDate { get; set; }

    public virtual CountryRegion CountryRegionCodeNavigation { get; set; }

    public virtual ICollection<Customer> Customers { get; } = new List<Customer>();

    public virtual ICollection<SalesOrderHeader> SalesOrderHeaders { get; } = new List<SalesOrderHeader>();

    public virtual ICollection<SalesPerson> SalesPeople { get; } = new List<SalesPerson>();

    public virtual ICollection<SalesTerritoryHistory> SalesTerritoryHistories { get; } = new List<SalesTerritoryHistory>();

    public virtual ICollection<StateProvince> StateProvinces { get; } = new List<StateProvince>();
}