using System.Diagnostics;

namespace Extenso.Data.QueryBuilder;

/// <summary>
/// Represents the SQL literal.
/// </summary>
[DebuggerDisplay("{Value}")]
public class SqlLiteral
{
    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    /// Initializes a new instance of the SqlLiteral class.
    /// </summary>
    /// <param name="value">The value.</param>
    public SqlLiteral(string value)
    {
        Value = value;
    }
}