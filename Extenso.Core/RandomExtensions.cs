using System;
using System.Collections.Generic;
using System.Drawing;

//using System.Drawing;
using System.Linq;

namespace Extenso
{
    public static class RandomExtensions
    {
        /// <summary>
        /// Returns a random System.Boolean.
        /// </summary>
        /// <param name="random">This instance of System.Random.</param>
        /// <returns>System.Boolean</returns>
        public static bool NextBoolean(this Random random)
        {
            return random.Next(byte.MinValue, byte.MaxValue) > (byte.MaxValue / 2);
        }

        /// <summary>
        /// Returns a random System.Drawing.Color.
        /// </summary>
        /// <param name="random">This instance of System.Random.</param>
        /// <returns>System.Drawing.Color</returns>
        public static Color NextColor(this Random random)
        {
            return Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
        }

        /// <summary>
        /// Returns a random System.Drawing.Color.
        /// </summary>
        /// <param name="random">This instance of System.Random.</param>
        /// <returns>System.Drawing.Color</returns>
        public static Color NextColorWithAlpha(this Random random)
        {
            return Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
        }

        /// <summary>
        /// Returns a random System.DateTime.
        /// </summary>
        /// <param name="random">This instance of System.Random.</param>
        /// <returns>System.DateTime</returns>
        public static DateTime NextDateTime(this Random random)
        {
            return NextDateTime(random, DateTime.MinValue.Year, DateTime.MaxValue.Year);
        }

        /// <summary>
        /// Returns a random System.DateTime between the specified minimum and maximum years.
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
        /// Returns a random element from the specified IEnumerable&lt;T&gt;.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="random">This instance of System.Random.</param>
        /// <param name="enumerable"></param>
        /// <returns>Object of type &lt;T&gt; from specified IEnumerable&lt;T&gt;.</returns>
        public static T NextFrom<T>(this Random random, IEnumerable<T> enumerable)
        {
            var list = enumerable as List<T> ?? enumerable.ToList();
            return list.ElementAt(random.Next(0, list.Count - 1));
        }
    }
}