namespace Extenso.TestLib.Data.Entities;

/// <summary>
/// Sales representative current information.
/// </summary>
public partial class SalesPerson
{
    /// <summary>
    /// Primary key for SalesPerson records. Foreign key to Employee.BusinessEntityID
    /// </summary>
    public int BusinessEntityId { get; set; }

    /// <summary>
    /// Territory currently assigned to. Foreign key to SalesTerritory.SalesTerritoryID.
    /// </summary>
    public int? TerritoryId { get; set; }

    /// <summary>
    /// Projected yearly sales.
    /// </summary>
    public decimal? SalesQuota { get; set; }

    /// <summary>
    /// Bonus due if quota is met.
    /// </summary>
    public decimal Bonus { get; set; }

    /// <summary>
    /// Commision percent received per sale.
    /// </summary>
    public decimal CommissionPct { get; set; }

    /// <summary>
    /// Sales total year to date.
    /// </summary>
    public decimal SalesYtd { get; set; }

    /// <summary>
    /// Sales total of previous year.
    /// </summary>
    public decimal SalesLastYear { get; set; }

    /// <summary>
    /// ROWGUIDCOL number uniquely identifying the record. Used to support a merge replication sample.
    /// </summary>
    public Guid Rowguid { get; set; }

    /// <summary>
    /// Date and time the record was last updated.
    /// </summary>
    public DateTime ModifiedDate { get; set; }

    public virtual Employee BusinessEntity { get; set; }

    public virtual ICollection<SalesOrderHeader> SalesOrderHeaders { get; } = [];

    public virtual ICollection<SalesPersonQuotaHistory> SalesPersonQuotaHistories { get; } = [];

    public virtual ICollection<SalesTerritoryHistory> SalesTerritoryHistories { get; } = [];

    public virtual ICollection<Store> Stores { get; } = [];

    public virtual SalesTerritory Territory { get; set; }
}