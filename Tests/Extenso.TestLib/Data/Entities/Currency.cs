namespace Extenso.TestLib.Data.Entities;

/// <summary>
/// Lookup table containing standard ISO currencies.
/// </summary>
public partial class Currency
{
    /// <summary>
    /// The ISO code for the Currency.
    /// </summary>
    public string CurrencyCode { get; set; }

    /// <summary>
    /// Currency name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Date and time the record was last updated.
    /// </summary>
    public DateTime ModifiedDate { get; set; }

    public virtual ICollection<CountryRegionCurrency> CountryRegionCurrencies { get; } = [];

    public virtual ICollection<CurrencyRate> CurrencyRateFromCurrencyCodeNavigations { get; } = [];

    public virtual ICollection<CurrencyRate> CurrencyRateToCurrencyCodeNavigations { get; } = [];
}