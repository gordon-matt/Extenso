using System;
using System.Reflection;

namespace Extenso.Reflection
{
    public static class StringExtensions
    {
        public static object ParseOrDefault(this string input, Type type)
        {
            MethodInfo parseMethod = type.GetTypeInfo().GetMethod("Parse", new Type[] { typeof(string) });

            if (parseMethod != null)
            {
                return parseMethod.Invoke(null, new string[] { input });
            }
            return null;
        }

        public static TOut ParseOrDefault<TOut>(this string input)
        {
            return input.ParseOrDefault(default(TOut));
        }

        public static TOut ParseOrDefault<TOut>(this string input, TOut defaultValue)
        {
            Type type = typeof(TOut);
            MethodInfo parseMethod = type.GetTypeInfo().GetMethod("Parse", new Type[] { typeof(string) });

            if (parseMethod != null)
            {
                var value = parseMethod.Invoke(null, new string[] { input });
                return (value is TOut ? (TOut)value : defaultValue);
            }
            else { return defaultValue; }
        }

        public static bool TryParseOrDefault<TOut>(this string input, out TOut output)
        {
            return input.TryParseOrDefault(out output, default(TOut));
        }

        public static bool TryParseOrDefault<TOut>(this string input, out TOut output, TOut defaultValue)
        {
            output = defaultValue;

            Type type = typeof(TOut);
            MethodInfo parseMethod = type.GetTypeInfo().GetMethod(
                "TryParse",
                new Type[] { typeof(string), typeof(TOut).MakeByRefType() });

            if (parseMethod != null)
            {
                object[] parameters = new object[] { input, output };
                var value = parseMethod.Invoke(null, parameters);

                if (value is bool)
                {
                    bool successful = (bool)value;
                    if (successful)
                    {
                        output = (TOut)parameters[1];
                        return true;
                    }
                }
            }
            return false;
        }
    }
}