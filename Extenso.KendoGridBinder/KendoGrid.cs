using Extenso.KendoGridBinder.AutoMapperExtensions;

namespace Extenso.KendoGridBinder;

/// <summary>
/// Represents the kendo grid.
/// </summary>
public class KendoGrid : KendoGrid<object>
{
    /// <summary>
    /// Initializes a new instance of the KendoGrid class.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="source">The source.</param>
    /// <param name="mappings">The mappings.</param>
    /// <param name="conversion">The conversion.</param>
    /// <param name="includes">The includes.</param>
    public KendoGrid(
         KendoGridBaseRequest request,
         IEnumerable<dynamic> source,
         Dictionary<string, MapExpression<dynamic>> mappings = null,
         Func<IQueryable<dynamic>, IEnumerable<dynamic>> conversion = null,
         IEnumerable<string> includes = null
        )
        : base(request, source.AsQueryable(), mappings, conversion, includes)
    {
    }

    /// <summary>
    /// Initializes a new instance of the KendoGrid class.
    /// </summary>
    /// <param name="list">The list.</param>
    /// <param name="totalCount">The total count.</param>
    public KendoGrid(IEnumerable<dynamic> list, int totalCount)
        : base(list, totalCount)
    {
    }
}