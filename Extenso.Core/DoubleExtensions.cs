namespace Extenso
{
    /// <summary>
    /// Provides a set of static methods for querying and manipulating instances of System.Double.
    /// </summary>
    public static class DoubleExtensions
    {
        /// <summary>
        /// Gets a value indicating whether the given nullable System.Double is null or the default.
        /// </summary>
        /// <param name="source">The nullable System.Double to examine.</param>
        /// <returns>true if source is null or the default; otherwise, false.</returns>
        public static bool IsNullOrDefault(this double? source)
        {
            return source == null || source == default(double);
        }
    }
}