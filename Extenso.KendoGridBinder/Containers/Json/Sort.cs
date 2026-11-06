using System.Runtime.Serialization;

namespace Extenso.KendoGridBinder.Containers.Json;

/// <summary>
/// This class maps 1 : 1 to JSON Sort
/// </summary>
[DataContract(Name = "sort")]
public class Sort
{
    /// <summary>
    /// Gets or sets the field.
    /// </summary>
    [DataMember(Name = "field")]
    public string Field { get; set; }

    /// <summary>
    /// Gets or sets the dir.
    /// </summary>
    [DataMember(Name = "dir")]
    public string Dir { get; set; }

    /// <summary>
    /// Gets or sets the compare.
    /// </summary>
    [DataMember(Name = "compare")]
    public object Compare { get; set; }
}