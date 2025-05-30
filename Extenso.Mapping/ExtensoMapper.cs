using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using Extenso.Reflection;

namespace Extenso.Mapping;

/// <summary>
/// A lightweight, simpler alternative to AutoMapper.
/// </summary>
public static class ExtensoMapper
{
    private static readonly ConcurrentDictionary<TypePair, Lazy<Delegate>> expressionMappingCache = new();
    private static readonly ConcurrentDictionary<TypePair, Func<object, object>> mappings = new();

    /// <summary>
    /// Registers a mapping function for the specified source and destination types.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDestination"></typeparam>
    /// <param name="mapFunc"></param>
    public static void Register<TSource, TDestination>(Func<TSource, TDestination> mapFunc)
    {
        var key = new TypePair(typeof(TSource), typeof(TDestination));
        mappings[key] = source => mapFunc((TSource)source);
    }

    /// <summary>
    /// Maps an object of type TSource to TDestination using the registered mapping function.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDestination"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static TDestination Map<TSource, TDestination>(TSource source)
    {
        ArgumentNullException.ThrowIfNull(source);

        var key = new TypePair(typeof(TSource), typeof(TDestination));

        return mappings.TryGetValue(key, out var mapFunc)
            ? (TDestination)mapFunc(source)
            : throw new InvalidOperationException($"Mapping from {typeof(TSource)} to {typeof(TDestination)} is not registered.");
    }

    /// <summary>
    /// Maps an object of type sourceType to destinationType using the registered mapping function.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="sourceType"></param>
    /// <param name="destinationType"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static object Map(object source, Type sourceType, Type destinationType)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(sourceType);
        ArgumentNullException.ThrowIfNull(destinationType);

        var key = new TypePair(sourceType, destinationType);

        return mappings.TryGetValue(key, out var mapFunc)
            ? mapFunc(source)
            : throw new InvalidOperationException($"Mapping from {sourceType} to {destinationType} is not registered.");
    }

    /// <summary>
    /// Maps a function for use as an Entity Framework include path
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDestination"></typeparam>
    /// <typeparam name="TProperty"></typeparam>
    /// <param name="includePath"></param>
    /// <returns></returns>
    public static Expression<Func<TDestination, TProperty>> MapInclude<TSource, TDestination, TProperty>(
        Expression<Func<TSource, TProperty>> includePath)
    {
        ArgumentNullException.ThrowIfNull(includePath);

        var parameter = Expression.Parameter(typeof(TDestination), "x");
        var visitor = new ExpressionMappingVisitor<TSource, TDestination>(parameter);
        var body = visitor.Visit(includePath.Body);

        return Expression.Lambda<Func<TDestination, TProperty>>(body, parameter);
    }

    /// <summary>
    /// Maps an include expression from the source queryable type to the destination queryable type.
    /// </summary>
    /// <typeparam name="TSource">The type of the source queryable.</typeparam>
    /// <typeparam name="TDestination">The type of the destination queryable.</typeparam>
    /// <param name="includeExpression">An expression defining the include operation on the source queryable.</param>
    /// <returns>A function that applies the mapped include operation to a queryable of the destination type.</returns>
    public static Func<IQueryable<TDestination>, IQueryable<TDestination>> MapInclude<TSource, TDestination>(
        Expression<Func<IQueryable<TSource>, IQueryable<TSource>>> includeExpression) => MapQuery<TSource, TDestination>(includeExpression);

    /// <summary>
    /// Maps an include expression from TSource to TDestination for Entity Framework.
    /// </summary>
    /// <typeparam name="TSource">The source type</typeparam>
    /// <typeparam name="TDestination">The destination type</typeparam>
    /// <param name="orderByExpression">The include expression to map</param>
    /// <returns>A function that can be used to include related entities in a query</returns>
    public static Func<IQueryable<TDestination>, IQueryable<TDestination>> MapOrderBy<TSource, TDestination>(
        Expression<Func<IQueryable<TSource>, IQueryable<TSource>>> orderByExpression) => MapQuery<TSource, TDestination>(orderByExpression);

    /// <summary>
    /// Maps a predicate for use as an Entity Framework where clause
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDestination"></typeparam>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static Expression<Func<TDestination, bool>> MapPredicate<TSource, TDestination>(
        Expression<Func<TSource, bool>> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        var key = new TypePair(typeof(TSource), typeof(TDestination));

        var lazyDelegate = expressionMappingCache.GetOrAdd(key, k =>
            new Lazy<Delegate>(() => CompileExpressionMapping<TSource, TDestination>()));

        var compiledFunc = (Func<Expression<Func<TSource, bool>>, Expression<Func<TDestination, bool>>>)lazyDelegate.Value;
        return compiledFunc(predicate);
    }

    /// <summary>
    /// Maps a projection expression from TSource to TDestination.
    /// </summary>
    /// <typeparam name="TSource">The source type</typeparam>
    /// <typeparam name="TDestination">The destination type</typeparam>
    /// <typeparam name="TResult">The result type</typeparam>
    /// <param name="projectionExpression">The projection expression to map</param>
    /// <returns>A mapped projection expression</returns>
    public static Expression<Func<TDestination, TResult>> MapProjection<TSource, TDestination, TResult>(
        Expression<Func<TSource, TResult>> projectionExpression)
    {
        ArgumentNullException.ThrowIfNull(projectionExpression);
        var mapping = new Dictionary<Type, Type> { { typeof(TSource), typeof(TDestination) } };
        var newProjection = (LambdaExpression)ExpressionTypeMapper.ReplaceTypes(projectionExpression, mapping);
        return (Expression<Func<TDestination, TResult>>)newProjection;
    }

    /// <summary>
    /// Maps a queryable of TSource to TDestination.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDestination"></typeparam>
    /// <param name="query"></param>
    /// <returns></returns>
    public static IQueryable<TDestination> MapQuery<TSource, TDestination>(this IQueryable<TSource> query)
    {
        // Create the parameter expression for the source type
        var parameter = Expression.Parameter(typeof(TSource), "src");

        // Build the mapping expression
        var body = CreateMappingExpression<TSource, TDestination>(parameter);

        // Create the final selector lambda
        var selector = Expression.Lambda<Func<TSource, TDestination>>(body, parameter);

        // Apply the mapping to the query
        return query.Select(selector);
    }

    /// <summary>
    /// Creates a mapping expression that transforms an update operation from the source type to the destination type.
    /// </summary>
    /// <typeparam name="TSource">The type of the source object used in the update operation.</typeparam>
    /// <typeparam name="TDestination">The type of the destination object to which the update operation is mapped.</typeparam>
    /// <param name="updateFactory">An expression representing the update operation for the source type.</param>
    /// <returns>An expression representing the mapped update operation for the destination type.</returns>
    public static Expression<Func<TDestination, TDestination>> MapUpdate<TSource, TDestination>(
        Expression<Func<TSource, TSource>> updateFactory)
    {
        ArgumentNullException.ThrowIfNull(updateFactory);
        var mapping = new Dictionary<Type, Type> { { typeof(TSource), typeof(TDestination) } };
        var newUpdate = (LambdaExpression)ExpressionTypeMapper.ReplaceTypes(updateFactory, mapping);
        return (Expression<Func<TDestination, TDestination>>)newUpdate;
    }

    #region Private Methods

    private static Func<Expression<Func<TSource, bool>>, Expression<Func<TDestination, bool>>> CompileExpressionMapping<TSource, TDestination>() =>
        predicate =>
        {
            var parameter = Expression.Parameter(typeof(TDestination), "x");
            var visitor = new ExpressionMappingVisitor<TSource, TDestination>(parameter);
            var body = visitor.Visit(predicate.Body);
            return Expression.Lambda<Func<TDestination, bool>>(body, parameter);
        };

    private static Expression CreateMappingExpression<TSource, TDestination>(ParameterExpression parameter)
    {
        var bindings = new List<MemberBinding>();
        var destinationType = typeof(TDestination);

        foreach (var destProp in destinationType.GetProperties().Where(p => p.CanWrite))
        {
            try
            {
                var sourceProp = typeof(TSource).GetProperty(destProp.Name);
                if (sourceProp == null) continue;

                var sourceExpr = Expression.Property(parameter, sourceProp);
                Expression mappedExpr;

                // Handle nested object mapping
                if (RequiresNestedMapping(sourceProp.PropertyType, destProp.PropertyType))
                {
                    mappedExpr = CreateNestedMappingExpression(sourceExpr, sourceProp.PropertyType, destProp.PropertyType);
                }
                else
                {
                    // Simple property mapping
                    mappedExpr = sourceExpr;

                    // Add conversion if needed
                    if (sourceProp.PropertyType != destProp.PropertyType)
                    {
                        mappedExpr = Expression.Convert(mappedExpr, destProp.PropertyType);
                    }
                }

                bindings.Add(Expression.Bind(destProp, mappedExpr));
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Failed to map property {destProp.Name} from {typeof(TSource)} to {typeof(TDestination)}", ex);
            }
        }

        return Expression.MemberInit(Expression.New(destinationType), bindings);
    }

    private static Expression CreateMappingExpression(ParameterExpression parameter, Type sourceType, Type destType)
    {
        var bindings = new List<MemberBinding>();

        foreach (var destProp in destType.GetProperties().Where(p => p.CanWrite))
        {
            var sourceProp = sourceType.GetProperty(destProp.Name);
            if (sourceProp == null) continue;

            var sourceExpr = Expression.Property(parameter, sourceProp);
            Expression mappedExpr;

            if (RequiresNestedMapping(sourceProp.PropertyType, destProp.PropertyType))
            {
                mappedExpr = CreateNestedMappingExpression(sourceExpr, sourceProp.PropertyType, destProp.PropertyType);
            }
            else
            {
                mappedExpr = sourceExpr;
                if (sourceProp.PropertyType != destProp.PropertyType)
                {
                    mappedExpr = Expression.Convert(mappedExpr, destProp.PropertyType);
                }
            }

            bindings.Add(Expression.Bind(destProp, mappedExpr));
        }

        return Expression.MemberInit(Expression.New(destType), bindings);
    }

    private static Expression CreateNestedMappingExpression(Expression sourceExpr, Type sourceType, Type destType)
    {
        // Handle collection types
        if (sourceType.IsCollection() && destType.IsCollection())
        {
            var sourceElementType = sourceType.GetGenericArguments()[0];
            var destElementType = destType.GetGenericArguments()[0];

            // Create the mapping expression for the elements
            var elementParam = Expression.Parameter(sourceElementType, "element");
            var elementMapping = CreateMappingExpression(elementParam, sourceElementType, destElementType);
            var elementLambda = Expression.Lambda(elementMapping, elementParam);

            // Call Select() to map each element
            var selectMethod = typeof(Enumerable).GetMethods()
                .First(m => m.Name == "Select" && m.GetParameters().Length == 2)
                .MakeGenericMethod(sourceElementType, destElementType);

            var selectCall = Expression.Call(selectMethod, sourceExpr, elementLambda);

            // Convert to destination collection type if needed
            if (destType.IsAssignableFrom(selectCall.Type))
            {
                return selectCall;
            }

            // Handle case where we need to convert to a specific collection type (like List<T>)
            var toListMethod = typeof(Enumerable).GetMethod("ToList")
                .MakeGenericMethod(destElementType);
            return Expression.Call(toListMethod, selectCall);
        }

        // Original handling for non-collection types
        var nestedParameter = Expression.Parameter(sourceType, "nested");
        var nestedBody = CreateMappingExpression(nestedParameter, sourceType, destType);
        var nestedLambda = Expression.Lambda(nestedBody, nestedParameter);
        return Expression.Invoke(nestedLambda, sourceExpr);
    }

    private static Func<IQueryable<TDestination>, IQueryable<TDestination>> MapQuery<TSource, TDestination>(
       Expression<Func<IQueryable<TSource>, IQueryable<TSource>>> queryExpression)
    {
        ArgumentNullException.ThrowIfNull(queryExpression);

        var mapping = new Dictionary<Type, Type> { { typeof(TSource), typeof(TDestination) } };
        var newInclude = (LambdaExpression)ExpressionTypeMapper.ReplaceTypes(queryExpression, mapping);

        var newBody = Expression.Convert(newInclude.Body, typeof(IQueryable<TDestination>));
        var lambda = Expression.Lambda<Func<IQueryable<TDestination>, IQueryable<TDestination>>>(newBody, newInclude.Parameters[0]);

        return lambda.Compile();
    }

    private static bool RequiresNestedMapping(Type sourceType, Type destType)
    {
        if (sourceType.IsCollection() && destType.IsCollection())
        {
            var sourceElementType = sourceType.GetGenericArguments()[0];
            var destElementType = destType.GetGenericArguments()[0];
            return RequiresNestedMapping(sourceElementType, destElementType);
        }

        return !sourceType.IsValueType &&
               !destType.IsValueType &&
               sourceType != typeof(string) &&
               destType != typeof(string) &&
               sourceType != destType;
    }

    #endregion Private Methods

    #region Nested Types

    private class ExpressionMappingVisitor<TSource, TDestination> : ExpressionVisitor
    {
        private static readonly ConcurrentDictionary<MemberInfo, MemberInfo> memberMappingsCache = new();
        private static readonly ConcurrentDictionary<Type, Type> typeMappingsCache = new();
        private readonly ParameterExpression parameter;
        private readonly Dictionary<MemberExpression, MemberExpression> visitedMembers = [];

        public ExpressionMappingVisitor(ParameterExpression parameter)
        {
            this.parameter = parameter;
        }

        internal static Type GetMappedType(Type sourceType)
        {
            if (sourceType == null) return null;

            // Check cache first
            if (typeMappingsCache.TryGetValue(sourceType, out var cachedType))
            {
                return cachedType;
            }

            // Optimized lookup in mappings dictionary
            foreach (var mapping in mappings)
            {
                if (mapping.Key.Source == sourceType)
                {
                    typeMappingsCache[sourceType] = mapping.Key.Destination;
                    return mapping.Key.Destination;
                }
            }

            typeMappingsCache[sourceType] = null; // Cache negative results too
            return null;
        }

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            var parameters = node.Parameters.Select(p =>
                p.Type == typeof(TSource) ? parameter : p).ToArray();
            var body = Visit(node.Body);
            return Expression.Lambda(body, parameters);
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            // Check cache first
            if (visitedMembers.TryGetValue(node, out var cachedResult))
            {
                return cachedResult;
            }

            // Handle primitive types directly
            if (node.Member.DeclaringType?.IsPrimitive == true ||
                node.Member.DeclaringType == typeof(string) ||
                node.Member.DeclaringType == typeof(decimal))
            {
                return base.VisitMember(node);
            }

            var newExpression = Visit(node.Expression);
            var destMember = GetMappedMember(node.Member, newExpression?.Type) ?? node.Member;

            var result = Expression.MakeMemberAccess(newExpression, destMember);

            // Cache the result
            visitedMembers[node] = result;

            return result;
        }

        protected override Expression VisitMemberInit(MemberInitExpression node)
        {
            var newExpression = (NewExpression)Visit(node.NewExpression);

            var bindings = node.Bindings.Select(binding => binding is MemberAssignment assignment
                    ? Expression.Bind(
                        GetMappedMember(binding.Member) ?? binding.Member,
                        Visit(assignment.Expression)
                    )
                    : binding);

            return Expression.MemberInit(newExpression, bindings);
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.DeclaringType == typeof(Queryable))
            {
                var args = node.Arguments.Select(arg =>
                {
                    if (arg is UnaryExpression unary && unary.Operand is LambdaExpression lambda)
                    {
                        var newParams = lambda.Parameters.Select(p =>
                            Expression.Parameter(
                                GetMappedType(p.Type) ?? p.Type,
                                p.Name));

                        var newBody = Visit(lambda.Body);
                        return Expression.Quote(
                            Expression.Lambda(newBody, newParams));
                    }
                    return Visit(arg);
                }).ToArray();

                var method = node.Method.GetGenericMethodDefinition();
                var genericArgs = node.Method.GetGenericArguments()
                    .Select(t => GetMappedType(t) ?? t)
                    .ToArray();

                var newMethod = method.MakeGenericMethod(genericArgs);
                return Expression.Call(null, newMethod, args);
            }

            return base.VisitMethodCall(node);
        }

        protected override Expression VisitNew(NewExpression node)
        {
            var mappedType = GetMappedType(node.Type) ?? node.Type;
            var args = node.Arguments.Select(Visit);

            return node.Members != null
                ? Expression.New(
                    GetMatchingConstructor(mappedType, node.Constructor),
                    args,
                    node.Members.Select(m => GetMappedMember(m) ?? m)
                )
                : Expression.New(
                GetMatchingConstructor(mappedType, node.Constructor),
                args
            );
        }

        protected override Expression VisitParameter(ParameterExpression node) =>
            node.Type == typeof(TSource) ? parameter : base.VisitParameter(node);

        private static MemberInfo GetMappedMember(MemberInfo sourceMember, Type currentDestType)
        {
            // First try exact match in current type
            var destMember = currentDestType?.GetProperty(sourceMember.Name,
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

            if (destMember != null) return destMember;

            // Check if declaring type is mapped
            var sourceDeclaringType = sourceMember.DeclaringType;
            if (sourceDeclaringType == null) return null;

            var mappedType = GetMappedType(sourceDeclaringType);
            return mappedType != null
                ? mappedType.GetProperty(sourceMember.Name,
                    BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase)
                : (MemberInfo)null;
        }

        private static MemberInfo GetMappedMember(MemberInfo sourceMember) => sourceMember.DeclaringType.IsPrimitive ||
            sourceMember.DeclaringType == typeof(string) ||
            sourceMember.DeclaringType == typeof(decimal)
            ? sourceMember
            : memberMappingsCache.GetOrAdd(sourceMember, key =>
            {
                if (key is PropertyInfo sourceProp)
                {
                    // For projections, we want to keep the original property
                    if (sourceProp.DeclaringType != typeof(TSource) && sourceProp.DeclaringType != typeof(TDestination))
                    {
                        return sourceProp;
                    }

                    var destProp = typeof(TDestination).GetProperty(sourceProp.Name);
                    if (destProp != null) return destProp;

                    var sourceDeclaringType = sourceProp.DeclaringType;
                    var mappedType = GetMappedType(sourceDeclaringType);
                    if (mappedType != null)
                    {
                        return mappedType.GetProperty(sourceProp.Name) ?? sourceProp;
                    }
                }
                return sourceMember; // Return original if no mapping found
            });

        private static ConstructorInfo GetMatchingConstructor(Type type, ConstructorInfo sourceConstructor) => sourceConstructor == null
            ? null
            : type.GetConstructors()
            .FirstOrDefault(c =>
                c.GetParameters().Select(p => p.ParameterType)
                    .SequenceEqual(sourceConstructor.GetParameters()
                    .Select(p => GetMappedType(p.ParameterType) ?? p.ParameterType)));
    }

    #endregion Nested Types
}