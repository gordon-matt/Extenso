using System.Linq.Expressions;
using System.Reflection;

namespace Extenso.KendoGridBinder.Extensions;

public static class ExpressionHelper
{
    /// <summary>
    /// Creates the expression from path.
    /// http://stackoverflow.com/questions/26089918/dynamically-create-sort-lambda-expression
    /// </summary>
    /// <typeparam name="T">The type from the object.</typeparam>
    /// <typeparam name="TProperty">The type from the property.</typeparam>
    /// <param name="propertyPath">The property path.</param>
    /// <returns>Expression{Func{T, TProperty}}</returns>
    public static Expression<Func<T, TProperty>> CreateTypedExpressionFromPath<T, TProperty>(string propertyPath)
    {
        var param = Expression.Parameter(typeof(T), "x");
        string[] parts = propertyPath.Split('.');
        var parent = parts.Aggregate<string, Expression>(param, Expression.Property);

        if (parent.Type.GetTypeInfo().IsValueType)
        {
            var converted = Expression.Convert(parent, typeof(TProperty));
            return Expression.Lambda<Func<T, TProperty>>(converted, param);
        }

        return Expression.Lambda<Func<T, TProperty>>(parent, param);
    }

    /// <summary>
    /// Creates the expression from path.
    /// </summary>
    /// <typeparam name="T">The type from the object</typeparam>
    /// <param name="propertyPath">The property path.</param>
    /// <returns>Expression{Func{T, object}}</returns>
    public static Expression<Func<T, object>> CreateTypedExpressionFromPath<T>(string propertyPath) => CreateTypedExpressionFromPath<T, object>(propertyPath);
}