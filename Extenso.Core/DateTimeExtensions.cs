using System;

namespace Extenso
{
    public static class DateTimeExtensions
    {
        public static bool IsNullOrDefault(this DateTime? dateTime)
        {
            return dateTime == null || dateTime == default(DateTime);
        }

        public static string ToISO8601DateString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-ddTHH:mm:ss.fff");
        }

        public static DateTime ParseUnixTimestamp(int unixTimestamp)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0)
                .AddSeconds(unixTimestamp)
                .ToLocalTime();
        }

        public static double ToUnixTimestamp(this DateTime dateTime)
        {
            return (dateTime - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds * 1000;
        }

        /// <summary>
        /// Gets the beginning of the current month - including time.
        ///
        /// </summary>
        /// <param name="source">The date time object being extended..</param>
        /// <returns>
        /// Beginning of the current month.
        /// </returns>
        public static DateTime BeginningThisMonth(this DateTime source)
        {
            return new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
        }

        /// <summary>
        /// Gets the beginning of the current week - including time.
        ///
        /// </summary>
        /// <param name="source">The date time object being extended..</param>
        /// <returns>
        /// Beginning of the current week.
        /// </returns>
        public static DateTime BeginningThisWeek(this DateTime source)
        {
            DateTime dateTime = source;
            while (dateTime.DayOfWeek != DayOfWeek.Monday)
            {
                dateTime = dateTime.AddDays(-1.0);
            }
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);
        }

        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            var diff = dt.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }

            return dt.AddDays(-1 * diff).Date;
        }

        public static DateTime EndOfWeek(this DateTime dt, DayOfWeek endOfWeek)
        {
            var diff = endOfWeek - dt.DayOfWeek;
            return dt.AddDays(diff).Date;
        }

        public static DateTime StartOfMonth(this DateTime source)
        {
            return new DateTime(source.Year, source.Month, 1, 0, 0, 0, source.Kind);
        }

        public static DateTime StartOfYear(this DateTime source)
        {
            return new DateTime(source.Year, 1, 1, 0, 0, 0, source.Kind);
        }
    }
}