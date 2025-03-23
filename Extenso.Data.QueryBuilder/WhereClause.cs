using System.Collections.Generic;

namespace Extenso.Data.QueryBuilder;

public class WhereClause
{
    public bool IsContainerOnly { get; private set; }

    private WhereClause()
    {
        SubClauses = [];
    }

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

    public static WhereClause CreateContainer(LogicOperator logicOperator) => new()
    {
        IsContainerOnly = true,
        LogicOperator = logicOperator
    };

    public LogicOperator LogicOperator { get; private set; }

    public string Table { get; private set; }

    public string Column { get; private set; }

    public ComparisonOperator ComparisonOperator { get; private set; }

    public object Value { get; private set; }

    public ICollection<WhereClause> SubClauses { get; private set; }

    public WhereClause AddSubClause(WhereClause clause)
    {
        SubClauses.Add(clause);
        return this;
    }

    public override string ToString() => IsContainerOnly
            ? "Container"
            : $"{LogicOperator} {Table}.{Column} {ComparisonOperator} {Value} - Sub Clauses: {SubClauses.Count}";
}