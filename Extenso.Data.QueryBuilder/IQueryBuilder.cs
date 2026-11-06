// Based on code by Ewout Stortenbeker
// CodeEngine Framework (http://www.code-engine.com/)
// Email: 4ewout@gmail.com
// The version used in here has been heavily modified from the original

namespace Extenso.Data.QueryBuilder;

/// <summary>
/// Defines the contract for the query builder.
/// </summary>
public interface IQueryBuilder
{
    /// <summary>
    /// Builds the query.
    /// </summary>
    /// <returns>The query.</returns>
    string BuildQuery();
}