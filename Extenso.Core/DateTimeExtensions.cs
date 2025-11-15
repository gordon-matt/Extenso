using System.Globalization;

namespace Extenso;

/// <summary>
/// Provides a set of static methods for querying and manipulating instances of System.DateTime.
/// </summary>
public static class DateTimeExtensions
{
    extension(DateTime source)
    {
        /// <summary>
        /// Gets a System.DateTime value that represents the last day of the week for the given System.DateTime and bases it
        /// on the last day of week specified with a System.DayOfWeek value.
        /// </summary>
        /// <param name="endOfWeek">A System.DayOfWeek value to specify which day is the last day of a week. If not specified, Saturday is the default.</param>
        /// <returns>A System.DateTime that represents the last day of the week for source based on endOfWeek.</returns>
        public DateTime EndOfWeek(DayOfWeek endOfWeek = DayOfWeek.Saturday)
        {
            int diff = endOfWeek - source.DayOfWeek;
            return source.AddDays(diff).Date;
        }

        /// <summary>
        /// Gets a System.DateTime value that represents the first day of the month for the given System.DateTime.
        /// </summary>
        /// <returns>A System.DateTime that represents the first day of the month for source.</returns>
        public DateTime StartOfMonth() => new(source.Year, source.Month, 1, 0, 0, 0, source.Kind);

        /// <summary>
        /// Gets a System.DateTime value that represents the first day of the week for the given System.DateTime and bases it
        /// on the first day of week specified with a System.DayOfWeek value.
        /// </summary>
        /// <param name="startOfWeek">A System.DayOfWeek value to specify which day is the first day of a week. If not specified, Sunday is the default.</param>
        /// <returns>A System.DateTime that represents the first day of the week for source based on startOfWeek.</returns>
        public DateTime StartOfWeek(DayOfWeek startOfWeek = DayOfWeek.Sunday)
        {
            int diff = source.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }

            return source.AddDays(-1 * diff).Date;
        }

        /// <summary>
        /// Gets a System.DateTime value that represents the first day of the year for the given System.DateTime.
        /// </summary>
        /// <returns>A System.DateTime that represents the first day of the year for source.</returns>
        public DateTime StartOfYear() => new(source.Year, 1, 1, 0, 0, 0, source.Kind);

        /// <summary>
        /// Converts the value of the current System.DateTime object to an ISO 8601 formatted string.
        /// </summary>
        /// <returns>A System.String with the value of source in ISO 8601 format.</returns>
        public string ToISO8601DateString() => source.ToString("yyyy-MM-ddTHH:mm:ss.fffK", CultureInfo.InvariantCulture);

        /// <summary>
        /// Converts the value of the given System.DateTime object to its equivalent Unix Timestamp (a.k.a POSIX time).
        /// </summary>
        /// <returns>A System.Int32 whose value represents the Unix Timestamp equivalent of the given System.DateTime.</returns>
        public long ToUnixTimestamp() => new DateTimeOffset(source).ToUnixTimeSeconds();
    }

    extension(DateTime? source)
    {
        /// <summary>
        /// Gets a value indicating whether the given nullable System.DateTime is null or the default.
        /// </summary>
        /// <returns>true if source is null or the default; otherwise, false.</returns>
        public bool IsNullOrDefault() => source is null || source == default(DateTime);
    }

    /// <summary>
    /// Converts the Unix Timestamp (a.k.a POSIX time) to its System.DateTime equivalent.
    /// </summary>
    /// <param name="unixTimestamp">The Unix Timestamp to convert to its System.DateTime equivalent.</param>
    /// <returns>A System.DateTime equivalent for unixTimestamp</returns>
    public static DateTime ParseUnixTimestamp(int unixTimestamp) =>
        DateTimeOffset.FromUnixTimeSeconds(unixTimestamp).DateTime;
}