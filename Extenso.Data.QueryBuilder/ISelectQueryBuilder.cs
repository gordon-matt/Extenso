namespace Extenso.Data.QueryBuilder;

/// <summary>
/// Defines the contract for the select query builder.
/// </summary>
public interface ISelectQueryBuilder : IQueryBuilder
{
    /// <summary>
    /// Selects the all.
    /// </summary>
    /// <returns>The all.</returns>
    ISelectQueryBuilder SelectAll();

    /// <summary>
    /// Selects as.
    /// </summary>
    /// <param name="tableName">The table name.</param>
    /// <param name="column">The column.</param>
    /// <param name="alias">The alias.</param>
    /// <returns>The i select query builder.</returns>
    ISelectQueryBuilder SelectAs(string tableName, string column, string alias = null);

    /// <summary>
    /// Selects the value.
    /// </summary>
    /// <param name="tableName">The table name.</param>
    /// <param name="columns">The columns.</param>
    /// <returns>The i select query builder.</returns>
    ISelectQueryBuilder Select(string tableName, params ReadOnlySpan<string> columns);

    /// <summary>
    /// Selects the value.
    /// </summary>
    /// <param name="columns">The columns.</param>
    /// <returns>The i select query builder.</returns>
    ISelectQueryBuilder Select(IEnumerable<TableColumnPair> columns);

    /// <summary>
    /// Selects the value.
    /// </summary>
    /// <param name="literal">The literal.</param>
    /// <returns>The i select query builder.</returns>
    ISelectQueryBuilder Select(SqlLiteral literal);

    /// <summary>
    /// Selects the count all.
    /// </summary>
    /// <returns>The count all.</returns>
    ISelectQueryBuilder SelectCountAll();

    /// <summary>
    /// Gets the distinct.
    /// </summary>
    /// <param name="isDistinct">The is distinct.</param>
    /// <returns>The distinct.</returns>
    ISelectQueryBuilder Distinct(bool isDistinct = true);

    /// <summary>
    /// Creates a value from the value.
    /// </summary>
    /// <param name="tableName">The table name.</param>
    /// <returns>The i select query builder.</returns>
    ISelectQueryBuilder From(string tableName);

    /// <summary>
    /// Creates a value from the value.
    /// </summary>
    /// <param name="tableNames">The table names.</param>
    /// <returns>The i select query builder.</returns>
    ISelectQueryBuilder From(params ReadOnlySpan<string> tableNames);

    /// <summary>
    /// Joins the value.
    /// </summary>
    /// <param name="joinType">The join type.</param>
    /// <param name="toTableName">The to table name.</param>
    /// <param name="toColumnName">The to column name.</param>
    /// <param name="comparisonOperator">The comparison operator.</param>
    /// <param name="fromTableName">The from table name.</param>
    /// <param name="fromColumnName">The from column name.</param>
    /// <returns>The i select query builder.</returns>
    ISelectQueryBuilder Join(JoinType joinType, string toTableName, string toColumnName, ComparisonOperator comparisonOperator, string fromTableName, string fromColumnName);

    /// <summary>
    /// Wheres the value.
    /// </summary>
    /// <param name="tableName">The table name.</param>
    /// <param name="column">The column.</param>
    /// <param name="comparisonOperator">The comparison operator.</param>
    /// <param name="value">The value.</param>
    /// <param name="logicOperator">The logic operator.</param>
    /// <returns>The i select query builder.</returns>
    ISelectQueryBuilder Where(string tableName, string column, ComparisonOperator comparisonOperator, object value, LogicOperator logicOperator = LogicOperator.And);

    /// <summary>
    /// Wheres the value.
    /// </summary>
    /// <param name="whereClause">The where clause.</param>
    /// <returns>The i select query builder.</returns>
    ISelectQueryBuilder Where(WhereClause whereClause);

    /// <summary>
    /// Wheres the value.
    /// </summary>
    /// <param name="whereStatement">The where statement.</param>
    /// <returns>The i select query builder.</returns>
    ISelectQueryBuilder Where(WhereStatement whereStatement);

    /// <summary>
    /// Wheres the value.
    /// </summary>
    /// <param name="literal">The literal.</param>
    /// <returns>The i select query builder.</returns>
    ISelectQueryBuilder Where(string literal);

    /// <summary>
    /// Orders by.
    /// </summary>
    /// <param name="column">The column.</param>
    /// <param name="order">The order.</param>
    /// <returns>The i select query builder.</returns>
    ISelectQueryBuilder OrderBy(string column, SortDirection order);

    /// <summary>
    /// Orders by.
    /// </summary>
    /// <param name="tableName">The table name.</param>
    /// <param name="column">The column.</param>
    /// <param name="sortDirection">The sort direction.</param>
    /// <returns>The i select query builder.</returns>
    ISelectQueryBuilder OrderBy(string tableName, string column, SortDirection sortDirection);

    /// <summary>
    /// Orders by.
    /// </summary>
    /// <param name="literal">The literal.</param>
    /// <returns>The i select query builder.</returns>
    ISelectQueryBuilder OrderBy(string literal);

    /// <summary>
    /// Gets the group by.
    /// </summary>
    /// <param name="tableName">The table name.</param>
    /// <param name="columns">The columns.</param>
    /// <returns>The group by.</returns>
    ISelectQueryBuilder GroupBy(string tableName, params ReadOnlySpan<string> columns);

    /// <summary>
    /// Gets the group by.
    /// </summary>
    /// <param name="columns">The columns.</param>
    /// <returns>The group by.</returns>
    ISelectQueryBuilder GroupBy(IEnumerable<TableColumnPair> columns);

    /// <summary>
    /// Gets the having.
    /// </summary>
    /// <param name="tableName">The table name.</param>
    /// <param name="column">The column.</param>
    /// <param name="comparisonOperator">The comparison operator.</param>
    /// <param name="value">The value.</param>
    /// <param name="logicOperator">The logic operator.</param>
    /// <returns>The having.</returns>
    ISelectQueryBuilder Having(string tableName, string column, ComparisonOperator comparisonOperator, object value, LogicOperator logicOperator = LogicOperator.And);

    /// <summary>
    /// Gets the having.
    /// </summary>
    /// <param name="whereClause">The where clause.</param>
    /// <returns>The having.</returns>
    ISelectQueryBuilder Having(WhereClause whereClause);

    /// <summary>
    /// Gets the having.
    /// </summary>
    /// <param name="havingStatement">The having statement.</param>
    /// <returns>The having.</returns>
    ISelectQueryBuilder Having(WhereStatement havingStatement);

    /// <summary>
    /// Gets the having.
    /// </summary>
    /// <param name="literal">The literal.</param>
    /// <returns>The having.</returns>
    ISelectQueryBuilder Having(string literal);

    /// <summary>
    /// Skips the value.
    /// </summary>
    /// <param name="count">The count.</param>
    /// <returns>The i select query builder.</returns>
    ISelectQueryBuilder Skip(int count);

    /// <summary>
    /// Takes the value.
    /// </summary>
    /// <param name="count">The count.</param>
    /// <returns>The i select query builder.</returns>
    ISelectQueryBuilder Take(int count);
}

/// <summary>
/// Represents the table column pair.
/// </summary>
public struct TableColumnPair
{
    /// <summary>
    /// Initializes a new instance of the TableColumnPair class.
    /// </summary>
    /// <param name="tableName">The table name.</param>
    /// <param name="columnName">The column name.</param>
    public TableColumnPair(string tableName, string columnName)
    {
        TableName = tableName;
        ColumnName = columnName;
    }

    /// <summary>
    /// Gets or sets the table name.
    /// </summary>
    public string TableName { get; set; }

    /// <summary>
    /// Gets or sets the column name.
    /// </summary>
    public string ColumnName { get; set; }
}