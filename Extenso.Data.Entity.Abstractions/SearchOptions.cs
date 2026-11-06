using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Extenso.Data.Entity;

/// <summary>
/// Represents the search options.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
public class SearchOptions<TEntity> : ContextOptions
    where TEntity : class
{
    private readonly string filePath;
    private readonly int lineNumber;

    /// <summary>
    /// Initializes a new instance of the SearchOptions class.
    /// </summary>
    /// <param name="filePath">The file path.</param>
    /// <param name="lineNumber">The line number.</param>
    public SearchOptions([CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = 0)
    {
        this.filePath = filePath;
        this.lineNumber = lineNumber;
    }

    /// <summary>
    /// Gets or sets the query.
    /// </summary>
    public Expression<Func<TEntity, bool>> Query { get; set; }

    /// <summary>
    /// Gets or sets the include.
    /// </summary>
    public Expression<Func<IQueryable<TEntity>, IQueryable<TEntity>>> Include { get; set; }

    /// <summary>
    /// Gets or sets the order by.
    /// </summary>
    public Expression<Func<IQueryable<TEntity>, IQueryable<TEntity>>> OrderBy { get; set; }

    /// <summary>
    /// Gets or sets the page size.
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Gets or sets the page number.
    /// </summary>
    public int PageNumber { get; set; }

    /// <summary>
    ///  Configure the query to load the collections in the query results through separate database queries.
    /// </summary>
    public bool SplitQuery { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is tag with call site.
    /// </summary>
    public bool TagWithCallSite { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether this instance is ignore mandatory filters.
    /// </summary>
    public bool IgnoreMandatoryFilters { get; set; }

    /// <summary>
    /// Gets or sets the mandatory filters.
    /// </summary>
    public Dictionary<string, object> MandatoryFilters { get; set; } = [];

    /// <summary>
    ///  Adds a tag to the collection of tags associated with an EF LINQ query. Tags are
    ///  query annotations that can provide contextual tracing information at different
    ///  points in the query pipeline.
    /// </summary>
    public IEnumerable<string> Tags { get; set; }

    /// <summary>
    /// Gets the call site tag.
    /// </summary>
    public string CallSiteTag => $"File: {filePath}:{lineNumber}";
}