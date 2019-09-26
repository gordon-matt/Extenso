namespace Extenso.Data.QueryBuilder
{
    using System;
    using System.Collections.Generic;

    public class WhereStatement : List<WhereClause>
    {
        public string Literal { get; set; }

        public static WhereStatement CreateFromLiteral(string literal)
        {
            if (string.IsNullOrEmpty(literal))
            {
                throw new ArgumentNullException(nameof(literal));
            }

            literal = literal.Trim();
            if (!literal.StartsWith("WHERE ", StringComparison.InvariantCultureIgnoreCase))
            {
                literal = $"WHERE {literal} ";
            }

            return new WhereStatement
            {
                Literal = literal
            };
        }

        public WhereStatement AddClause(WhereClause clause)
        {
            Add(clause);
            return this;
        }
    }
}