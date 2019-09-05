namespace Extenso.Data.QueryBuilder
{
    using System.Collections.Generic;

    public class WhereStatement : List<WhereClause>
    {
        public WhereStatement AddClause(WhereClause clause)
        {
            Add(clause);
            return this;
        }
    }
}