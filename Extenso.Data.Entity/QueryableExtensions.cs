using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Extenso.Data.Entity;

/// <summary>
/// Provides extension methods for queryables.
/// </summary>
public static class QueryableExtensions
{
    extension<T>(IQueryable<T> query) where T : class
    {
        /// <summary>
        /// Gets the include.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="path">The path.</param>
        /// <param name="condition">The condition.</param>
        /// <returns>The include.</returns>
        public IQueryable<T> Include<TProperty>(Expression<Func<T, TProperty>> path, bool condition) =>
            condition ? query.Include(path) : query;

        /// <summary>
        /// Wheres the if.
        /// </summary>
        /// <param name="predicate">The predicate to apply.</param>
        /// <param name="condition">The condition.</param>
        /// <returns>The if.</returns>
        public IQueryable<T> WhereIf(Expression<Func<T, bool>> predicate, bool condition) =>
            condition ? query.Where(predicate) : query;
    }
}