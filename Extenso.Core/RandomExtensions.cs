using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Extenso
{
    /// <summary>
    /// Provides a set of static methods for querying and manipulating instances of System.Random. Some of these can be useful for generating "dummy" data.
    /// </summary>
    public static class RandomExtensions
    {
        /// <summary>
        /// Returns a random System.Boolean.
        /// </summary>
        /// <param name="random">An instance of System.Random.</param>
        /// <returns>System.Boolean</returns>
        public static bool NextBoolean(this Random random)
        {
            return random.Next(byte.MinValue, byte.MaxValue) > (byte.MaxValue / 2);
        }

        /// <summary>
        /// Returns a random System.Drawing.Color.
        /// </summary>
        /// <param name="random">An instance of System.Random.</param>
        /// <param name="includeAlphaChannel">If true, a random value will also be generated for the alpha channel.</param>
        /// <returns>System.Drawing.Color</returns>
        public static Color NextColor(this Random random, bool includeAlphaChannel = false)
        {
            if (includeAlphaChannel)
            {
                return Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
            }
            else
            {
                return Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
            }
        }

        /// <summary>
        /// Returns a random System.DateTime between DateTime.MinValue and DateTime.MaxValue.
        /// </summary>
        /// <param name="random">An instance of System.Random.</param>
        /// <returns>System.DateTime</returns>
        public static DateTime NextDateTime(this Random random)
        {
            return NextDateTime(random, DateTime.MinValue.Year, DateTime.MaxValue.Year);
        }

        /// <summary>
        /// Returns a random System.DateTime between the specified years.
        /// </summary>
        /// <param name="random">This instance of System.Random.</param>
        /// <param name="minYear"></param>
        /// <param name="maxYear"></param>
        /// <returns>System.DateTime.</returns>
        public static DateTime NextDateTime(this Random random, int minYear, int maxYear)
        {
            int year = random.Next(minYear, maxYear);
            int month = random.Next(1, 12);
            int day = random.Next(1, 28);
            int hour = random.Next(0, 23);
            int minute = random.Next(0, 59);
            int second = random.Next(0, 59);

            return new DateTime(year, month, day, hour, minute, second);
        }

        /// <summary>
        /// Returns a random element from the given sequence.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="random">An instance of System.Random.</param>
        /// <param name="collection">A collection from which to randomly select an object.</param>
        /// <returns>A randomly selected object from the given System.Collections.Generic.IEnumerable`1.</returns>
        public static T NextFrom<T>(this Random random, IEnumerable<T> collection)
        {
            int count = collection.Count();
            return collection.ElementAt(random.Next(0, count - 1));
        }
    }
}