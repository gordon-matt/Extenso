using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Query;

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

    public static Expression<Func<TDestination, TDestination>> MapUpdate<TSource, TDestination>(
        Expression<Func<TSource, TSource>> updateFactory)
    {
        ArgumentNullException.ThrowIfNull(updateFactory);

        var parameter = Expression.Parameter(typeof(TDestination), "x");
        var visitor = new ExpressionMappingVisitor<TSource, TDestination>(parameter);

        var body = (MemberInitExpression)visitor.Visit(updateFactory.Body);

        return Expression.Lambda<Func<TDestination, TDestination>>(body, parameter);
    }

    /// <summary>
    /// Maps an include expression from TModel to TEntity for Entity Framework.
    /// </summary>
    /// <typeparam name="TModel">The source type</typeparam>
    /// <typeparam name="TEntity">The destination type</typeparam>
    /// <param name="includeExpression">The include expression to map</param>
    /// <returns>A function that can be used to include related entities in a query</returns>
    public static Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> MapInclude<TModel, TEntity>(
        Expression<Func<IQueryable<TModel>, IQueryable<TModel>>> includeExpression)
    {
        ArgumentNullException.ThrowIfNull(includeExpression);

        if (includeExpression.Body is MethodCallExpression methodCall)
        {
            // Handle Include/ThenInclude methods
            if (methodCall.Method.Name == "Include" || methodCall.Method.Name == "ThenInclude")
            {
                // Get the lambda expression from the method call
                if (methodCall.Arguments.Count > 1 &&
                    methodCall.Arguments[1] is UnaryExpression unary &&
                    unary.Operand is LambdaExpression lambda)
                {
                    // Create a new parameter for the entity type
                    var parameter = Expression.Parameter(typeof(TEntity), "x");
                    var visitor = new ExpressionMappingVisitor<TModel, TEntity>(parameter);
                    var body = visitor.Visit(lambda.Body);

                    // Create the new lambda expression
                    var mappedLambda = Expression.Lambda(body, parameter);

                    // Get all Include/ThenInclude methods
                    var methods = typeof(EntityFrameworkQueryableExtensions)
                        .GetMethods()
                        .Where(m => m.Name == methodCall.Method.Name)
                        .ToList();

                    // Find the method with matching parameter types
                    var includeMethod = methods.FirstOrDefault(m =>
                    {
                        var parameters = m.GetParameters();
                        return parameters.Length == 2 &&
                               parameters[1].ParameterType.IsGenericType &&
                               parameters[1].ParameterType.GetGenericTypeDefinition() == typeof(Expression<>);
                    });

                    if (includeMethod == null)
                    {
                        throw new InvalidOperationException($"Could not find appropriate {methodCall.Method.Name} method");
                    }

                    // Make generic method with the correct types
                    includeMethod = includeMethod.MakeGenericMethod(typeof(TEntity), body.Type);

                    return query => (IIncludableQueryable<TEntity, object>)includeMethod.Invoke(null, new object[] { query, mappedLambda });
                }
            }
        }

        throw new ArgumentException("Invalid include expression format", nameof(includeExpression));
    }

    /// <summary>
    /// Maps an order by expression from TModel to TEntity.
    /// </summary>
    /// <typeparam name="TModel">The source type</typeparam>
    /// <typeparam name="TEntity">The destination type</typeparam>
    /// <param name="orderByExpression">The order by expression to map</param>
    /// <returns>A function that can be used to order a query</returns>
    public static Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> MapOrderBy<TModel, TEntity>(
        Expression<Func<IQueryable<TModel>, IQueryable<TModel>>> orderByExpression)
    {
        ArgumentNullException.ThrowIfNull(orderByExpression);

        if (orderByExpression.Body is MethodCallExpression methodCall)
        {
            // Handle method chains
            if (methodCall.Arguments[0] is MethodCallExpression previousCall)
            {
                // Recursively map the previous method call
                var previousFunc = MapOrderBy<TModel, TEntity>(
                    Expression.Lambda<Func<IQueryable<TModel>, IQueryable<TModel>>>(
                        previousCall,
                        orderByExpression.Parameters));

                // Map the current method call
                if (methodCall.Arguments.Count > 1 &&
                    methodCall.Arguments[1] is UnaryExpression unary &&
                    unary.Operand is LambdaExpression lambda)
                {
                    var parameter = Expression.Parameter(typeof(TEntity), "x");
                    var visitor = new ExpressionMappingVisitor<TModel, TEntity>(parameter);
                    var body = visitor.Visit(lambda.Body);
                    var mappedLambda = Expression.Lambda(body, parameter);

                    string methodName = methodCall.Method.Name;
                    MethodInfo methodInfo;
                    if (methodName == "OrderBy" || methodName == "ThenBy")
                    {
                        methodInfo = typeof(Queryable).GetMethods()
                            .First(m => m.Name == methodName && m.GetParameters().Length == 2)
                            .MakeGenericMethod(typeof(TEntity), body.Type);
                    }
                    else if (methodName == "OrderByDescending" || methodName == "ThenByDescending")
                    {
                        methodInfo = typeof(Queryable).GetMethods()
                            .First(m => m.Name == methodName && m.GetParameters().Length == 2)
                            .MakeGenericMethod(typeof(TEntity), body.Type);
                    }
                    else
                    {
                        throw new ArgumentException($"Unsupported ordering method: {methodName}", nameof(orderByExpression));
                    }

                    return query =>
                    {
                        var orderedQuery = previousFunc(query);
                        return (IOrderedQueryable<TEntity>)methodInfo.Invoke(null, new object[] { orderedQuery, mappedLambda });
                    };
                }
            }
            else
            {
                // Handle single method call
                if (methodCall.Arguments.Count > 1 &&
                    methodCall.Arguments[1] is UnaryExpression unary &&
                    unary.Operand is LambdaExpression lambda)
                {
                    var parameter = Expression.Parameter(typeof(TEntity), "x");
                    var visitor = new ExpressionMappingVisitor<TModel, TEntity>(parameter);
                    var body = visitor.Visit(lambda.Body);
                    var mappedLambda = Expression.Lambda(body, parameter);

                    string methodName = methodCall.Method.Name;
                    MethodInfo methodInfo;
                    if (methodName == "OrderBy" || methodName == "ThenBy")
                    {
                        methodInfo = typeof(Queryable).GetMethods()
                            .First(m => m.Name == methodName && m.GetParameters().Length == 2)
                            .MakeGenericMethod(typeof(TEntity), body.Type);
                    }
                    else if (methodName == "OrderByDescending" || methodName == "ThenByDescending")
                    {
                        methodInfo = typeof(Queryable).GetMethods()
                            .First(m => m.Name == methodName && m.GetParameters().Length == 2)
                            .MakeGenericMethod(typeof(TEntity), body.Type);
                    }
                    else
                    {
                        throw new ArgumentException($"Unsupported ordering method: {methodName}", nameof(orderByExpression));
                    }

                    return query => (IOrderedQueryable<TEntity>)methodInfo.Invoke(null, new object[] { query, mappedLambda });
                }
            }
        }

        throw new ArgumentException("Invalid order by expression format", nameof(orderByExpression));
    }

    /// <summary>
    /// Maps a projection expression from TModel to TEntity.
    /// </summary>
    /// <typeparam name="TModel">The source type</typeparam>
    /// <typeparam name="TEntity">The destination type</typeparam>
    /// <typeparam name="TResult">The result type</typeparam>
    /// <param name="projectionExpression">The projection expression to map</param>
    /// <returns>A mapped projection expression</returns>
    public static Expression<Func<TEntity, TResult>> MapProjection<TModel, TEntity, TResult>(
        Expression<Func<TModel, TResult>> projectionExpression)
    {
        ArgumentNullException.ThrowIfNull(projectionExpression);

        var parameter = Expression.Parameter(typeof(TEntity), "x");
        var visitor = new ExpressionMappingVisitor<TModel, TEntity>(parameter);
        var body = visitor.Visit(projectionExpression.Body);

        return Expression.Lambda<Func<TEntity, TResult>>(body, parameter);
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

    private static Expression CreateMappingExpression(
        ParameterExpression parameter,
        Type sourceType,
        Type destType)
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

    private static Expression CreateNestedMappingExpression(
        Expression sourceExpr,
        Type sourceType,
        Type destType)
    {
        var nestedParameter = Expression.Parameter(sourceType, "nested");
        var nestedBody = CreateMappingExpression(nestedParameter, sourceType, destType);
        var nestedLambda = Expression.Lambda(nestedBody, nestedParameter);
        return Expression.Invoke(nestedLambda, sourceExpr);
    }

    private static bool RequiresNestedMapping(Type sourceType, Type destType) =>
        !sourceType.IsValueType &&
        !destType.IsValueType &&
        sourceType != typeof(string) &&
        destType != typeof(string) &&
        sourceType != destType;

    private class ExpressionMappingVisitor<TModel, TEntity> : ExpressionVisitor
    {
        private static readonly ConcurrentDictionary<MemberInfo, MemberInfo> memberMappingsCache = new();
        private static readonly ConcurrentDictionary<Type, Type> typeMappingsCache = new();
        private readonly ParameterExpression parameter;
        private readonly Dictionary<MemberExpression, MemberExpression> visitedMembers = [];

        public ExpressionMappingVisitor(ParameterExpression parameter)
        {
            this.parameter = parameter;
        }

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            var parameters = node.Parameters.Select(p =>
                p.Type == typeof(TModel) ? parameter : p).ToArray();
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

            var bindings = node.Bindings.Select(binding =>
            {
                if (binding is MemberAssignment assignment)
                {
                    return Expression.Bind(
                        GetMappedMember(binding.Member) ?? binding.Member,
                        Visit(assignment.Expression)
                    );
                }
                return binding;
            });

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

            if (node.Members != null)
            {
                return Expression.New(
                    GetMatchingConstructor(mappedType, node.Constructor),
                    args,
                    node.Members.Select(m => GetMappedMember(m) ?? m)
                );
            }

            return Expression.New(
                GetMatchingConstructor(mappedType, node.Constructor),
                args
            );
        }

        protected override Expression VisitParameter(ParameterExpression node) =>
            node.Type == typeof(TModel) ? parameter : base.VisitParameter(node);

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
            if (mappedType != null)
            {
                return mappedType.GetProperty(sourceMember.Name,
                    BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            }

            return null;
        }

        private static MemberInfo GetMappedMember(MemberInfo sourceMember)
        {
            if (sourceMember.DeclaringType.IsPrimitive ||
                sourceMember.DeclaringType == typeof(string) ||
                sourceMember.DeclaringType == typeof(decimal))
            {
                return sourceMember;
            }

            return memberMappingsCache.GetOrAdd(sourceMember, key =>
            {
                if (key is PropertyInfo sourceProp)
                {
                    // For projections, we want to keep the original property
                    if (sourceProp.DeclaringType != typeof(TModel) && sourceProp.DeclaringType != typeof(TEntity))
                    {
                        return sourceProp;
                    }

                    var destProp = typeof(TEntity).GetProperty(sourceProp.Name);
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
        }

        private static Type GetMappedType(Type sourceType)
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

        private static ConstructorInfo GetMatchingConstructor(Type type, ConstructorInfo sourceConstructor) => sourceConstructor == null
            ? null
            : type.GetConstructors()
            .FirstOrDefault(c =>
                c.GetParameters().Select(p => p.ParameterType)
                    .SequenceEqual(sourceConstructor.GetParameters()
                    .Select(p => GetMappedType(p.ParameterType) ?? p.ParameterType)));
    }

    private class ReplaceExpressionVisitor : ExpressionVisitor
    {
        private readonly Expression newValue;
        private readonly Expression oldValue;

        public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
        {
            this.oldValue = oldValue;
            this.newValue = newValue;
        }

        public override Expression Visit(Expression node) => node == oldValue
            ? newValue
            : base.Visit(node);
    }

    #endregion Private Methods
}