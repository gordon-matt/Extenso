using Extenso.KendoGridBinder.AutoMapperExtensions;

namespace Extenso.KendoGridBinder;

public class KendoGrid<TModel> : KendoGrid<TModel, TModel>
{
    public KendoGrid(
         KendoGridBaseRequest request,
         IEnumerable<TModel> source,
         Dictionary<string, MapExpression<TModel>> mappings = null,
         Func<IQueryable<TModel>, IEnumerable<TModel>> conversion = null,
         IEnumerable<string> includes = null)
        : base(request, source.AsQueryable(), mappings, conversion, includes)
    {
    }

    public KendoGrid(IEnumerable<TModel> list, int totalCount)
        : base(list, totalCount)
    {
    }
}