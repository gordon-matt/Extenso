namespace Extenso;

/// <summary>
/// Supports deep and shallow cloning, which create new instances of a class with the same value as an existing instance.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface ICloneable<T>
{
    /// <summary>
    /// Creates a new object that is a shallow copy of the current instance.
    /// </summary>
    /// <returns>A new object that is a shallow copy of this instance.</returns>
    T ShallowCopy();

    /// <summary>
    /// Creates a new object that is a deep copy of the current instance.
    /// </summary>
    /// <returns>A new object that is a deep copy of this instance.</returns>
    T DeepCopy();
}