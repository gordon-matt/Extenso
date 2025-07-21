using System.Linq.Expressions;

namespace Extenso.KendoGridBinder;

public interface IKendoGridExValueResolver<TEntity>
{
    Expression<Func<TEntity, object>> Expression { get; }

    string DestinationProperty { get; }
}