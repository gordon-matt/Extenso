using System.Collections;
using System.IO;
using System.Reflection;
using System.Text;
using Extenso.Collections;
using Extenso.Reflection;

namespace Extenso.Data.QueryBuilder;

/// <summary>
/// Represents the base select query builder.
/// </summary>
public abstract class BaseSelectQueryBuilder : ISelectQueryBuilder
{
    #region Non-Public Members

    /// <summary>
    /// The group by columns.
    /// </summary>
    protected readonly ICollection<string> groupByColumns = [];
    /// <summary>
    /// The selected columns.
    /// </summary>
    protected readonly IDictionary<string, string> selectedColumns = new Dictionary<string, string>();
    /// <summary>
    /// The having statement.
    /// </summary>
    protected WhereStatement havingStatement = [];
    /// <summary>
    /// The is distinct.
    /// </summary>
    protected bool isDistinct;
    /// <summary>
    /// The joins.
    /// </summary>
    protected ICollection<JoinClause> joins = [];
    /// <summary>
    /// The order by statement.
    /// </summary>
    protected ICollection<OrderByClause> orderByStatement = [];
    /// <summary>
    /// The order by literal.
    /// </summary>
    protected string orderByLiteral;
    /// <summary>
    /// The selected tables.
    /// </summary>
    protected ICollection<string> selectedTables = [];
    /// <summary>
    /// The where statement.
    /// </summary>
    protected WhereStatement whereStatement = [];
    /// <summary>
    /// The table aliases.
    /// </summary>
    protected IDictionary<string, string> tableAliases = new Dictionary<string, string>();

    /// <summary>
    /// The skip count.
    /// </summary>
    protected int skipCount;
    /// <summary>
    /// The take count.
    /// </summary>
    protected int takeCount;

    #endregion Non-Public Members

    #region Public Methods

    /// <summary>
    /// Selects the all.
    /// </summary>
    /// <returns>The all.</returns>
    public virtual ISelectQueryBuilder SelectAll()
    {
        selectedColumns.Clear();
        return this;
    }

    /// <summary>
    /// Selects as.
    /// </summary>
    /// <param name="tableName">The table name.</param>
    /// <param name="column">The column.</param>
    /// <param name="alias">The alias.</param>
    /// <returns>The i select query builder.</returns>
    public virtual ISelectQueryBuilder SelectAs(string tableName, string column, string alias)
    {
        //selectedColumns.Clear();
        string columnName = CreateFieldName(tableName, column);
        selectedColumns.Add(columnName, alias);
        return this;
    }

    /// <summary>
    /// Selects the value.
    /// </summary>
    /// <param name="tableName">The table name.</param>
    /// <param name="columns">The columns.</param>
    /// <returns>The i select query builder.</returns>
    public virtual ISelectQueryBuilder Select(string tableName, params ReadOnlySpan<string> columns)
    {
        selectedColumns.Clear();
        string enclosedTableName = EncloseTable(tableName);
        foreach (string column in columns)
        {
            selectedColumns.Add(string.Concat(enclosedTableName, '.', EncloseIdentifier(column)), null);
        }
        return this;
    }

    /// <summary>
    /// Selects the value.
    /// </summary>
    /// <param name="columns">The columns.</param>
    /// <returns>The i select query builder.</returns>
    public virtual ISelectQueryBuilder Select(IEnumerable<TableColumnPair> columns)
    {
        selectedColumns.Clear();
        foreach (var column in columns)
        {
            selectedColumns.Add(CreateFieldName(column.TableName, column.ColumnName), null);
        }
        return this;
    }

    /// <summary>
    /// Selects the value.
    /// </summary>
    /// <param name="literal">The literal.</param>
    /// <returns>The i select query builder.</returns>
    public virtual ISelectQueryBuilder Select(SqlLiteral literal)
    {
        selectedColumns.Add(literal.Value, null);
        return this;
    }

    /// <summary>
    /// Selects the count all.
    /// </summary>
    /// <returns>The count all.</returns>
    public virtual ISelectQueryBuilder SelectCountAll()
    {
        selectedColumns.Clear();
        selectedColumns.Add("COUNT(*)", null);
        return this;
    }

    /// <summary>
    /// Gets the distinct.
    /// </summary>
    /// <param name="isDistinct">The is distinct.</param>
    /// <returns>The distinct.</returns>
    public virtual ISelectQueryBuilder Distinct(bool isDistinct = true)
    {
        this.isDistinct = isDistinct;
        return this;
    }

    /// <summary>
    /// Creates a value from the value.
    /// </summary>
    /// <param name="tableName">The table name.</param>
    /// <returns>The i select query builder.</returns>
    public virtual ISelectQueryBuilder From(string tableName)
    {
        selectedTables.Clear();
        selectedTables.Add(EncloseTable(tableName));
        return this;
    }

    /// <summary>
    /// Creates a value from the value.
    /// </summary>
    /// <param name="tableNames">The table names.</param>
    /// <returns>The i select query builder.</returns>
    public virtual ISelectQueryBuilder From(params ReadOnlySpan<string> tableNames)
    {
        selectedTables.Clear();
        foreach (string table in tableNames)
        {
            selectedTables.Add(EncloseTable(table));
        }
        return this;
    }

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
    public virtual ISelectQueryBuilder Join(
        JoinType joinType,
        string toTableName,
        string toColumnName,
        ComparisonOperator comparisonOperator,
        string fromTableName,
        string fromColumnName)
    {
        var join = new JoinClause(
            joinType,
            toTableName,
            toColumnName,
            comparisonOperator,
            fromTableName,
            fromColumnName);

        joins.Add(join);
        return this;
    }

    /// <summary>
    /// Wheres the value.
    /// </summary>
    /// <param name="tableName">The table name.</param>
    /// <param name="column">The column.</param>
    /// <param name="comparisonOperator">The comparison operator.</param>
    /// <param name="value">The value.</param>
    /// <param name="logicOperator">The logic operator.</param>
    /// <returns>The i select query builder.</returns>
    public virtual ISelectQueryBuilder Where(string tableName, string column, ComparisonOperator comparisonOperator, object value, LogicOperator logicOperator = LogicOperator.And)
    {
        var whereClause = new WhereClause(
            logicOperator,
            tableName,
            column,
            comparisonOperator,
            value);

        whereStatement.AddClause(whereClause);
        return this;
    }

    /// <summary>
    /// Wheres the value.
    /// </summary>
    /// <param name="whereClause">The where clause.</param>
    /// <returns>The i select query builder.</returns>
    public virtual ISelectQueryBuilder Where(WhereClause whereClause)
    {
        whereStatement.AddClause(whereClause);
        return this;
    }

    /// <summary>
    /// Wheres the value.
    /// </summary>
    /// <param name="whereStatement">The where statement.</param>
    /// <returns>The i select query builder.</returns>
    public virtual ISelectQueryBuilder Where(WhereStatement whereStatement)
    {
        this.whereStatement = whereStatement;
        return this;
    }

    /// <summary>
    /// Wheres the value.
    /// </summary>
    /// <param name="literal">The literal.</param>
    /// <returns>The i select query builder.</returns>
    public virtual ISelectQueryBuilder Where(string literal)
    {
        whereStatement = WhereStatement.CreateFromLiteral(literal);
        return this;
    }

    /// <summary>
    /// Orders by.
    /// </summary>
    /// <param name="column">The column.</param>
    /// <param name="order">The order.</param>
    /// <returns>The i select query builder.</returns>
    public virtual ISelectQueryBuilder OrderBy(string column, SortDirection order)
    {
        orderByStatement.Add(new OrderByClause(EncloseIdentifier(column), order));
        return this;
    }

    /// <summary>
    /// Orders by.
    /// </summary>
    /// <param name="tableName">The table name.</param>
    /// <param name="column">The column.</param>
    /// <param name="order">The order.</param>
    /// <returns>The i select query builder.</returns>
    public virtual ISelectQueryBuilder OrderBy(string tableName, string column, SortDirection order)
    {
        orderByStatement.Add(new OrderByClause(CreateFieldName(tableName, column), order));
        return this;
    }

    /// <summary>
    /// Orders by.
    /// </summary>
    /// <param name="literal">The literal.</param>
    /// <returns>The i select query builder.</returns>
    public virtual ISelectQueryBuilder OrderBy(string literal)
    {
        orderByLiteral = literal;
        return this;
    }

    /// <summary>
    /// Gets the group by.
    /// </summary>
    /// <param name="tableName">The table name.</param>
    /// <param name="columns">The columns.</param>
    /// <returns>The group by.</returns>
    public virtual ISelectQueryBuilder GroupBy(string tableName, params ReadOnlySpan<string> columns)
    {
        string enclosedTableName = EncloseTable(tableName);
        foreach (string column in columns)
        {
            groupByColumns.Add(string.Concat(enclosedTableName, '.', EncloseIdentifier(column)));
        }
        return this;
    }

    /// <summary>
    /// Gets the group by.
    /// </summary>
    /// <param name="columns">The columns.</param>
    /// <returns>The group by.</returns>
    public virtual ISelectQueryBuilder GroupBy(IEnumerable<TableColumnPair> columns)
    {
        foreach (var column in columns)
        {
            groupByColumns.Add(CreateFieldName(column.TableName, column.ColumnName));
        }
        return this;
    }

    /// <summary>
    /// Gets the having.
    /// </summary>
    /// <param name="tableName">The table name.</param>
    /// <param name="column">The column.</param>
    /// <param name="comparisonOperator">The comparison operator.</param>
    /// <param name="value">The value.</param>
    /// <param name="logicOperator">The logic operator.</param>
    /// <returns>The having.</returns>
    public virtual ISelectQueryBuilder Having(string tableName, string column, ComparisonOperator comparisonOperator, object value, LogicOperator logicOperator = LogicOperator.And)
    {
        var whereClause = new WhereClause(
            logicOperator,
            tableName,
            column,
            comparisonOperator,
            value);

        havingStatement.AddClause(whereClause);
        return this;
    }

    /// <summary>
    /// Gets the having.
    /// </summary>
    /// <param name="whereClause">The where clause.</param>
    /// <returns>The having.</returns>
    public virtual ISelectQueryBuilder Having(WhereClause whereClause)
    {
        havingStatement.AddClause(whereClause);
        return this;
    }

    /// <summary>
    /// Gets the having.
    /// </summary>
    /// <param name="havingStatement">The having statement.</param>
    /// <returns>The having.</returns>
    public virtual ISelectQueryBuilder Having(WhereStatement havingStatement)
    {
        this.havingStatement = havingStatement;
        return this;
    }

    /// <summary>
    /// Gets the having.
    /// </summary>
    /// <param name="literal">The literal.</param>
    /// <returns>The having.</returns>
    public virtual ISelectQueryBuilder Having(string literal)
    {
        havingStatement = WhereStatement.CreateFromLiteral(literal);
        return this;
    }

    /// <summary>
    /// Skips the value.
    /// </summary>
    /// <param name="count">The count.</param>
    /// <returns>The i select query builder.</returns>
    public virtual ISelectQueryBuilder Skip(int count)
    {
        skipCount = count;
        return this;
    }

    /// <summary>
    /// Takes the value.
    /// </summary>
    /// <param name="count">The count.</param>
    /// <returns>The i select query builder.</returns>
    public virtual ISelectQueryBuilder Take(int count)
    {
        takeCount = count;
        return this;
    }

    /// <summary>
    /// Builds the query.
    /// </summary>
    /// <returns>The query.</returns>
    public abstract string BuildQuery();

    #endregion Public Methods

    #region Non-Public Methods

    /// <summary>
    /// Gets the enclose identifier.
    /// </summary>
    /// <param name="identifier">The identifier.</param>
    /// <returns>The enclose identifier.</returns>
    protected abstract string EncloseIdentifier(string identifier);

    /// <summary>
    /// Gets the enclose table.
    /// </summary>
    /// <param name="tableName">The table name.</param>
    /// <returns>The enclose table.</returns>
    protected virtual string EncloseTable(string tableName) =>
        tableName.Contains('.')
            ? string.Join(".", tableName.Split('.').Select(EncloseIdentifier))
            : EncloseIdentifier(tableName);

    /// <summary>
    /// Creates the field name.
    /// </summary>
    /// <param name="tableName">The table name.</param>
    /// <param name="column">The column.</param>
    /// <returns>The field name.</returns>
    protected virtual string CreateFieldName(string tableName, string column) =>
        $"{EncloseTable(tableName)}.{EncloseIdentifier(column)}";

    /// <summary>
    /// Creates the where statement.
    /// </summary>
    /// <param name="statement">The statement.</param>
    /// <param name="isHaving">The is having.</param>
    /// <returns>The where statement.</returns>
    protected virtual string CreateWhereStatement(WhereStatement statement, bool isHaving)
    {
        if (!string.IsNullOrEmpty(statement.Literal))
        {
            return isHaving ? $"HAVING {statement.Literal} " : $"WHERE {statement.Literal} ";
        }
        else
        {
            var sb = new StringBuilder();
            sb.Append(isHaving ? "HAVING " : "WHERE ");

            bool isFirst = true;
            foreach (var clause in statement)
            {
                CreateWhereClause(clause, sb, isFirst);
                isFirst = false;
            }

            return sb.ToString();
        }
    }

    /// <summary>
    /// Creates the where clause.
    /// </summary>
    /// <param name="clause">The clause.</param>
    /// <param name="sb">The sb.</param>
    /// <param name="isFirst">The is first.</param>
    protected virtual void CreateWhereClause(WhereClause clause, StringBuilder sb, bool isFirst)
    {
        if (!isFirst)
        {
            sb.Append(clause.LogicOperator == LogicOperator.Or ? "OR " : "AND ");
        }

        // Encapsulate if this is a container OR if there are subclauses
        bool doEncapsulateOuter = clause.IsContainerOnly || !clause.SubClauses.IsNullOrEmpty();

        if (doEncapsulateOuter)
        {
            sb.Append('(');
        }

        if (!clause.IsContainerOnly)
        {
            sb.Append(CreateComparisonClause(clause.Table, clause.Column, clause.ComparisonOperator, clause.Value));
        }

        if (!clause.SubClauses.IsNullOrEmpty())
        {
            bool doEncapsulateInner = !clause.IsContainerOnly;

            if (doEncapsulateInner)
            {
                sb.Append("AND (");
            }

            for (int i = 0; i < clause.SubClauses.Count; i++)
            {
                var subClause = clause.SubClauses.ElementAt(i);
                CreateWhereClause(subClause, sb, i == 0);
            }
            if (doEncapsulateInner)
            {
                sb.Append(")");
            }
        }

        if (doEncapsulateOuter)
        {
            sb.Append(")");
        }

        sb.Append(' ');
    }

    /// <summary>
    /// Creates the comparison clause.
    /// </summary>
    /// <param name="tableName">The table name.</param>
    /// <param name="columnName">The column name.</param>
    /// <param name="comparisonOperator">The comparison operator.</param>
    /// <param name="value">The value.</param>
    /// <returns>The comparison clause.</returns>
    protected virtual string CreateComparisonClause(string tableName, string columnName, ComparisonOperator comparisonOperator, object value)
    {
        string fieldName = CreateFieldName(tableName, columnName);

        string output = string.Empty;
        if (value != null && value != DBNull.Value)
        {
            switch (comparisonOperator)
            {
                case ComparisonOperator.EqualTo: output = $"{fieldName} = {FormatSQLValue(value)}"; break;
                case ComparisonOperator.NotEqualTo: output = $"{fieldName} <> {FormatSQLValue(value)}"; break;
                case ComparisonOperator.GreaterThan: output = $"{fieldName} > {FormatSQLValue(value)}"; break;
                case ComparisonOperator.GreaterThanOrEqualTo: output = $"{fieldName} >= {FormatSQLValue(value)}"; break;
                case ComparisonOperator.LessThan: output = $"{fieldName} < {FormatSQLValue(value)}"; break;
                case ComparisonOperator.LessThanOrEqualTo: output = $"{fieldName} <= {FormatSQLValue(value)}"; break;
                case ComparisonOperator.Like: output = $"{fieldName} LIKE {FormatSQLValue(value)}"; break;
                case ComparisonOperator.NotLike: output = $"NOT {fieldName} LIKE {FormatSQLValue(value)}"; break;
                case ComparisonOperator.In:
                    {
                        var type = value.GetType();
                        if (type.IsCollection())
                        {
                            var collection = value as IEnumerable;
                            var collectionType = GetCollectionType(collection, type);
                            if (collectionType == typeof(DateTime))
                            {
                                // IN() won't work for dates. Need to substitue multiple date ranges instead..
                                //  ..for the start and end of each day in `value`
                                var values = collection.OfType<DateTime>().ToArray();
                                var clause = GetDateRangesForInOperator(tableName, columnName, values);
                                var sb = new StringBuilder();
                                CreateWhereClause(clause, sb, true);
                                return sb.ToString();
                            }
                        }

                        output = $"{fieldName} IN ({FormatSQLValue(value)})";
                    }
                    break;

                case ComparisonOperator.Contains: output = $"{fieldName} LIKE {FormatSQLValue("%" + value + "%")}"; break;
                case ComparisonOperator.NotContains: output = $"NOT {fieldName} LIKE {FormatSQLValue("%" + value + "%")}"; break;
                case ComparisonOperator.StartsWith: output = $"{fieldName} LIKE {FormatSQLValue(value + "%")}"; break;
                case ComparisonOperator.EndsWith: output = $"{fieldName} LIKE {FormatSQLValue("%" + value)}"; break;
                case ComparisonOperator.HasFlag: output = $"{fieldName} & {value} <> 0"; break;
            }
        }
        else // value==null	|| value==DBNull.Value
        {
            if (comparisonOperator is not ComparisonOperator.EqualTo and not ComparisonOperator.NotEqualTo)
            {
                throw new Exception("Cannot use comparison operator " + comparisonOperator.ToString() + " for NULL values.");
            }
            else
            {
                switch (comparisonOperator)
                {
                    case ComparisonOperator.EqualTo: output = $"{fieldName} IS NULL"; break;
                    case ComparisonOperator.NotEqualTo: output = $"{fieldName} IS NOT NULL"; break;
                }
            }
        }
        return output;
    }

    /// <summary>
    /// Formats the SQL value.
    /// </summary>
    /// <param name="someValue">The some value.</param>
    /// <returns>The SQL value.</returns>
    protected virtual string FormatSQLValue(object someValue)
    {
        string formattedValue = string.Empty;
        // string StringType = Type.GetType("string").Name;
        // string DateTimeType = Type.GetType("DateTime").Name;

        if (someValue == null)
        {
            formattedValue = "NULL";
            return formattedValue;
        }

        var type = someValue.GetType();

        if (type.IsCollection())
        {
            var collection = someValue as IEnumerable;
            var collectionType = GetCollectionType(collection, type);
            if (collectionType == null)
            {
                formattedValue = "NULL";
                return formattedValue;
            }

            formattedValue = collectionType.Name switch
            {
                "String" => string.Join("','", collection.OfType<string>()).EnquoteSingle(),
                "DateTime" => string.Join("','", collection.OfType<DateTime>().Select(x => x.ToString("yyyy-MM-dd HH:mm:ss"))).EnquoteSingle(),
                "Guid" => string.Join("','", collection.OfType<Guid>().Select(x => x.ToString())).EnquoteSingle(),
                "SqlLiteral" => string.Join(",", collection.OfType<SqlLiteral>().Select(x => x.Value)),
                "DBNull" => "NULL",
                _ => string.Join(",", collection.OfType<object>()),
            };
        }
        else
        {
            formattedValue = type.Name switch
            {
                "String" => string.Format("'{0}'", ((string)someValue).Replace("'", "''")),
                "DateTime" => string.Format("'{0:yyyy/MM/dd HH:mm:ss}'", (DateTime)someValue),
                "Guid" => string.Format("'{0}'", (Guid)someValue),
                "Boolean" => (bool)someValue ? "1" : "0",
                "SqlLiteral" => ((SqlLiteral)someValue).Value,
                "DBNull" => "NULL",
                _ => someValue.ToString(),
            };
        }
        return formattedValue;
    }

    private static Type GetCollectionType(IEnumerable collection, Type parentType)
    {
        var typeInfo = parentType.GetTypeInfo();
        if (typeInfo.IsGenericType)
        {
            return typeInfo.GetGenericArguments().Single();
        }
        else
        {
            object firstItem = collection.OfType<object>().FirstOrDefault();
            if (firstItem != null)
            {
                return firstItem.GetType();
            }
        }

        return null;
    }

    private static WhereClause GetDateRangesForInOperator(string tableName, string columnName, IEnumerable<DateTime> values)
    {
        var outerContainer = WhereClause.CreateContainer(LogicOperator.And);

        int count = values.Count();
        for (int i = 0; i < count; i++)
        {
            var dateTime = values.ElementAt(i);

            var innerContainer = WhereClause.CreateContainer(i == 0 ? LogicOperator.And : LogicOperator.Or);

            var clauseFrom = new WhereClause(LogicOperator.And, tableName, columnName, ComparisonOperator.GreaterThanOrEqualTo, dateTime.Date);
            var clauseTo = new WhereClause(LogicOperator.And, tableName, columnName, ComparisonOperator.LessThan, dateTime.Date.AddDays(1));

            innerContainer.AddSubClause(clauseFrom);
            innerContainer.AddSubClause(clauseTo);

            outerContainer.AddSubClause(innerContainer);
        }

        return outerContainer;
    }

    #endregion Non-Public Methods
}