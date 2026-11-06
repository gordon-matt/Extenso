using System.Runtime.Serialization;

namespace Extenso.KendoGridBinder.Containers.Json;

/// <summary>
/// This class maps 1 : 1 to JSON filter
/// </summary>
[DataContract]
public class Filter
{
    /// <summary>
    /// Gets or sets the field.
    /// </summary>
    [DataMember(Name = "field")]
    public string Field { get; set; }

    /// <summary>
    /// Gets or sets the operator.
    /// </summary>
    [DataMember(Name = "operator")]
    public string Operator { get; set; }

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    [DataMember(Name = "value")]
    public string Value { get; set; }

    /// <summary>
    /// Gets or sets the logic.
    /// </summary>
    [DataMember(Name = "logic")]
    public string Logic { get; set; }

    /// <summary>
    /// Gets or sets the ignore case.
    /// </summary>
    [DataMember(Name = "ignoreCase")]
    public string IgnoreCase { get; set; }

    /// <summary>
    /// Gets or sets the filters.
    /// </summary>
    [DataMember(Name = "filters")]
    public List<Filter> Filters { get; set; }
}