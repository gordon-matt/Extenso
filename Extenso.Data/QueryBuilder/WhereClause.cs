namespace Extenso.Data.QueryBuilder
{
    using System.Collections.Generic;

    public class WhereClause
    {
        public WhereClause(LogicOperator logicOperator, string table, string column, ComparisonOperator comparisonOperator, object value)
        {
            this.LogicOperator = logicOperator;
            this.Table = table;
            this.Column = column;
            this.ComparisonOperator = comparisonOperator;
            this.Value = value;
            this.SubClauses = new List<WhereClause>();
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
    }
}