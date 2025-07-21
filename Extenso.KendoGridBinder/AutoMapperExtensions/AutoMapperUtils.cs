using AutoMapper;
using AutoMapper.Internal;
using Extenso.KendoGridBinder.Extensions;

namespace Extenso.KendoGridBinder.AutoMapperExtensions;

public class AutoMapperUtils
{
    private readonly MapperConfiguration _mapperConfiguration;

    public AutoMapperUtils(MapperConfiguration mapperConfiguration)
    {
        _mapperConfiguration = mapperConfiguration;
    }

    public Dictionary<string, MapExpression<TEntity>> GetModelMappings<TEntity, TViewModel>(Dictionary<string, MapExpression<TEntity>> mappings = null)
    {
        if (SameTypes<TEntity, TViewModel>())
        {
            return null;
        }

        var map = _mapperConfiguration?.Internal().FindTypeMapFor<TEntity, TViewModel>();
        if (map == null)
        {
            return null;
        }

        mappings ??= [];

        // Custom expressions because they do not map field to field
        foreach (var propertyMap in map.PropertyMaps.Where(pm => pm.CustomMapExpression != null))
        {
            // Get the linq expression body
            string body = propertyMap.CustomMapExpression.Body.ToString();

            // Get the item tag
            string tag = propertyMap.CustomMapExpression.Parameters[0].Name;

            string destination = body.Replace($"{tag}.", string.Empty);
            string source = propertyMap.DestinationMember.Name;

            var customExpression = new MapExpression<TEntity>
            {
                Path = destination,
                Expression = propertyMap.CustomMapExpression.ToTypedExpression<TEntity>()
            };

            if (!mappings.ContainsKey(source))
            {
                mappings.Add(source, customExpression);
            }
        }

        foreach (var propertyMap in map.PropertyMaps.Where(pm => pm.CustomMapExpression is null and null))
        {
            var customResolver = propertyMap.Resolver;
            if (customResolver is IKendoGridExValueResolver<TEntity>)
            {
                string source = propertyMap.DestinationMember.Name;

                var kendoResolver = (IKendoGridExValueResolver<TEntity>)customResolver;
                string destination = kendoResolver.DestinationProperty;
                var expression = propertyMap.CustomMapExpression != null
                    ? propertyMap.CustomMapExpression.ToTypedExpression<TEntity>()
                    : kendoResolver.Expression;

                var customExpression = new MapExpression<TEntity>
                {
                    Path = destination,
                    Expression = expression
                };

                if (!mappings.ContainsKey(source))
                {
                    mappings.Add(source, customExpression);
                }
            }
        }

        return mappings;
    }

    public static bool SameTypes<TEntity, TViewModel>() => typeof(TEntity) == typeof(TViewModel);
}