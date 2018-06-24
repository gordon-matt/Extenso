using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Extenso
{
    /// <summary>
    /// Provides a set of static methods for querying and manipulating enumerations.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Gets a value that is used for display in the UI. If the given enum value is decorated with a
        /// System.ComponentModel.DataAnnotations.DisplayAttribute, the Name property of that is used.
        /// Otherwise, the text value of the enumeration is treated as pascal case and thus spaces
        /// are added in front of each capital letter.
        /// </summary>
        /// <typeparam name="T">The type of the enumeration.</typeparam>
        /// <param name="enum">The enumeration member to get the display name for.</param>
        /// <returns>A System.String that is used for display in the UI.</returns>
        public static string GetDisplayName<T>(T @enum)
        {
            if (!(@enum is Enum))
            {
                return @enum.ToString();
            }

            var field = typeof(T).GetField(@enum.ToString());

            var displayAttribute = field.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault();
            if (displayAttribute != null)
            {
                var attribute = (DisplayAttribute)displayAttribute;

                string displayName = attribute.Name;
                if (!string.IsNullOrEmpty(displayName))
                {
                    return displayName;
                }
            }

            return @enum.ToString().SpacePascal();
        }

        /// <summary>
        /// Gets a value that is used for display in the UI. If the given enum value is decorated with a
        /// System.ComponentModel.DataAnnotations.DisplayAttribute, the Name property of that is used.
        /// Otherwise, the text value of the enum value is treated as pascal case and thus spaces
        /// are added in front of each capital letter.
        /// </summary>
        /// <typeparam name="T">The type of the enumeration.</typeparam>
        /// <param name="enum">The enumeration member to get the display name for.</param>
        /// <param name="order">
        /// When this method returns, order contains a System.Int32 whose value can be used to place the return value above or below other values retrieved from this method.
        /// </param>
        /// <returns>A System.String that is used for display in the UI.</returns>
        public static string GetDisplayName<T>(T @enum, out int order)
        {
            order = 0;
            if (!(@enum is Enum))
            {
                return @enum.ToString();
            }

            var field = typeof(T).GetField(@enum.ToString());

            var displayAttribute = field.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault();
            if (displayAttribute != null)
            {
                var attribute = (DisplayAttribute)displayAttribute;

                int? displayOrder = attribute.GetOrder();
                order = displayOrder ?? 0;

                string displayName = attribute.Name;
                if (!string.IsNullOrEmpty(displayName))
                {
                    return displayName;
                }
            }

            return @enum.ToString().SpacePascal();
        }

        /// <summary>
        /// Gets a collection of values that are used for display in the UI. If an given enum value is decorated with a
        /// System.ComponentModel.DataAnnotations.DisplayAttribute, the Name property of that is used.
        /// Otherwise, the text value of the enum value is treated as pascal case and thus spaces
        /// are added in front of each capital letter.
        /// </summary>
        /// <typeparam name="T">The type of the enumeration.</typeparam>
        /// <returns>A collection of strings that are used for display in the UI.</returns>
        public static IEnumerable<string> GetDisplayNames<T>()
        {
            return GetValues<T>().Select(x => GetDisplayName(x));
        }

        // https://stackoverflow.com/questions/4171140/iterate-over-values-in-flags-enum
        /// <summary>
        /// Gets a collection of individual values represented by the given bit field (set of flags).
        /// </summary>
        /// <typeparam name="T">The type of the enumeration. This should match the type of source.</typeparam>
        /// <param name="source">The enum value which is a set of flags from which individual values are to be extracted.</param>
        /// <returns>A collection of individual values extracted from source.</returns>
        public static IEnumerable<T> GetFlags<T>(this Enum source)
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("The generic type parameter must be an Enum.");
            }

            if (source.GetType() != typeof(T))
            {
                throw new ArgumentException("The generic type parameter does not match the target type.");
            }

            ulong flag = 1;
            foreach (var value in Enum.GetValues(source.GetType()).Cast<T>())
            {
                ulong bits = Convert.ToUInt64(value);
                while (flag < bits)
                {
                    flag <<= 1;
                }

                if (flag == bits && source.HasFlag(value as Enum))
                {
                    yield return value;
                }
            }
        }

        /// <summary>
        /// Retrieves a collection of the values of the constants in a specified enumeration.
        /// </summary>
        /// <typeparam name="T">The type of the enumeration.</typeparam>
        /// <returns>A collection that contains the values of the constants in the specified enumeration type.</returns>
        public static IEnumerable<T> GetValues<T>()
        {
            return (T[])Enum.GetValues(typeof(T));
        }

        /// <summary>
        /// Converts the string representation of the name or numeric value of one or more
        /// enumerated constants to an equivalent enumerated object. A parameter specifies
        /// whether the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="T">The type of the enumeration.</typeparam>
        /// <param name="source">The string containing the name or value to convert.</param>
        /// <param name="ignoreCase">true to ignore case; false to regard case.</param>
        /// <returns>An object of the specified type whose value is represented by source.</returns>
        public static T Parse<T>(string source, bool ignoreCase = true) where T : struct
        {
            return (T)Enum.Parse(typeof(T), source, ignoreCase);
        }

        /// <summary>
        /// Converts the string representation of the name or numeric value of one or more
        /// enumerated constants to an equivalent enumerated object. A parameter specifies
        /// whether the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="T">The type of the enumeration.</typeparam>
        /// <param name="source">The string containing the name or value to convert.</param>
        /// <param name="ignoreCase">true to ignore case; false to regard case.</param>
        /// <returns>An object of type T whose value is represented by source.</returns>
        public static T ToEnum<T>(this string source, bool ignoreCase = true) where T : struct
        {
            return Parse<T>(source, ignoreCase);
        }

        /// <summary>
        /// Converts the string representation of the name or numeric value of one or more
        /// enumerated constants to an equivalent enumerated object. A parameter specifies
        /// whether the operation is case-sensitive. Another parameter specifies the
        /// value to use in case the operation fails.
        /// </summary>
        /// <typeparam name="T">The type of the enumeration.</typeparam>
        /// <param name="source">The string containing the name or value to convert.</param>
        /// <param name="ignoreCase">true to ignore case; false to regard case.</param>
        /// <param name="fallback">The value to use in case the operation fails.</param>
        /// <returns>An object of type T whose value is represented by source.</returns>
        public static T ToEnum<T>(this string source, bool ignoreCase = true, T fallback = default(T)) where T : struct
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                return fallback;
            }

            T result;
            if (TryParse(source, out result, ignoreCase))
            {
                return result;
            }

            return fallback;
        }

        /// <summary>
        /// Converts the string representation of the name or numeric value of one or more
        /// enumerated constants to an equivalent enumerated object. A parameter specifies
        /// whether the operation is case-sensitive. The return value indicates whether the
        /// conversion succeeded.
        /// </summary>
        /// <typeparam name="T">The type of the enumeration.</typeparam>
        /// <param name="source">The string representation of the enumeration name or underlying value to convert.</param>
        /// <param name="result">
        /// When this method returns, result contains an object of type T whose value
        /// is represented by value if the parse operation succeeds. If the parse operation
        /// fails, result contains the default value of the underlying type of T. Note
        /// that this value need not be a member of the T enumeration. This parameter
        /// is passed uninitialized.
        /// </param>
        /// <param name="ignoreCase">true to ignore case; false to consider case.</param>
        /// <returns>true if the value parameter was converted successfully; otherwise, false.</returns>
        public static bool TryParse<T>(string source, out T result, bool ignoreCase = true) where T : struct
        {
            return Enum.TryParse(source, ignoreCase, out result);
        }
    }
}