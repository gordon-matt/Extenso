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
    public string FieldName;
    public SortDirection SortDirection;

    public OrderByClause(string field)
    {
        FieldName = field;
        SortDirection = SortDirection.Ascending;
    }

    public OrderByClause(string field, SortDirection order)
    {
        FieldName = field;
        SortDirection = order;
    }
}