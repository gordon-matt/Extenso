using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Extenso.Data.Entity;

public class SearchOptions<TEntity> : ContextOptions
    where TEntity : class
{
    private readonly string filePath;
    private readonly int lineNumber;

    public SearchOptions([CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = 0)
    {
        this.filePath = filePath;
        this.lineNumber = lineNumber;
    }

    public Expression<Func<TEntity, bool>> Query { get; set; }

    public Expression<Func<IQueryable<TEntity>, IQueryable<TEntity>>> Include { get; set; }

    public Expression<Func<IQueryable<TEntity>, IQueryable<TEntity>>> OrderBy { get; set; }

    public int PageSize { get; set; }

    public int PageNumber { get; set; }

    /// <summary>
    ///  Configure the query to load the collections in the query results through separate database queries.
    /// </summary>
    public bool SplitQuery { get; set; }

    public bool TagWithCallSite { get; set; }

    public bool IgnoreMandatoryFilters { get; set; }

    public Dictionary<string, object> MandatoryFilters { get; set; } = [];

    /// <summary>
    ///  Adds a tag to the collection of tags associated with an EF LINQ query. Tags are
    ///  query annotations that can provide contextual tracing information at different
    ///  points in the query pipeline.
    /// </summary>
    public IEnumerable<string> Tags { get; set; }

    public string CallSiteTag => $"File: {filePath}:{lineNumber}";
}