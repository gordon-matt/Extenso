using Extenso.KendoGridBinder.AutoMapperExtensions;

namespace Extenso.KendoGridBinder;

/// <summary>
/// Represents the kendo grid.
/// </summary>
/// <typeparam name="TModel">The type of the model.</typeparam>
public class KendoGrid<TModel> : KendoGrid<TModel, TModel>
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
         IEnumerable<TModel> source,
         Dictionary<string, MapExpression<TModel>> mappings = null,
         Func<IQueryable<TModel>, IEnumerable<TModel>> conversion = null,
         IEnumerable<string> includes = null)
        : base(request, source.AsQueryable(), mappings, conversion, includes)
    {
    }

    /// <summary>
    /// Initializes a new instance of the KendoGrid class.
    /// </summary>
    /// <param name="list">The list.</param>
    /// <param name="totalCount">The total count.</param>
    public KendoGrid(IEnumerable<TModel> list, int totalCount)
        : base(list, totalCount)
    {
    }
}