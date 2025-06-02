using System.Linq.Expressions;

namespace Extenso.Data.Entity;

public class SearchOptions<TEntity>
    where TEntity : class
{
    public Expression<Func<TEntity, bool>> Query { get; set; }

    public Expression<Func<IQueryable<TEntity>, IQueryable<TEntity>>> Include { get; set; }

    public Expression<Func<IQueryable<TEntity>, IQueryable<TEntity>>> OrderBy { get; set; }

    public int PageSize { get; set; }

    public int PageNumber { get; set; }

    public bool SplitQuery { get; set; }
}