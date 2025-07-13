namespace Extenso.Mapping;

public readonly struct TypePair : IEquatable<TypePair>
{
    public Type Source { get; }

    public Type Destination { get; }

    public TypePair(Type source, Type destination)
    {
        Source = source ?? throw new ArgumentNullException(nameof(source));
        Destination = destination ?? throw new ArgumentNullException(nameof(destination));
    }

    public bool Equals(TypePair other) =>
        Source == other.Source && Destination == other.Destination;

    public override int GetHashCode() =>
        HashCode.Combine(Source, Destination);
}