using System.Drawing;

namespace Extenso;

/// <summary>
/// Provides a set of static methods for querying and manipulating instances of System.Random. Some of these can be useful for generating "dummy" data.
/// </summary>
public static class RandomExtensions
{
    extension(Random source)
    {
        /// <summary>
        /// Returns a random System.Boolean.
        /// </summary>
        /// <returns>System.Boolean</returns>
        public bool NextBoolean() => source.Next(byte.MinValue, byte.MaxValue) > (byte.MaxValue / 2);

        /// <summary>
        /// Returns a random System.Drawing.Color.
        /// </summary>
        /// <param name="includeAlphaChannel">If true, a random value will also be generated for the alpha channel.</param>
        /// <returns>System.Drawing.Color</returns>
        public Color NextColor(bool includeAlphaChannel = false) => includeAlphaChannel
            ? Color.FromArgb(source.Next(0, 255), source.Next(0, 255), source.Next(0, 255), source.Next(0, 255))
            : Color.FromArgb(source.Next(0, 255), source.Next(0, 255), source.Next(0, 255));

        /// <summary>
        /// Returns a random System.DateTime between DateTime.MinValue and DateTime.MaxValue.
        /// </summary>
        /// <returns>System.DateTime</returns>
        public DateTime NextDateTime() => NextDateTime(source, DateTime.MinValue.Year, DateTime.MaxValue.Year);

        /// <summary>
        /// Returns a random System.DateTime between the specified years.
        /// </summary>
        /// <param name="minYear"></param>
        /// <param name="maxYear"></param>
        /// <returns>System.DateTime.</returns>
        public DateTime NextDateTime(int minYear, int maxYear)
        {
            int year = source.Next(minYear, maxYear);
            int month = source.Next(1, 12);
            int day = source.Next(1, 28);
            int hour = source.Next(0, 23);
            int minute = source.Next(0, 59);
            int second = source.Next(0, 59);

            return new DateTime(year, month, day, hour, minute, second);
        }

        /// <summary>
        /// Returns a random element from the given sequence.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">A collection from which to randomly select an object.</param>
        /// <returns>A randomly selected object from the given System.Collections.Generic.IEnumerable`1.</returns>
        public T NextFrom<T>(IEnumerable<T> collection)
        {
            int count = collection.Count();
            return collection.ElementAt(source.Next(0, count - 1));
        }
    }
}