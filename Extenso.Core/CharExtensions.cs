namespace Extenso;

/// <summary>
/// Provides a set of static methods for querying and manipulating instances of System.Char.
/// </summary>
public static class CharExtensions
{
    /// <summary>
    /// Initializes a new instance of the System.String class to the value indicated by a specified Unicode character repeated a specified number of times.
    /// </summary>
    /// <param name="c">A Unicode character.</param>
    /// <param name="count">The number of times c occurs.</param>
    /// <returns>A System.String consisting of the specified Unicode character repeated the specified number of times.</returns>
    public static string Repeat(this char c, int count) => new(c, count);
}