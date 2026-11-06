namespace Extenso.Data.Entity;

/// <summary>
/// Defines the contract for the entity.
/// </summary>
public interface IEntity
{
    /// <summary>
    /// Gets the key values.
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    [System.Text.Json.Serialization.JsonIgnore]
    object[] KeyValues { get; }
}