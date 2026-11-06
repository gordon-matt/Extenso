namespace Extenso.KendoGridBinder;

/// <summary>
/// KendoGroup
///
/// Properties are lowercase to support Kendo UI Grid
/// </summary>
public class KendoGroup
{
    // ReSharper disable InconsistentNaming
    /// <summary>
    /// Gets or sets the field.
    /// </summary>
    public string field { get; set; }

    /// <summary>
    /// Gets or sets the aggregates.
    /// </summary>
    public object aggregates { get; set; }
    /// <summary>
    /// Gets or sets the items.
    /// </summary>
    public object items { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether this instance is has subgroups.
    /// </summary>
    public bool hasSubgroups { get; set; }
    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    public object value { get; set; }
    // ReSharper restore InconsistentNaming
}