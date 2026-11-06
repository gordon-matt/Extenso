// Based on code by Ewout Stortenbeker
// CodeEngine Framework (http://www.code-engine.com/)
// Email: 4ewout@gmail.com
// The version used in here has been heavily modified from the original

using System.Diagnostics;

namespace Extenso.Data.QueryBuilder;

/// <summary>
/// Represents a JOIN clause to be used with SELECT statements
/// </summary>
[DebuggerDisplay("{JoinType} {FromTable}.{FromColumn} {ComparisonOperator} {ToTable}.{ToColumn}")]
public struct JoinClause
{
    /// <summary>
    /// The join type.
    /// </summary>
    public JoinType JoinType;
    /// <summary>
    /// The from table.
    /// </summary>
    public string FromTable;
    /// <summary>
    /// The from column.
    /// </summary>
    public string FromColumn;
    /// <summary>
    /// The comparison operator.
    /// </summary>
    public ComparisonOperator ComparisonOperator;
    /// <summary>
    /// The to table.
    /// </summary>
    public string ToTable;
    /// <summary>
    /// The to column.
    /// </summary>
    public string ToColumn;

    /// <summary>
    /// Initializes a new instance of the JoinClause class.
    /// </summary>
    /// <param name="join">The join.</param>
    /// <param name="toTableName">The to table name.</param>
    /// <param name="toColumnName">The to column name.</param>
    /// <param name="comparisonOperator">The comparison operator.</param>
    /// <param name="fromTableName">The from table name.</param>
    /// <param name="fromColumnName">The from column name.</param>
    public JoinClause(
        JoinType join,
        string toTableName,
        string toColumnName,
        ComparisonOperator comparisonOperator,
        string fromTableName,
        string fromColumnName)
    {
        JoinType = join;
        FromTable = fromTableName;
        FromColumn = fromColumnName;
        ComparisonOperator = comparisonOperator;
        ToTable = toTableName;
        ToColumn = toColumnName;
    }
}