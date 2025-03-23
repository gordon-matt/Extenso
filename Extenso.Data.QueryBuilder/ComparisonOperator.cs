// Based on code by Ewout Stortenbeker
// CodeEngine Framework (http://www.code-engine.com/)
// Email: 4ewout@gmail.com
// The version used in here has been heavily modified from the original

namespace Extenso.Data.QueryBuilder;

/// <summary>
/// Represents comparison operators for WHERE, HAVING and JOIN clauses
/// </summary>
public enum ComparisonOperator : byte
{
    /// <summary>
    /// Equivalent to ' = ' in T-SQL
    /// </summary>
    EqualTo = 0,

    /// <summary>
    /// Equivalent to ' &lt;&gt; ' in T-SQL
    /// </summary>
    NotEqualTo = 1,

    /// <summary>
    /// Equivalent to ' LIKE ' in T-SQL
    /// </summary>
    Like = 2,

    /// <summary>
    /// Equivalent to ' NOT LIKE ' in T-SQL
    /// </summary>
    NotLike = 3,

    /// <summary>
    /// Equivalent to ' &gt; ' in T-SQL
    /// </summary>
    GreaterThan = 4,

    /// <summary>
    /// Equivalent to ' &gt;= ' in T-SQL
    /// </summary>
    GreaterThanOrEqualTo = 5,

    /// <summary>
    /// Equivalent to ' &lt; ' in T-SQL
    /// </summary>
    LessThan = 6,

    /// <summary>
    /// Equivalent to ' &lt;= ' in T-SQL
    /// </summary>
    LessThanOrEqualTo = 7,

    /// <summary>
    /// Equivalent to ' IN(…) ' in T-SQL
    /// </summary>
    In = 8,

    /// <summary>
    /// Equivalent to ' LIKE %…% ' in T-SQL
    /// </summary>
    Contains = 9,

    /// <summary>
    /// Equivalent to ' NOT LIKE %…% ' in T-SQL
    /// </summary>
    NotContains = 10,

    /// <summary>
    /// Equivalent to ' LIKE …% ' in T-SQL
    /// </summary>
    StartsWith = 11,

    /// <summary>
    /// Equivalent to ' LIKE %… ' in T-SQL
    /// </summary>
    EndsWith = 12,

    /// <summary>
    /// Equivalent to ' [Column] & [Value] <> 0' in T-SQL
    /// </summary>
    HasFlag = 13
}