using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Extenso.Collections;

namespace Extenso.Text
{
    public static class StringBuilderExtensions
    {
        public static StringBuilder Append(this StringBuilder stringBuilder, params string[] values)
        {
            foreach (string value in values)
            {
                stringBuilder.Append(value);
            }
            return stringBuilder;
        }

        public static StringBuilder Append(this StringBuilder stringBuilder, params object[] values)
        {
            foreach (object value in values)
            {
                stringBuilder.Append(value);
            }
            return stringBuilder;
        }

        public static StringBuilder Append<T>(this StringBuilder stringBuilder, IEnumerable<T> values)
        {
            return stringBuilder.Append(values.Join());
        }

        public static StringBuilder Append<T>(this StringBuilder stringBuilder, IEnumerable<T> values, Func<T, object> selector)
        {
            return stringBuilder.Append(values.Select(selector).Join());
        }

        public static StringBuilder Append<T>(this StringBuilder stringBuilder, IEnumerable<T> values, string separator)
        {
            return stringBuilder.Append(values.Join(separator));
        }

        public static StringBuilder Append<T>(this StringBuilder stringBuilder, IEnumerable<T> values, string separator, Func<T, object> selector)
        {
            return stringBuilder.Append(values.Select(selector).Join(separator));
        }
    }
}