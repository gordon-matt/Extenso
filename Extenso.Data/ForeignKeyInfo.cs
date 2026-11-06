using System.Diagnostics;

namespace Extenso.Data;

/// <summary>
/// Represents the foreign key info.
/// </summary>
[DebuggerDisplay("{ForeignKeyName}")]
public sealed class ForeignKeyInfo
{
    /// <summary>
    /// Gets or sets the foreign key table.
    /// </summary>
    public string ForeignKeyTable { get; set; }

    /// <summary>
    /// Gets or sets the foreign key column.
    /// </summary>
    public string ForeignKeyColumn { get; set; }

    /// <summary>
    /// Gets or sets the primary key table.
    /// </summary>
    public string PrimaryKeyTable { get; set; }

    /// <summary>
    /// Gets or sets the primary key column.
    /// </summary>
    public string PrimaryKeyColumn { get; set; }

    /// <summary>
    /// Gets or sets the primary key name.
    /// </summary>
    public string PrimaryKeyName { get; set; }

    /// <summary>
    /// Gets or sets the foreign key name.
    /// </summary>
    public string ForeignKeyName { get; set; }

    /// <summary>
    /// Initializes a new instance of the ForeignKeyInfo class.
    /// </summary>
    public ForeignKeyInfo()
    {
    }

    /// <summary>
    /// Initializes a new instance of the ForeignKeyInfo class.
    /// </summary>
    /// <param name="fkTable">The fk table.</param>
    /// <param name="fkColumn">The fk column.</param>
    /// <param name="pkTable">The pk table.</param>
    /// <param name="pkColumn">The pk column.</param>
    /// <param name="primaryKeyName">The primary key name.</param>
    /// <param name="foreignKeyName">The foreign key name.</param>
    public ForeignKeyInfo(string fkTable, string fkColumn, string pkTable, string pkColumn, string primaryKeyName, string foreignKeyName)
    {
        ForeignKeyTable = fkTable;
        ForeignKeyColumn = fkColumn;
        PrimaryKeyTable = pkTable;
        PrimaryKeyColumn = pkColumn;
        PrimaryKeyName = primaryKeyName;
        ForeignKeyName = foreignKeyName;
    }
}