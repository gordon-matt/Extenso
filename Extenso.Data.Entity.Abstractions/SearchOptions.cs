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

    /// <summary>
    ///  Configure the query to load the collections in the query results through separate database queries.
    /// </summary>
    public bool SplitQuery { get; set; }

    /// <summary>
    ///  Adds a tag to the collection of tags associated with an EF LINQ query. Tags are
    ///  query annotations that can provide contextual tracing information at different
    ///  points in the query pipeline.
    /// </summary>
    public string Tag { get; set; }
}