namespace Extenso;

/// <summary>
/// Provides a set of static methods for querying and manipulating instances of System.Int32.
/// </summary>
public static class Int32Extensions
{
    extension(int source)
    {
        /// <summary>
        /// Gets a value indicating whether the given System.Int32 lies between two other System.Int32 values.
        /// </summary>
        /// <param name="lower">The lower value (we want value to be higher than this)</param>
        /// <param name="higher">The higher value (we want value to be lower than this)</param>
        /// <returns>true if value is between lower and higher; otherwise false;</returns>
        public bool Between(int lower, int higher) => source > lower && source < higher;

        /// <summary>
        /// Gets a value indicating whether this System.Int32 is a multiple of another System.Int32.
        /// </summary>
        /// <param name="numberToCompare">The System.Int32 to compare with.</param>
        /// <returns>true if this System.Int32 is a multiple of numberToCompare; otherwise, false.</returns>
        public bool IsMultipleOf(int numberToCompare) => source % numberToCompare == 0;
    }

    extension(int? source)
    {
        /// <summary>
        /// Gets a value indicating whether the given nullable System.Int32 is null or the default.
        /// </summary>
        /// <returns>true if source is null or the default; otherwise, false.</returns>
        public bool IsNullOrDefault() => source is null or default(int);
    }
}