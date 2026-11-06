using System.Runtime.Serialization;

namespace Extenso.KendoGridBinder.Containers.Json;

/// <summary>
/// This class maps 1 : 1 to JSON Group
/// </summary>
[DataContract(Name = "group")]
public class GroupObject
{
    /// <summary>
    /// Gets or sets the field.
    /// </summary>
    [DataMember(Name = "field")]
    public string Field { get; set; }

    /// <summary>
    /// Gets or sets the direction.
    /// </summary>
    [DataMember(Name = "dir")]
    public string Direction { get; set; }

    /// <summary>
    /// Gets or sets the aggregate objects.
    /// </summary>
    [DataMember(Name = "aggregates")]
    public IEnumerable<AggregateObject> AggregateObjects { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="GroupObject"/> class.
    /// </summary>
    public GroupObject()
    {
        Direction = "asc";
    }
}