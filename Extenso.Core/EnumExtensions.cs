using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Extenso.Collections;

namespace Extenso
{
    public static class EnumExtensions
    {
        public static T ToEnum<T>(this string str, bool ignoreCase = true) where T : struct
        {
            return Parse<T>(str, ignoreCase);
        }

        public static T ToEnum<T>(this string str, T fallback, bool ignoreCase = true) where T : struct
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return fallback;
            }

            T result;
            if (TryParse<T>(str, out result, ignoreCase))
            {
                return result;
            }
            return fallback;
        }

        public static T[] GetValues<T>()
        {
            return (T[])Enum.GetValues(typeof(T));
        }

        public static T Parse<T>(string value, bool ignoreCase = true) where T : struct
        {
            return (T)Enum.Parse(typeof(T), value, ignoreCase);
        }

        public static bool TryParse<T>(string value, out T result, bool ignoreCase = true) where T : struct
        {
            return Enum.TryParse<T>(value, ignoreCase, out result);
        }

        public static IEnumerable<string> GetValuesAsWords<T>() where T : struct
        {
            var values = new List<string>();
            GetValues<T>().ForEach(item => values.Add(item.ToString().SpacePascal()));
            return values;
        }

        public static IEnumerable<Enum> GetFlags(this Enum value)
        {
            if (value == null)
            {
                return Enumerable.Empty<Enum>();
            }

            return GetFlags(value, Enum.GetValues(value.GetType()).Cast<Enum>().ToArray());
        }

        public static IEnumerable<Enum> GetIndividualFlags(this Enum value)
        {
            if (value == null)
            {
                return Enumerable.Empty<Enum>();
            }

            return GetFlags(value, GetFlagValues(value.GetType()).ToArray());
        }

        private static IEnumerable<Enum> GetFlags(Enum value, Enum[] values)
        {
            ulong bits = Convert.ToUInt64(value);
            var results = new List<Enum>();
            for (int i = values.Length - 1; i >= 0; i--)
            {
                ulong mask = Convert.ToUInt64(values[i]);
                if (i == 0 && mask == 0L)
                    break;
                if ((bits & mask) == mask)
                {
                    results.Add(values[i]);
                    bits -= mask;
                }
            }
            if (bits != 0L)
                return Enumerable.Empty<Enum>();
            if (Convert.ToUInt64(value) != 0L)
                return results.Reverse<Enum>();
            if (bits == Convert.ToUInt64(value) && values.Length > 0 && Convert.ToUInt64(values[0]) == 0L)
                return values.Take(1);
            return Enumerable.Empty<Enum>();
        }

        private static IEnumerable<Enum> GetFlagValues(Type enumType)
        {
            ulong flag = 0x1;
            foreach (var value in Enum.GetValues(enumType).Cast<Enum>())
            {
                ulong bits = Convert.ToUInt64(value);
                if (bits == 0L)

                    //yield return value;
                    continue; // skip the zero value
                while (flag < bits) flag <<= 1;
                if (flag == bits)
                    yield return value;
            }
        }

        public static string GetDisplayName<T>(T value)
        {
            if (!(value is Enum))
            {
                return value.ToString();
            }

            var field = typeof(T).GetField(value.ToString());

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

            return value.ToString().SpacePascal();
        }

        public static string GetDisplayName<T>(T value, out int order)
        {
            order = 0;
            if (!(value is Enum))
            {
                return value.ToString();
            }

            var field = typeof(T).GetTypeInfo().GetField(value.ToString());

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

            return value.ToString().SpacePascal();
        }
    }
}