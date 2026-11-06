using System.Diagnostics;

namespace Extenso.Data;

/// <summary>
/// Represents the foreign key info collection.
/// </summary>
[DebuggerDisplay("Count: {Count}")]
public sealed class ForeignKeyInfoCollection : List<ForeignKeyInfo>
{
    /// <summary>
    /// Gets or sets the item at the specified index.
    /// </summary>
    /// <param name="fkColumnName">The fk column name.</param>
    /// <returns>The item at the specified index.</returns>
    public ForeignKeyInfo this[string fkColumnName] => this.SingleOrDefault(x => x.ForeignKeyColumn == fkColumnName);

    /// <summary>
    /// Determines whether the collection contains the specified value.
    /// </summary>
    /// <param name="fkColumnName">The fk column name.</param>
    /// <returns>true when the condition is met; otherwise, false.</returns>
    public bool Contains(string fkColumnName) => this.Any(x => x.ForeignKeyColumn == fkColumnName);
}