namespace Extenso;

/// <summary>
/// Provides a set of static methods for querying and manipulating instances of System.Int64.
/// </summary>
public static class Int64Extensions
{
    extension(long? source)
    {
        /// <summary>
        /// Gets a value indicating whether the given nullable System.Int64 is null or the default.
        /// </summary>
        /// <returns>true if source is null or the default; otherwise, false.</returns>
        public bool IsNullOrDefault() => source is null or default(long);
    }
}