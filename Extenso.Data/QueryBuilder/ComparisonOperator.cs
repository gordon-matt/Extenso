// Based on code by Ewout Stortenbeker
// CodeEngine Framework (http://www.code-engine.com/)
// Email: 4ewout@gmail.com
// The version used in here has been heavily modified from the original

namespace Extenso.Data.QueryBuilder
{
    /// <summary>
    /// Represents comparison operators for WHERE, HAVING and JOIN clauses
    /// </summary>
    public enum ComparisonOperator : byte
    {
        EqualTo = 0,
        NotEqualTo = 1,
        Like = 2,
        NotLike = 3,
        GreaterThan = 4,
        GreaterThanOrEqualTo = 5,
        LessThan = 6,
        LessThanOrEqualTo = 7,
        In = 8,
        Contains = 9,
        NotContains = 10,
        StartsWith = 11,
        EndsWith = 12,
    }
}