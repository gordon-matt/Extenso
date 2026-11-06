using Extenso.KendoGridBinder.Containers;
using Extenso.KendoGridBinder.Containers.Json;
using Extenso.KendoGridBinder.ModelBinder.Mvc;

namespace Extenso.KendoGridBinder;

/// <summary>
/// Represents the kendo grid base request.
/// </summary>
[Microsoft.AspNetCore.Mvc.ModelBinder(BinderType = typeof(KendoGridMvcModelBinder))]
public abstract class KendoGridBaseRequest
{
    /// <summary>
    /// Gets or sets the take.
    /// </summary>
    public int? Take { get; set; }
    /// <summary>
    /// Gets or sets the skip.
    /// </summary>
    public int? Skip { get; set; }
    /// <summary>
    /// Gets or sets the page.
    /// </summary>
    public int? Page { get; set; }
    /// <summary>
    /// Gets or sets the page size.
    /// </summary>
    public int? PageSize { get; set; }

    /// <summary>
    /// Gets or sets the logic.
    /// </summary>
    public string Logic { get; set; }

    /// <summary>
    /// Gets or sets the filter object wrapper.
    /// </summary>
    public FilterObjectWrapper FilterObjectWrapper { get; set; }
    /// <summary>
    /// Gets or sets the sort objects.
    /// </summary>
    public IEnumerable<SortObject> SortObjects { get; set; }
    /// <summary>
    /// Gets or sets the group objects.
    /// </summary>
    public IEnumerable<GroupObject> GroupObjects { get; set; }
    /// <summary>
    /// Gets or sets the aggregate objects.
    /// </summary>
    public IEnumerable<AggregateObject> AggregateObjects { get; set; }
}