using System.Data;
using System.Diagnostics;

namespace Extenso.Data;

// TODO: Should take into account that sometimes a column can be both a primary and foreign key (composite keys)
/// <summary>
/// Specifies the key type.
/// </summary>
public enum KeyType : byte
{
    /// <summary>
    /// The none value.
    /// </summary>
    None = 0,
    /// <summary>
    /// The primary key value.
    /// </summary>
    PrimaryKey = 1,
    /// <summary>
    /// The foreign key value.
    /// </summary>
    ForeignKey = 2
}

/// <summary>
/// Represents the column info.
/// </summary>
[DebuggerDisplay("{ColumnName} ({DataTypeNative})")]
public class ColumnInfo
{
    /// <summary>
    /// Gets or sets the column name.
    /// </summary>
    public string ColumnName { get; set; }

    /// <summary>
    /// Gets or sets the ordinal position.
    /// </summary>
    public int OrdinalPosition { get; set; }

    /// <summary>
    /// Gets or sets the default value.
    /// </summary>
    public string DefaultValue { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is nullable.
    /// </summary>
    public bool IsNullable { get; set; }

    /// <summary>
    /// Gets or sets the data type.
    /// </summary>
    public DbType DataType { get; set; }

    /// <summary>
    /// Gets or sets the data type native.
    /// </summary>
    public string DataTypeNative { get; set; }

    /// <summary>
    /// Gets or sets the maximum length.
    /// </summary>
    public long MaximumLength { get; set; }

    /// <summary>
    /// Gets or sets the precision.
    /// </summary>
    public int Precision { get; set; }

    /// <summary>
    /// Gets or sets the scale.
    /// </summary>
    public int Scale { get; set; }

    /// <summary>
    /// Gets or sets the key type.
    /// </summary>
    public KeyType KeyType { get; set; }

    /// <summary>
    /// MSSQL: IDENTITY, PG: Sequence, etc..
    /// </summary>
    public bool IsAutoIncremented { get; set; }
}