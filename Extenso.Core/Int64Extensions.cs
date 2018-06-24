namespace Extenso
{
    /// <summary>
    /// Provides a set of static methods for querying and manipulating instances of System.Int64.
    /// </summary>
    public static class Int64Extensions
    {
        /// <summary>
        /// Gets a value indicating whether the given nullable System.Int64 is null or the default.
        /// </summary>
        /// <param name="source">The nullable System.Int64 to examine.</param>
        /// <returns>true if source is null or the default; otherwise, false.</returns>
        public static bool IsNullOrDefault(this long? source)
        {
            return source == null || source == default(long);
        }
    }
}