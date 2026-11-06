namespace Extenso.KendoGridBinder.Containers;

/// <summary>
/// Represents the sort object.
/// </summary>
public class SortObject
{
    /// <summary>
    /// Initializes a new instance of the SortObject class.
    /// </summary>
    /// <param name="field">The field.</param>
    /// <param name="direction">The direction.</param>
    public SortObject(string field, string direction)
    {
        Field = field;
        Direction = direction;
    }

    /// <summary>
    /// Gets or sets the field.
    /// </summary>
    public string Field { get; set; }
    /// <summary>
    /// Gets or sets the direction.
    /// </summary>
    public string Direction { get; set; }
}