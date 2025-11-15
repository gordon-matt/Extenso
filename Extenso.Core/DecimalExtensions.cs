namespace Extenso;

/// <summary>
/// Provides a set of static methods for querying and manipulating instances of System.Decimal.
/// </summary>
public static class DecimalExtensions
{
    extension(decimal source)
    {
        /// <summary>
        /// Gets a value indicating whether the given System.Decimal lies between two other System.Decimal values.
        /// </summary>
        /// <param name="lower">The lower value (we want value to be higher than this)</param>
        /// <param name="higher">The higher value (we want value to be lower than this)</param>
        /// <returns>true if source is between lower and higher; otherwise false;</returns>
        public bool Between(int lower, int higher) => source > lower && source < higher;
    }

    extension(decimal? source)
    {
        /// <summary>
        /// Gets a value indicating whether the given nullable System.Decimal is null or the default.
        /// </summary>
        /// <returns>true if source is null or the default; otherwise, false.</returns>
        public bool IsNullOrDefault() => source is null or default(decimal);
    }
}