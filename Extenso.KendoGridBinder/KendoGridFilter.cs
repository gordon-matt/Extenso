using Extenso.KendoGridBinder.Containers;
using Newtonsoft.Json;

namespace Extenso.KendoGridBinder;

/// <summary>
/// Represents the kendo grid filter.
/// </summary>
[JsonObject(MemberSerialization.OptIn)]
public class KendoGridFilter
{
    /// <summary>
    /// Gets or sets the logic.
    /// </summary>
    [JsonProperty(PropertyName = "logic")]
    public string Logic { get; set; }

    /// <summary>
    /// Gets or sets the field.
    /// </summary>
    [JsonProperty(PropertyName = "field")]
    public string Field { get; set; }

    /// <summary>
    /// Gets or sets the operator.
    /// </summary>
    [JsonProperty(PropertyName = "operator")]
    public string Operator { get; set; }

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    [JsonProperty(PropertyName = "value")]
    public string Value { get; set; }

    /// <summary>
    /// Gets or sets the filters.
    /// </summary>
    [JsonProperty(PropertyName = "filters")]
    public List<KendoGridFilter> Filters { get; set; }

    /// <summary>
    /// Converts the value to the filter object wrapper.
    /// </summary>
    /// <returns>The filter object wrapper.</returns>
    public FilterObjectWrapper ToFilterObjectWrapper()
    {
        var filters = new List<FilterObject>();
        foreach (var filter in Filters)
        {
            FilterObject filterObject;
            if (filter.Filters != null && filter.Filters.Count == 2)
            {
                var firstFilter = filter.Filters.First();
                var lastFilter = filter.Filters.Last();
                filterObject = new FilterObject
                {
                    Field1 = firstFilter.Field,
                    Operator1 = firstFilter.Operator,
                    Value1 = firstFilter.Value,

                    Logic = filter.Logic,

                    Field2 = lastFilter.Field,
                    Operator2 = lastFilter.Operator,
                    Value2 = lastFilter.Value
                };
            }
            else
            {
                filterObject = new FilterObject
                {
                    Field1 = filter.Field,
                    Operator1 = filter.Operator,
                    Value1 = filter.Value,
                };
            }

            filters.Add(filterObject);
        }

        return new FilterObjectWrapper { Logic = Logic, FilterObjects = filters };
    }
}