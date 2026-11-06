using System.Diagnostics;

namespace Extenso.Data;

/// <summary>
/// Represents the column info collection.
/// </summary>
[DebuggerDisplay("Count: {Count}")]
public sealed class ColumnInfoCollection : List<ColumnInfo>
{
    /// <summary>
    /// Gets or sets the item at the specified index.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <returns>The item at the specified index.</returns>
    public ColumnInfo this[string name] => this.SingleOrDefault(x => x.ColumnName == name);

    /// <summary>
    /// Determines whether the collection contains the specified value.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <returns>true when the condition is met; otherwise, false.</returns>
    public bool Contains(string name) => this.Any(x => x.ColumnName == name);
}