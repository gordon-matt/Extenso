namespace Extenso.Data.QueryBuilder
{
    using System.Collections.Generic;

    public class WhereClause
    {
        public bool IsContainerOnly { get; private set; }

        private WhereClause()
        {
            SubClauses = new List<WhereClause>();
        }

        public WhereClause(LogicOperator logicOperator, string table, string column, ComparisonOperator comparisonOperator, object value)
        {
            LogicOperator = logicOperator;
            Table = table;
            Column = column;
            ComparisonOperator = comparisonOperator;
            Value = value;
            SubClauses = new List<WhereClause>();
            IsContainerOnly = false;
        }

        public static WhereClause CreateContainer(LogicOperator logicOperator)
        {
            return new WhereClause
            {
                IsContainerOnly = true,
                LogicOperator = logicOperator
            };
        }

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

        public override string ToString()
        {
            if (IsContainerOnly)
            {
                return "Container";
            }

            return $"{LogicOperator} {Table}.{Column} {ComparisonOperator.ToString()} {Value} - Sub Clauses: {SubClauses.Count}";
        }
    }
}