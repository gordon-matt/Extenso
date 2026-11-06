namespace Extenso.Mapping;

/// <summary>
/// Represents the type pair.
/// </summary>
public readonly struct TypePair : IEquatable<TypePair>
{
    /// <summary>
    /// Gets the source.
    /// </summary>
    public Type Source { get; }

    /// <summary>
    /// Gets the destination.
    /// </summary>
    public Type Destination { get; }

    /// <summary>
    /// Initializes a new instance of the TypePair class.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="destination">The destination.</param>
    public TypePair(Type source, Type destination)
    {
        Source = source ?? throw new ArgumentNullException(nameof(source));
        Destination = destination ?? throw new ArgumentNullException(nameof(destination));
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <param name="other">The other.</param>
    /// <returns>true when the condition is met; otherwise, false.</returns>
    public bool Equals(TypePair other) =>
        Source == other.Source && Destination == other.Destination;

    /// <inheritdoc/>
    public override int GetHashCode() =>
        HashCode.Combine(Source, Destination);
}