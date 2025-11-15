using System.Linq.Expressions;
using System.Reflection;

namespace Extenso.KendoGridBinder.Extensions;

public static class LambdaExpressionExtensions
{
    extension(LambdaExpression expression)
    {
        /// <summary>
        /// http://blog.cincura.net/232247-casting-expression-func-tentity-tproperty-to-expression-func-tentity-object/
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <returns>Expression{Func{T, TProperty}}</returns>
        public Expression<Func<T, TProperty>> ToTypedExpression<T, TProperty>()
        {
            var propertyType = expression.Body.Type;

            return !propertyType.GetTypeInfo().IsValueType
                ? Expression.Lambda<Func<T, TProperty>>(expression.Body, expression.Parameters)
                : Expression.Lambda<Func<T, TProperty>>(Expression.Convert(expression.Body, typeof(TProperty)), expression.Parameters);
        }

        /// <summary>
        /// Converts LambdaExpression to a typed expression (Expression{Func{T, object}}).
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <returns>Expression{Func{T, object}}</returns>
        public Expression<Func<T, object>> ToTypedExpression<T>() => expression.ToTypedExpression<T, object>();
    }
}