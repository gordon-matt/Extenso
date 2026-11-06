using System.Runtime.Serialization;

namespace Extenso.KendoGridBinder.Containers.Json;

/// <summary>
/// This class maps 1 : 1 to JSON Aggregate
/// </summary>
[DataContract(Name = "aggregate")]
public class AggregateObject
{
    /// <summary>
    /// Gets or sets the field.
    /// </summary>
    [DataMember(Name = "field")]
    public string Field { get; set; }

    /// <summary>
    /// Gets or sets the aggregate.
    /// </summary>
    [DataMember(Name = "aggregate")]
    public string Aggregate { get; set; }

    // TODO : is this used/supported ?
    /// <summary>
    /// Gets or sets the direction.
    /// </summary>
    [DataMember(Name = "dir")]
    public string Direction { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AggregateObject"/> class.
    /// </summary>
    public AggregateObject()
    {
        Direction = "asc";
    }

    /// <summary>
    /// Get the Linq Aggregate
    /// </summary>
    /// <param name="fieldConverter">Convert which can be used to convert ViewModel to Model if needed.</param>
    /// <returns>string</returns>
    public string GetLinqAggregate(Func<string, string> fieldConverter = null)
    {
        string convertedField = fieldConverter != null ? fieldConverter.Invoke(Field) : Field;
        return Aggregate switch
        {
            "count" => $"Count() as count__{Field}",
            "sum" => $"Sum(TEntity__.{convertedField}) as sum__{Field}",
            "max" => $"Max(TEntity__.{convertedField}) as max__{Field}",
            "min" => $"Min(TEntity__.{convertedField}) as min__{Field}",
            "average" => $"Average(TEntity__.{convertedField}) as average__{Field}",
            _ => string.Empty,
        };
    }
}