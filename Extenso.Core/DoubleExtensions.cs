namespace Extenso;

/// <summary>
/// Provides a set of static methods for querying and manipulating instances of System.Double.
/// </summary>
public static class DoubleExtensions
{
    extension(double? source)
    {
        /// <summary>
        /// Gets a value indicating whether the given nullable System.Double is null or the default.
        /// </summary>
        /// <returns>true if source is null or the default; otherwise, false.</returns>
        public bool IsNullOrDefault() => source is null or default(double);
    }
}