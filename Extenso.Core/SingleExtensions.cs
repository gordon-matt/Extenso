namespace Extenso
{
    /// <summary>
    /// Provides a set of static methods for querying and manipulating instances of System.Single.
    /// </summary>
    public static class SingleExtensions
    {
        /// <summary>
        /// Gets a value indicating whether the given nullable System.Single is null or the default.
        /// </summary>
        /// <param name="source">The nullable System.Single to examine.</param>
        /// <returns>true if source is null or the default; otherwise, false.</returns>
        public static bool IsNullOrDefault(this float? source)
        {
            return source == null || source == default(float);
        }
    }
}