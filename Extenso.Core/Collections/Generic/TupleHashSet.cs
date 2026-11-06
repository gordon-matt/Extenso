namespace Extenso.Collections.Generic;

/// <summary>
/// Represents a System.Collections.Generic.HashSet`1 of System.Tuple`2
/// </summary>
/// <typeparam name="T1">The type of the tuple's first component.</typeparam>
/// <typeparam name="T2">The type of the tuple's second component.</typeparam>
[Obsolete("Use a HashSet<T> of tuple types instead (see: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/value-tuples)")]
public class TupleHashSet<T1, T2> : HashSet<Tuple<T1, T2>>
{
    /// <summary>
    /// Adds the value.
    /// </summary>
    /// <param name="item1">The item1.</param>
    /// <param name="item2">The item2.</param>
    public void Add(T1 item1, T2 item2) => this.Add(new Tuple<T1, T2>(item1, item2));
}

/// <summary>
/// Represents a System.Collections.Generic.HashSet`1 of System.Tuple`3
/// </summary>
/// <typeparam name="T1">The type of the tuple's first component.</typeparam>
/// <typeparam name="T2">The type of the tuple's second component.</typeparam>
/// <typeparam name="T3">The type of the tuple's third component.</typeparam>
[Obsolete("Use a HashSet<T> of tuple types instead (see: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/value-tuples)")]
public class TupleHashSet<T1, T2, T3> : HashSet<Tuple<T1, T2, T3>>
{
    /// <summary>
    /// Adds the value.
    /// </summary>
    /// <param name="item1">The item1.</param>
    /// <param name="item2">The item2.</param>
    /// <param name="item3">The item3.</param>
    public void Add(T1 item1, T2 item2, T3 item3) => this.Add(new Tuple<T1, T2, T3>(item1, item2, item3));
}

/// <summary>
/// Represents a System.Collections.Generic.HashSet`1 of System.Tuple`4
/// </summary>
/// <typeparam name="T1">The type of the tuple's first component.</typeparam>
/// <typeparam name="T2">The type of the tuple's second component.</typeparam>
/// <typeparam name="T3">The type of the tuple's third component.</typeparam>
/// <typeparam name="T4">The type of the tuple's fourth component.</typeparam>
[Obsolete("Use a HashSet<T> of tuple types instead (see: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/value-tuples)")]
public class TupleHashSet<T1, T2, T3, T4> : HashSet<Tuple<T1, T2, T3, T4>>
{
    /// <summary>
    /// Adds the value.
    /// </summary>
    /// <param name="item1">The item1.</param>
    /// <param name="item2">The item2.</param>
    /// <param name="item3">The item3.</param>
    /// <param name="item4">The item4.</param>
    public void Add(T1 item1, T2 item2, T3 item3, T4 item4) => this.Add(new Tuple<T1, T2, T3, T4>(item1, item2, item3, item4));
}

/// <summary>
/// Represents a System.Collections.Generic.HashSet`1 of System.Tuple`5
/// </summary>
/// <typeparam name="T1">The type of the tuple's first component.</typeparam>
/// <typeparam name="T2">The type of the tuple's second component.</typeparam>
/// <typeparam name="T3">The type of the tuple's third component.</typeparam>
/// <typeparam name="T4">The type of the tuple's fourth component.</typeparam>
/// <typeparam name="T5">The type of the tuple's fifth component.</typeparam>
[Obsolete("Use a HashSet<T> of tuple types instead (see: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/value-tuples)")]
public class TupleHashSet<T1, T2, T3, T4, T5> : HashSet<Tuple<T1, T2, T3, T4, T5>>
{
    /// <summary>
    /// Adds the value.
    /// </summary>
    /// <param name="item1">The item1.</param>
    /// <param name="item2">The item2.</param>
    /// <param name="item3">The item3.</param>
    /// <param name="item4">The item4.</param>
    /// <param name="item5">The item5.</param>
    public void Add(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5) => this.Add(new Tuple<T1, T2, T3, T4, T5>(item1, item2, item3, item4, item5));
}

/// <summary>
/// Represents a System.Collections.Generic.HashSet`1 of System.Tuple`6
/// </summary>
/// <typeparam name="T1">The type of the tuple's first component.</typeparam>
/// <typeparam name="T2">The type of the tuple's second component.</typeparam>
/// <typeparam name="T3">The type of the tuple's third component.</typeparam>
/// <typeparam name="T4">The type of the tuple's fourth component.</typeparam>
/// <typeparam name="T5">The type of the tuple's fifth component.</typeparam>
/// <typeparam name="T6">The type of the tuple's sixth component.</typeparam>
[Obsolete("Use a HashSet<T> of tuple types instead (see: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/value-tuples)")]
public class TupleHashSet<T1, T2, T3, T4, T5, T6> : HashSet<Tuple<T1, T2, T3, T4, T5, T6>>
{
    /// <summary>
    /// Adds the value.
    /// </summary>
    /// <param name="item1">The item1.</param>
    /// <param name="item2">The item2.</param>
    /// <param name="item3">The item3.</param>
    /// <param name="item4">The item4.</param>
    /// <param name="item5">The item5.</param>
    /// <param name="item6">The item6.</param>
    public void Add(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6) => this.Add(new Tuple<T1, T2, T3, T4, T5, T6>(item1, item2, item3, item4, item5, item6));
}

/// <summary>
/// Represents a System.Collections.Generic.HashSet`1 of System.Tuple`7
/// </summary>
/// <typeparam name="T1">The type of the tuple's first component.</typeparam>
/// <typeparam name="T2">The type of the tuple's second component.</typeparam>
/// <typeparam name="T3">The type of the tuple's third component.</typeparam>
/// <typeparam name="T4">The type of the tuple's fourth component.</typeparam>
/// <typeparam name="T5">The type of the tuple's fifth component.</typeparam>
/// <typeparam name="T6">The type of the tuple's sixth component.</typeparam>
/// <typeparam name="T7">The type of the tuple's seventh component.</typeparam>
[Obsolete("Use a HashSet<T> of tuple types instead (see: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/value-tuples)")]
public class TupleHashSet<T1, T2, T3, T4, T5, T6, T7> : HashSet<Tuple<T1, T2, T3, T4, T5, T6, T7>>
{
    /// <summary>
    /// Adds the value.
    /// </summary>
    /// <param name="item1">The item1.</param>
    /// <param name="item2">The item2.</param>
    /// <param name="item3">The item3.</param>
    /// <param name="item4">The item4.</param>
    /// <param name="item5">The item5.</param>
    /// <param name="item6">The item6.</param>
    /// <param name="item7">The item7.</param>
    public void Add(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7) => this.Add(new Tuple<T1, T2, T3, T4, T5, T6, T7>(item1, item2, item3, item4, item5, item6, item7));
}

/// <summary>
/// Represents a System.Collections.Generic.HashSet`1 of System.Tuple`8
/// </summary>
/// <typeparam name="T1">The type of the tuple's first component.</typeparam>
/// <typeparam name="T2">The type of the tuple's second component.</typeparam>
/// <typeparam name="T3">The type of the tuple's third component.</typeparam>
/// <typeparam name="T4">The type of the tuple's fourth component.</typeparam>
/// <typeparam name="T5">The type of the tuple's fifth component.</typeparam>
/// <typeparam name="T6">The type of the tuple's sixth component.</typeparam>
/// <typeparam name="T7">The type of the tuple's seventh component.</typeparam>
/// <typeparam name="T8">The type of the tuple's eighth component.</typeparam>
/// <typeparam name="TRest">Any generic Tuple object that defines the types of the tuple's remaining components.</typeparam>
[Obsolete("Use a HashSet<T> of tuple types instead (see: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/value-tuples)")]
public class TupleHashSet<T1, T2, T3, T4, T5, T6, T7, T8> : HashSet<Tuple<T1, T2, T3, T4, T5, T6, T7, T8>>
{
    /// <summary>
    /// Adds the value.
    /// </summary>
    /// <param name="item1">The item1.</param>
    /// <param name="item2">The item2.</param>
    /// <param name="item3">The item3.</param>
    /// <param name="item4">The item4.</param>
    /// <param name="item5">The item5.</param>
    /// <param name="item6">The item6.</param>
    /// <param name="item7">The item7.</param>
    /// <param name="item8">The item8.</param>
    public void Add(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8) => this.Add(new Tuple<T1, T2, T3, T4, T5, T6, T7, T8>(item1, item2, item3, item4, item5, item6, item7, item8));
}