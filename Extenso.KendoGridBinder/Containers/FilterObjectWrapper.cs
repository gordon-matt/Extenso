namespace Extenso.KendoGridBinder.Containers;

/// <summary>
/// Represents the filter object wrapper.
/// </summary>
public class FilterObjectWrapper
{
    /// <summary>
    /// Gets or sets the logic.
    /// </summary>
    public string Logic { get; set; }

    /// <summary>
    /// Gets or sets the filter objects.
    /// </summary>
    public IEnumerable<FilterObject> FilterObjects { get; set; }

    /// <summary>
    /// Gets the logic token.
    /// </summary>
    public string LogicToken => Logic switch
    {
        "and" => "&&",
        "or" => "||",
        _ => null,
    };
}