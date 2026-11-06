using System.Linq.Expressions;

namespace Extenso.KendoGridBinder;

/// <summary>
/// Defines the contract for the kendo grid ex value resolver.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
public interface IKendoGridExValueResolver<TEntity>
{
    /// <summary>
    /// Gets the expression.
    /// </summary>
    Expression<Func<TEntity, object>> Expression { get; }

    /// <summary>
    /// Gets the destination property.
    /// </summary>
    string DestinationProperty { get; }
}