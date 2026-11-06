// Based on code by Ewout Stortenbeker
// CodeEngine Framework (http://www.code-engine.com/)
// Email: 4ewout@gmail.com
// The version used in here has been heavily modified from the original

using System.Diagnostics;

namespace Extenso.Data.QueryBuilder;

/// <summary>
/// Represents a ORDER BY clause to be used with SELECT statements
/// </summary>
[DebuggerDisplay("{FieldName} {SortDirection}")]
public struct OrderByClause
{
    /// <summary>
    /// The field name.
    /// </summary>
    public string FieldName;
    /// <summary>
    /// The sort direction.
    /// </summary>
    public SortDirection SortDirection;

    /// <summary>
    /// Initializes a new instance of the OrderByClause class.
    /// </summary>
    /// <param name="field">The field.</param>
    public OrderByClause(string field)
    {
        FieldName = field;
        SortDirection = SortDirection.Ascending;
    }

    /// <summary>
    /// Initializes a new instance of the OrderByClause class.
    /// </summary>
    /// <param name="field">The field.</param>
    /// <param name="order">The order.</param>
    public OrderByClause(string field, SortDirection order)
    {
        FieldName = field;
        SortDirection = order;
    }
}