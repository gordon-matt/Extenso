using System.Runtime.Serialization;

namespace Extenso.Data.Entity;

/// <summary>
/// Represents the base entity.
/// </summary>
/// <typeparam name="T">The type of the value.</typeparam>
public abstract class BaseEntity<T> : IEntity
{
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    public T Id { get; set; }

    #region IEntity Members

    /// <summary>
    /// Gets the key values.
    /// </summary>
    [IgnoreDataMember] // OData v8 does not like this property and will break if we don't use [IgnoreDataMember] here.
    public object[] KeyValues => [Id];

    #endregion IEntity Members
}