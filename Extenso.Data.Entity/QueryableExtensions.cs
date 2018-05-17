using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Extensions.Internal;

namespace Extenso.Data.Entity
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Include<T, TProperty>(this IQueryable<T> source, Expression<Func<T, TProperty>> path, bool condition) where T : class
        {
            return condition ? source.Include(path) : source;
        }

        public static async Task<HashSet<TSource>> ToHashSetAsync<TSource>(
            this IQueryable<TSource> source,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var asyncEnumerator = source.AsAsyncEnumerable().GetEnumerator();

            var hashSet = new HashSet<TSource>();

            try
            {
                while (true)
                {
                    if (await asyncEnumerator.MoveNext(cancellationToken))
                    {
                        hashSet.Add(asyncEnumerator.Current);
                    }
                    else { break; }
                }
            }
            finally
            {
                if (asyncEnumerator != null)
                {
                    asyncEnumerator.Dispose();
                }
            }
            asyncEnumerator = null;
            return hashSet;
        }

        public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, bool condition)
        {
            return condition ? source.Where(predicate) : source;
        }
    }
}