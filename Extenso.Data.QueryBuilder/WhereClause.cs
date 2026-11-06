using System.Diagnostics;

namespace Extenso.Data.QueryBuilder;

/// <summary>
/// Represents the where clause.
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class WhereClause
{
    /// <summary>
    /// Gets or sets a value indicating whether this instance is container only.
    /// </summary>
    public bool IsContainerOnly { get; private set; }

    private WhereClause()
    {
        SubClauses = [];
    }

    /// <summary>
    /// Initializes a new instance of the WhereClause class.
    /// </summary>
    /// <param name="logicOperator">The logic operator.</param>
    /// <param name="table">The table.</param>
    /// <param name="column">The column.</param>
    /// <param name="comparisonOperator">The comparison operator.</param>
    /// <param name="value">The value.</param>
    public WhereClause(LogicOperator logicOperator, string table, string column, ComparisonOperator comparisonOperator, object value)
    {
        LogicOperator = logicOperator;
        Table = table;
        Column = column;
        ComparisonOperator = comparisonOperator;
        Value = value;
        SubClauses = [];
        IsContainerOnly = false;
    }

    /// <summary>
    /// Creates the container.
    /// </summary>
    /// <param name="logicOperator">The logic operator.</param>
    /// <returns>The container.</returns>
    public static WhereClause CreateContainer(LogicOperator logicOperator) => new()
    {
        IsContainerOnly = true,
        LogicOperator = logicOperator
    };

    /// <summary>
    /// Gets or sets the logic operator.
    /// </summary>
    public LogicOperator LogicOperator { get; private set; }

    /// <summary>
    /// Gets or sets the table.
    /// </summary>
    public string Table { get; private set; }

    /// <summary>
    /// Gets or sets the column.
    /// </summary>
    public string Column { get; private set; }

    /// <summary>
    /// Gets or sets the comparison operator.
    /// </summary>
    public ComparisonOperator ComparisonOperator { get; private set; }

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    public object Value { get; private set; }

    /// <summary>
    /// Gets or sets the sub clauses.
    /// </summary>
    public ICollection<WhereClause> SubClauses { get; private set; }

    /// <summary>
    /// Adds the sub clause.
    /// </summary>
    /// <param name="clause">The clause.</param>
    /// <returns>The sub clause.</returns>
    public WhereClause AddSubClause(WhereClause clause)
    {
        SubClauses.Add(clause);
        return this;
    }

    private string DebuggerDisplay => IsContainerOnly
        ? "Container"
        : $"{LogicOperator} {Table}.{Column} {ComparisonOperator} {Value} - Sub Clauses: {SubClauses.Count}";
}