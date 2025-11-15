using System.Text;
using Extenso.Collections;

namespace Extenso.Text;

/// <summary>
/// Provides a set of static methods for manipulating instances of System.Text.StringBuilder.
/// </summary>
public static class StringBuilderExtensions
{
    extension(StringBuilder source)
    {
        /// <summary>
        /// Appends copies of the specified strings to the given System.Text.StringBuilder.
        /// </summary>
        /// <param name="values">The strings to append.</param>
        /// <returns>A reference to stringBuilder after the append operation has completed.</returns>
        public StringBuilder Append(params ReadOnlySpan<string> values)
        {
            foreach (string value in values)
            {
                source.Append(value);
            }
            return source;
        }

        /// <summary>
        /// Appends the string representations of the specified objects to the given System.Text.StringBuilder.
        /// </summary>
        /// <param name="values">The objects to append.</param>
        /// <returns>A reference to stringBuilder after the append operation has completed.</returns>
        public StringBuilder Append(params ReadOnlySpan<object> values)
        {
            foreach (object value in values)
            {
                source.Append(value);
            }
            return source;
        }

        /// <summary>
        /// Concatenates the string representations of each member of a sequence, using the given
        /// separator between each member and then appends the result to the given System.Text.StringBuilder.
        /// </summary>
        /// <typeparam name="T">The type of objects in the sequence.</typeparam>
        /// <param name="values">The sequence.</param>
        /// <param name="separator">The string to use as a separator. separator is included only if values has more than one element.</param>
        /// <param name="selector">A function to extract a value from each element.</param>
        /// <returns>A reference to stringBuilder after the append operation has completed.</returns>
        public StringBuilder Append<T>(IEnumerable<T> values, string separator = ",", Func<T, object> selector = null) =>
            selector is not null
                ? source.Append(values.Select(selector).Join(separator))
                : source.Append(values.Join(separator));
    }
}