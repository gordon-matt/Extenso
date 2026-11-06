// Based on code by Ewout Stortenbeker
// CodeEngine Framework (http://www.code-engine.com/)
// Email: 4ewout@gmail.com
// The version used in here has been heavily modified from the original

namespace Extenso.Data.QueryBuilder;

/// <summary>
/// Specifies the join type.
/// </summary>
public enum JoinType : byte
{
    /// <summary>
    /// The inner join value.
    /// </summary>
    InnerJoin,
    /// <summary>
    /// The outer join value.
    /// </summary>
    OuterJoin,
    /// <summary>
    /// The left join value.
    /// </summary>
    LeftJoin,
    /// <summary>
    /// The right join value.
    /// </summary>
    RightJoin
}