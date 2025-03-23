namespace Extenso.TestLib.Data.Entities;

/// <summary>
/// Lookup table containing the ISO standard codes for countries and regions.
/// </summary>
public partial class CountryRegion
{
    /// <summary>
    /// ISO standard code for countries and regions.
    /// </summary>
    public string CountryRegionCode { get; set; }

    /// <summary>
    /// Country or region name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Date and time the record was last updated.
    /// </summary>
    public DateTime ModifiedDate { get; set; }

    public virtual ICollection<CountryRegionCurrency> CountryRegionCurrencies { get; } = [];

    public virtual ICollection<SalesTerritory> SalesTerritories { get; } = [];

    public virtual ICollection<StateProvince> StateProvinces { get; } = [];
}