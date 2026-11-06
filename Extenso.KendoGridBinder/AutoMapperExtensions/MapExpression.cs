using System.Linq.Expressions;

namespace Extenso.KendoGridBinder.AutoMapperExtensions;

/// <summary>
/// Represents the map expression.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
public class MapExpression<TEntity>
{
    /// <summary>
    /// Gets or sets the path.
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// Gets or sets the expression.
    /// </summary>
    public Expression<Func<TEntity, object>> Expression { get; set; }
}