// Based on code by Ewout Stortenbeker
// CodeEngine Framework (http://www.code-engine.com/)
// Email: 4ewout@gmail.com
// The version used in here has been heavily modified from the original

namespace Extenso.Data.QueryBuilder;

/// <summary>
/// Represents a JOIN clause to be used with SELECT statements
/// </summary>
public struct JoinClause
{
    public JoinType JoinType;
    public string FromTable;
    public string FromColumn;
    public ComparisonOperator ComparisonOperator;
    public string ToTable;
    public string ToColumn;

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

    public override string ToString() => $"{JoinType} {FromTable}.{FromColumn} {ComparisonOperator} {ToTable}.{ToColumn}";
}