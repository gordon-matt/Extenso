using System.Runtime.Serialization;

namespace Extenso.KendoGridBinder.Containers.Json;

/// <summary>
/// Represents the grid request.
/// </summary>
[DataContract]
public class GridRequest
{
    /// <summary>
    /// Gets or sets the take.
    /// </summary>
    [DataMember(Name = "take")]
    public int? Take { get; set; }

    /// <summary>
    /// Gets or sets the skip.
    /// </summary>
    [DataMember(Name = "skip")]
    public int? Skip { get; set; }

    /// <summary>
    /// Gets or sets the page.
    /// </summary>
    [DataMember(Name = "page")]
    public int? Page { get; set; }

    /// <summary>
    /// Gets or sets the page size.
    /// </summary>
    [DataMember(Name = "pagesize")]
    public int? PageSize { get; set; }

    /// <summary>
    /// Gets or sets the logic.
    /// </summary>
    [DataMember(Name = "logic")]
    public string Logic { get; set; }

    /// <summary>
    /// Gets or sets the filter.
    /// </summary>
    [DataMember(Name = "filter")]
    public Filter Filter { get; set; }

    /// <summary>
    /// Gets or sets the sort.
    /// </summary>
    [DataMember(Name = "sort")]
    public IEnumerable<Sort> Sort { get; set; }

    /// <summary>
    /// Gets or sets the groups.
    /// </summary>
    [DataMember(Name = "group")]
    public IEnumerable<GroupObject> Groups { get; set; }

    /// <summary>
    /// Gets or sets the aggregate objects.
    /// </summary>
    [DataMember(Name = "aggregate")]
    public IEnumerable<AggregateObject> AggregateObjects { get; set; }
}