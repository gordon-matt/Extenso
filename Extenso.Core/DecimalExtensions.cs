namespace Extenso
{
    /// <summary>
    /// Provides a set of static methods for querying and manipulating instances of System.Decimal.
    /// </summary>
    public static class DecimalExtensions
    {
        /// <summary>
        /// Gets a value indicating whether the given System.Decimal lies between two other System.Decimal values.
        /// </summary>
        /// <param name="source">The System.Decimal to examine.</param>
        /// <param name="lower">The lower value (we want value to be higher than this)</param>
        /// <param name="higher">The higher value (we want value to be lower than this)</param>
        /// <returns>true if source is between lower and higher; otherwise false;</returns>
        public static bool Between(this decimal source, int lower, int higher)
        {
            return source > lower && source < higher;
        }

        /// <summary>
        /// Gets a value indicating whether the given nullable System.Decimal is null or the default.
        /// </summary>
        /// <param name="source">The nullable System.Decimal to examine.</param>
        /// <returns>true if source is null or the default; otherwise, false.</returns>
        public static bool IsNullOrDefault(this decimal? source)
        {
            return source == null || source == default(decimal);
        }
    }
}