using Extenso.KendoGridBinder.Containers;
using Extenso.KendoGridBinder.Containers.Json;
using Extenso.KendoGridBinder.ModelBinder.Mvc;

namespace Extenso.KendoGridBinder;

[Microsoft.AspNetCore.Mvc.ModelBinder(BinderType = typeof(KendoGridMvcModelBinder))]
public abstract class KendoGridBaseRequest
{
    public int? Take { get; set; }
    public int? Skip { get; set; }
    public int? Page { get; set; }
    public int? PageSize { get; set; }

    public string Logic { get; set; }

    public FilterObjectWrapper FilterObjectWrapper { get; set; }
    public IEnumerable<SortObject> SortObjects { get; set; }
    public IEnumerable<GroupObject> GroupObjects { get; set; }
    public IEnumerable<AggregateObject> AggregateObjects { get; set; }
}