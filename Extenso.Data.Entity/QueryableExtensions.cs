using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Extenso.Data.Entity;

public static class QueryableExtensions
{
    extension<T>(IQueryable<T> query) where T : class
    {
        public IQueryable<T> Include<TProperty>(Expression<Func<T, TProperty>> path, bool condition) =>
            condition ? query.Include(path) : query;

        public IQueryable<T> WhereIf(Expression<Func<T, bool>> predicate, bool condition) =>
            condition ? query.Where(predicate) : query;
    }
}