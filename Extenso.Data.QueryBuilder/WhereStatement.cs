namespace Extenso.Data.QueryBuilder;

/// <summary>
/// Represents the where statement.
/// </summary>
public class WhereStatement : List<WhereClause>
{
    /// <summary>
    /// Gets or sets the literal.
    /// </summary>
    public string Literal { get; set; }

    /// <summary>
    /// Creates from the literal.
    /// </summary>
    /// <param name="literal">The literal.</param>
    /// <returns>The literal.</returns>
    public static WhereStatement CreateFromLiteral(string literal) => string.IsNullOrEmpty(literal)
        ? throw new ArgumentNullException(nameof(literal))
        : new WhereStatement
        {
            Literal = literal
        };

    /// <summary>
    /// Adds the clause.
    /// </summary>
    /// <param name="clause">The clause.</param>
    /// <returns>The clause.</returns>
    public WhereStatement AddClause(WhereClause clause)
    {
        Add(clause);
        return this;
    }
}