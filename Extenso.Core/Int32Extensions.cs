namespace Extenso
{
    /// <summary>
    /// Provides a set of static methods for querying and manipulating instances of System.Int32.
    /// </summary>
    public static class Int32Extensions
    {
        /// <summary>
        /// Gets a value indicating whether the given System.Int32 lies between two other System.Int32 values.
        /// </summary>
        /// <param name="value">The System.Int32 to examine.</param>
        /// <param name="lower">The lower value (we want value to be higher than this)</param>
        /// <param name="higher">The higher value (we want value to be lower than this)</param>
        /// <returns>true if value is between lower and higher; otherwise false;</returns>
        public static bool Between(this int value, int lower, int higher)
        {
            return value > lower && value < higher;
        }

        /// <summary>
        /// Gets a value indicating whether this System.Int32 is a multiple of another System.Int32.
        /// </summary>
        /// <param name="i">The System.Int32 to examine.</param>
        /// <param name="numberToCompare">The System.Int32 to compare with.</param>
        /// <returns>true if this System.Int32 is a multiple of numberToCompare; otherwise, false.</returns>
        public static bool IsMultipleOf(this int i, int numberToCompare)
        {
            return i % numberToCompare == 0;
        }

        /// <summary>
        /// Gets a value indicating whether the given nullable System.Int32 is null or the default.
        /// </summary>
        /// <param name="source">The nullable System.Int32 to examine.</param>
        /// <returns>true if source is null or the default; otherwise, false.</returns>
        public static bool IsNullOrDefault(this int? source)
        {
            return source == null || source == default(int);
        }
    }
}