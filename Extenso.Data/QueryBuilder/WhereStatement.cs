namespace Extenso.Data.QueryBuilder
{
    using System.Collections.Generic;

    public class WhereStatement
    {
        public WhereStatement()
        {
            this.Clauses = new List<WhereClause>();
        }

        public ICollection<WhereClause> Clauses { get; private set; }

        public WhereStatement AddClause(WhereClause clause)
        {
            Clauses.Add(clause);
            return this;
        }
    }
}