using System.Collections.Specialized;
using Extenso.KendoGridBinder.Containers.Json;
using Extenso.KendoGridBinder.Extensions;
using Extenso.KendoGridBinder.ModelBinder.Api;
using Newtonsoft.Json;

namespace Extenso.KendoGridBinder.ModelBinder;

public static class GridHelper
{
    public static T Parse<T>(NameValueCollection queryString) where T : KendoGridBaseRequest, new() => new()
    {
        Take = queryString.GetQueryValue("take", (int?)null),
        Page = queryString.GetQueryValue("page", (int?)null),
        Skip = queryString.GetQueryValue("skip", (int?)null),
        PageSize = queryString.GetQueryValue("pageSize", (int?)null),

        FilterObjectWrapper = FilterHelper.Parse(queryString),
        GroupObjects = GroupHelper.Parse(queryString),
        AggregateObjects = AggregateHelper.Parse(queryString),
        SortObjects = SortHelper.Parse(queryString)
    };

    public static KendoGridApiRequest Parse(string jsonRequest)
    {
        var kendoJsonRequest = JsonConvert.DeserializeObject<GridRequest>(jsonRequest);

        return new KendoGridApiRequest
        {
            Take = kendoJsonRequest.Take,
            Page = kendoJsonRequest.Page,
            PageSize = kendoJsonRequest.PageSize,
            Skip = kendoJsonRequest.Skip,
            Logic = kendoJsonRequest.Logic,
            GroupObjects = GroupHelper.Map(kendoJsonRequest.Groups),
            AggregateObjects = AggregateHelper.Map(kendoJsonRequest.AggregateObjects),
            FilterObjectWrapper = FilterHelper.MapRootFilter(kendoJsonRequest.Filter),
            SortObjects = SortHelper.Map(kendoJsonRequest.Sort)
        };
    }
}