using System;
using System.Collections.Generic;

namespace Extenso.Data.QueryBuilder;

public class WhereStatement : List<WhereClause>
{
    public string Literal { get; set; }

    public static WhereStatement CreateFromLiteral(string literal) => string.IsNullOrEmpty(literal)
        ? throw new ArgumentNullException(nameof(literal))
        : new WhereStatement
        {
            Literal = literal
        };

    public WhereStatement AddClause(WhereClause clause)
    {
        Add(clause);
        return this;
    }
}