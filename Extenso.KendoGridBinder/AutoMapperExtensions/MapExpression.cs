using System.Linq.Expressions;

namespace Extenso.KendoGridBinder.AutoMapperExtensions;

public class MapExpression<TEntity>
{
    public string Path { get; set; }

    public Expression<Func<TEntity, object>> Expression { get; set; }
}