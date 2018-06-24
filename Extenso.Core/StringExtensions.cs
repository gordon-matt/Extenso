using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using Extenso.Collections;
using Newtonsoft.Json;

namespace Extenso
{
    /// <summary>
    /// Provides a set of static methods for querying and manipulating strings
    /// </summary>
    public static class StringExtensions
    {
        #region Fields

        private const string RegexArabicAndHebrew = @"[\u0600-\u06FF,\u0590-\u05FF]+";
        private static readonly char[] validSegmentChars = "/?#[]@\"^{}|`<>\t\r\n\f ".ToCharArray();

        #endregion Fields

        /// <summary>
        /// Adds a pair of double quotes to the specified System.String.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string AddDoubleQuotes(this string source)
        {
            return $"\"{source}\"";
        }

        /// <summary>
        /// Adds a pair of single quotes to the specified System.String.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string AddSingleQuotes(this string source)
        {
            return $"'{source}'";
        }

        public static bool Any(this string source, params char[] chars)
        {
            if (string.IsNullOrEmpty(source) || chars == null || chars.Length == 0)
            {
                return false;
            }

            Array.Sort(chars);

            for (var i = 0; i < source.Length; i++)
            {
                char current = source[i];
                if (Array.BinarySearch(chars, current) >= 0)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Adds the specified System.String values to the end of this System.String.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string Append(this string source, params string[] values)
        {
            var items = new string[values.Length + 1];
            items[0] = source;
            values.CopyTo(items, 1);
            return string.Concat(items);
        }

        /// <summary>
        /// Adds the specified System.Object values to the end of this System.String.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string Append(this string source, params object[] values)
        {
            var items = new object[values.Length + 1];
            items[0] = source;
            values.CopyTo(items, 1);
            return string.Concat(items);
        }

        public static bool AreAnyNullOrEmpty(params string[] values)
        {
            return values.Any(x => string.IsNullOrEmpty(x));
        }

        public static bool AreAnyNullOrWhiteSpace(params string[] values)
        {
            return values.Any(x => string.IsNullOrWhiteSpace(x));
        }

        //[Obsolete("Use StringExtensions.Base64Deserialize<T> instead")]
        //public static string Base64Decode(this string encodedData)
        //{
        //    if (string.IsNullOrEmpty(encodedData))
        //    {
        //        return encodedData;
        //    }

        //    byte[] bytes = Convert.FromBase64String(encodedData);
        //    return Encoding.UTF8.GetString(bytes);
        //}

        public static T Base64Deserialize<T>(this string source) where T : ISerializable
        {
            // We need to know the exact length of the string - Base64 can sometimes pad us by a byte or two
            int lengthDelimiterPosition = source.IndexOf(':');

            if (lengthDelimiterPosition == -1)
            {
                var bytes = Convert.FromBase64String(source);
                return bytes.BinaryDeserialize<T>();
            }
            else
            {
                int length = int.Parse(source.Substring(0, lengthDelimiterPosition));

                var bytes = Convert.FromBase64String(source.Substring(lengthDelimiterPosition + 1));
                using (var memoryStream = new MemoryStream(bytes, 0, length))
                {
                    var binaryFormatter = new BinaryFormatter();
                    return (T)binaryFormatter.Deserialize(memoryStream);
                }
            }
        }

        //[Obsolete("Use ObjectExtensions.Base64Serialize<T> instead")]
        //public static string Base64Encode(this string plainText)
        //{
        //    if (string.IsNullOrEmpty(plainText))
        //    {
        //        return plainText;
        //    }

        //    byte[] bytes = Encoding.UTF8.GetBytes(plainText);
        //    return Convert.ToBase64String(bytes);
        //}

        //public static T Base64Deserialize<T>(this string s)
        //{
        //    // We need to know the exact length of the string - Base64 can sometimes pad us by a byte or two
        //    int lengthDelimiterPosition = s.IndexOf(':');

        //    if (lengthDelimiterPosition == -1)
        //    {
        //        var bytes = Convert.FromBase64String(s);
        //        return bytes.BinaryDeserialize<T>();
        //    }
        //    else
        //    {
        //        int length = int.Parse(s.Substring(0, lengthDelimiterPosition));

        //        var bytes = Convert.FromBase64String(s.Substring(lengthDelimiterPosition + 1));
        //        using (var memoryStream = new MemoryStream(bytes, 0, length))
        //        {
        //            var binaryFormatter = new BinaryFormatter();
        //            return (T)binaryFormatter.Deserialize(memoryStream);
        //        }
        //    }
        //}

        /// <summary>
        /// Returns the characters between and exclusive of the two search characters; [from] and [to].
        /// </summary>
        /// <param name="source">This System.String.</param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static string Between(this string source, char left, char right)
        {
            int indexFrom = source.IndexOf(left);
            if (indexFrom != -1)
            {
                ++indexFrom;
                int indexTo = source.IndexOf(right, indexFrom);
                if (indexTo != -1)
                {
                    return source.Substring(indexFrom, indexTo - indexFrom);
                }
            }
            return string.Empty;
        }

        public static string CamelFriendly(this string camel)
        {
            if (string.IsNullOrWhiteSpace(camel))
            {
                return string.Empty;
            }

            var sb = new StringBuilder(camel);

            for (int i = camel.Length - 1; i > 0; i--)
            {
                var current = sb[i];
                if ('A' <= current && current <= 'Z')
                {
                    sb.Insert(i, ' ');
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Returns the number of times that the specified character appears in this System.String.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static int CharacterCount(this string source, char c)
        {
            return source.ToCharArray().Where(x => x == c).Count();
        }

        /// <summary>
        /// <para>Returns a value indicating whether the specified System.String object occurs</para>
        /// <para>within this string.</para>
        /// </summary>
        /// <param name="source">This instance of System.String.</param>
        /// <param name="value">The System.String object to seek.</param>
        /// <param name="comparisonType">One of the System.StringComparison values.</param>
        /// <returns>
        /// <para>true if the value parameter occurs within this string, or if value is the</para>
        /// <para>empty string (""); otherwise, false.</para>
        /// </returns>
        public static bool Contains(this string source, string value, StringComparison comparisonType)
        {
            return source.IndexOf(value, comparisonType) != -1;
        }

        /// <summary>
        /// <para>Returns a value indicating whether all of the specified System.String objects</para>
        /// <para>occur within this string.</para>
        /// </summary>
        /// <param name="source">The string</param>
        /// <param name="values">The strings to seek</param>
        /// <returns>true if all values are contained in this string; otherwise, false.</returns>
        public static bool ContainsAll(this string source, params string[] values)
        {
            foreach (string value in values)
            {
                if (!source.Contains(value))
                { return false; }
            }
            return true;
        }

        /// <summary>
        /// <para>Returns a value indicating whether all of the specified System.Char objects</para>
        /// <para>occur within this string.</para>
        /// </summary>
        /// <param name="source">The string</param>
        /// <param name="values">The characters to seek</param>
        /// <returns>true if all values are contained in this string; otherwise, false.</returns>
        public static bool ContainsAll(this string source, params char[] values)
        {
            foreach (char value in values)
            {
                if (!source.Contains(value.ToString()))
                { return false; }
            }
            return true;
        }

        /// <summary>
        /// <para>Returns a value indicating whether any of the specified System.String objects</para>
        /// <para>occur within this string.</para>
        /// </summary>
        /// <param name="source">The string</param>
        /// <param name="values">The strings to seek</param>
        /// <returns>true if any value is contained in this string; otherwise, false.</returns>
        public static bool ContainsAny(this string source, params string[] values)
        {
            foreach (string value in values)
            {
                if (source.Contains(value))
                { return true; }
            }
            return false;
        }

        /// <summary>
        /// <para>Returns a value indicating whether any of the specified System.Char objects</para>
        /// <para>occur within this string.</para>
        /// </summary>
        /// <param name="source">The string</param>
        /// <param name="values">The characters to seek</param>
        /// <returns>true if any value is contained in this string; otherwise, false.</returns>
        public static bool ContainsAny(this string source, params char[] values)
        {
            foreach (char value in values)
            {
                if (source.Contains(value.ToString()))
                { return true; }
            }
            return false;
        }

        /// <summary>
        /// <para>Returns a value indicating whether any of the System.String objects from the</para>
        /// <para>specified IEnumerable&lt;string&gt; occur within this string.</para>
        /// </summary>
        /// <param name="source">The string</param>
        /// <param name="values">The strings to seek</param>
        /// <returns>true if any value is contained in this string; otherwise, false.</returns>
        public static bool ContainsAny(this string source, IEnumerable<string> values)
        {
            foreach (string value in values)
            {
                if (string.IsNullOrEmpty(value))
                { continue; }
                if (source.Contains(value))
                { return true; }
            }
            return false;
        }

        public static bool ContainsWhiteSpace(this string source)
        {
            return source.Any(char.IsWhiteSpace);
        }

        public static string DeflateCompress(this string source)
        {
            var bytes = Encoding.UTF8.GetBytes(source);

            using (var memoryStream = new MemoryStream())
            {
                using (var deflateStream = new DeflateStream(memoryStream, CompressionMode.Compress, true))
                {
                    deflateStream.Write(bytes, 0, bytes.Length);
                }
                memoryStream.Position = 0;
                var compressed = new byte[memoryStream.Length];
                memoryStream.Read(compressed, 0, compressed.Length);
                var gZipBuffer = new byte[compressed.Length + 4];
                Buffer.BlockCopy(compressed, 0, gZipBuffer, 4, compressed.Length);
                Buffer.BlockCopy(BitConverter.GetBytes(bytes.Length), 0, gZipBuffer, 0, 4);
                return Convert.ToBase64String(gZipBuffer);
            }
        }

        public static string DeflateDecompress(this string source)
        {
            var compressedBuffer = Convert.FromBase64String(source);
            using (var memoryStream = new MemoryStream())
            {
                int dataLength = BitConverter.ToInt32(compressedBuffer, 0);
                memoryStream.Write(compressedBuffer, 4, compressedBuffer.Length - 4);
                var buffer = new byte[dataLength];
                memoryStream.Position = 0;
                using (var deflateStream = new DeflateStream(memoryStream, CompressionMode.Decompress))
                {
                    deflateStream.Read(buffer, 0, buffer.Length);
                }
                return Encoding.UTF8.GetString(buffer);
            }
        }

        /// <summary>
        /// <para>Determines whether the end of this string instance matches</para>
        /// <para>one of the specified strings.</para>
        /// </summary>
        /// <param name="source">The string</param>
        /// <param name="values">The strings to compare</param>
        /// <returns>true if any value matches the end of this string; otherwise, false.</returns>
        public static bool EndsWithAny(this string source, params string[] values)
        {
            foreach (string value in values)
            {
                if (source.EndsWith(value))
                { return true; }
            }
            return false;
        }

        public static string GZipCompress(this string source)
        {
            var bytes = Encoding.UTF8.GetBytes(source);

            using (var memoryStream = new MemoryStream())
            {
                using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
                {
                    gZipStream.Write(bytes, 0, bytes.Length);
                }
                memoryStream.Position = 0;
                var compressed = new byte[memoryStream.Length];
                memoryStream.Read(compressed, 0, compressed.Length);
                var gZipBuffer = new byte[compressed.Length + 4];
                Buffer.BlockCopy(compressed, 0, gZipBuffer, 4, compressed.Length);
                Buffer.BlockCopy(BitConverter.GetBytes(bytes.Length), 0, gZipBuffer, 0, 4);
                return Convert.ToBase64String(gZipBuffer);
            }
        }

        public static string GZipDecompress(this string source)
        {
            var gZipBuffer = Convert.FromBase64String(source);
            using (var memoryStream = new MemoryStream())
            {
                int dataLength = BitConverter.ToInt32(gZipBuffer, 0);
                memoryStream.Write(gZipBuffer, 4, gZipBuffer.Length - 4);
                var buffer = new byte[dataLength];
                memoryStream.Position = 0;
                using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                {
                    gZipStream.Read(buffer, 0, buffer.Length);
                }
                return Encoding.UTF8.GetString(buffer);
            }
        }

        /// <summary>
        /// Converts a string that has been HTML-encoded for HTTP transmission into a decoded string.
        /// </summary>
        /// <param name="source">The string to decode.</param>
        /// <returns>A decoded string</returns>
        public static string HtmlDecode(this string source)
        {
            return HttpUtility.HtmlDecode(source);
        }

        /// <summary>
        /// Converts a string to an HTML-encoded string.
        /// </summary>
        /// <param name="source">The string to encode.</param>
        /// <returns>An encoded string.</returns>
        public static string HtmlEncode(this string source)
        {
            return HttpUtility.HtmlEncode(source);
        }

        //public static bool IsNullOrUndefined(this string source)
        //{
        //    if (string.IsNullOrWhiteSpace(source) || source.ToLowerInvariant() == "undefined")
        //    {
        //        return true;
        //    }
        //    return false;
        //}

        /// <summary>
        /// <para>Indicates whether a specified string is null, empty, or consists only of</para>
        /// <para>white-space characters.</para>
        /// </summary>
        /// <param name="source">The string to test.</param>
        /// <returns>
        /// <para>true if the value parameter is null or System.String.Empty, or if value consists</para>
        /// <para>exclusively of white-space characters.</para>
        /// </returns>
        public static bool IsNullOrWhiteSpace(this string source)
        {
            if (source != null)
            {
                return source.All(char.IsWhiteSpace);
            }
            return true;
        }

        public static bool IsRightToLeft(this string source)
        {
            if (Regex.IsMatch(source, RegexArabicAndHebrew, RegexOptions.IgnoreCase))
            {
                return true;
            }
            return false;
        }

        public static bool IsValidUrlSegment(this string source)
        {
            // valid isegment from rfc3987 - http://tools.ietf.org/html/rfc3987#page-8
            // the relevant bits:
            // isegment    = *ipchar
            // ipchar      = iunreserved / pct-encoded / sub-delims / ":" / "@"
            // iunreserved = ALPHA / DIGIT / "-" / "." / "_" / "~" / ucschar
            // pct-encoded = "%" HEXDIG HEXDIG
            // sub-delims  = "!" / "$" / "&" / "'" / "(" / ")" / "*" / "+" / "," / ";" / "="
            // ucschar     = %xA0-D7FF / %xF900-FDCF / %xFDF0-FFEF / %x10000-1FFFD / %x20000-2FFFD / %x30000-3FFFD / %x40000-4FFFD / %x50000-5FFFD / %x60000-6FFFD / %x70000-7FFFD / %x80000-8FFFD / %x90000-9FFFD / %xA0000-AFFFD / %xB0000-BFFFD / %xC0000-CFFFD / %xD0000-DFFFD / %xE1000-EFFFD
            //
            // rough blacklist regex == m/^[^/?#[]@"^{}|\s`<>]+$/ (leaving off % to keep the regex simple)

            return !source.Any(validSegmentChars);
        }

        /// <summary>
        /// Encodes a string for JavaScript.
        /// </summary>
        /// <param name="source">A string to encode.</param>
        /// <returns></returns>
        public static string JavaScriptStringEncode(this string source)
        {
            return HttpUtility.JavaScriptStringEncode(source);
        }

        public static T JsonDeserialize<T>(this string source, JsonSerializerSettings settings = null)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                return default(T);
            }

            if (settings == null)
            {
                return JsonConvert.DeserializeObject<T>(source);
            }

            return JsonConvert.DeserializeObject<T>(source, settings);
        }

        public static object JsonDeserialize(this string source, Type type, JsonSerializerSettings settings = null)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                return null;
            }

            if (settings == null)
            {
                return JsonConvert.DeserializeObject(source, type);
            }

            return JsonConvert.DeserializeObject(source, type, settings);
        }

        /// <summary>
        /// Gets specified number of characters from left of string
        /// </summary>
        /// <param name="source"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static string Left(this string source, int count)
        {
            if (source == null)
            {
                return null;
            }

            if (source.Length <= count)
            {
                return source;
            }

            return source.Substring(0, count);
        }

        /// <summary>
        /// Returns all characters to the left of the first occurrence of [value] in this System.String.
        /// </summary>
        /// <param name="source">This System.String.</param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string LeftOf(this string source, char value)
        {
            if (source == null)
            {
                return null;
            }

            int index = source.IndexOf(value);
            if (index != -1)
            {
                return source.Substring(0, index);
            }
            return source;
        }

        /// <summary>
        /// Returns all characters to the left of the [n]th occurrence of [value] in this System.String.
        /// </summary>
        /// <param name="source">This System.String.</param>
        /// <param name="value"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static string LeftOf(this string source, char value, int n)
        {
            if (source == null)
            {
                return null;
            }

            int index = -1;
            while (n > 0)
            {
                index = source.IndexOf(value, index + 1);
                if (index == -1)
                { break; }
                --n;
            }
            if (index != -1)
            {
                return source.Substring(0, index);
            }
            return source;
        }

        /// <summary>
        /// Returns all characters to the left of the first occurrence of [value] in this System.String.
        /// </summary>
        /// <param name="source">This System.String.</param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string LeftOf(this string source, string value)
        {
            if (value == null)
            {
                return null;
            }

            int index = source.IndexOf(value);
            if (index != -1)
            {
                return source.Substring(0, index);
            }
            return source;
        }

        /// <summary>
        /// Returns all characters to the left of the last occurrence of [value] in this System.String.
        /// </summary>
        /// <param name="source">This System.String.</param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string LeftOfLastIndexOf(this string source, char value)
        {
            if (source == null)
            {
                return null;
            }

            string ret = source;
            int index = source.LastIndexOf(value);
            if (index != -1)
            {
                ret = source.Substring(0, index);
            }
            return ret;
        }

        /// <summary>
        /// Returns all characters to the left of the last occurrence of [value] in this System.String.
        /// </summary>
        /// <param name="source">This System.String.</param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string LeftOfLastIndexOf(this string source, string value)
        {
            if (source == null)
            {
                return null;
            }

            string ret = source;
            int index = source.LastIndexOf(value);
            if (index != -1)
            {
                ret = source.Substring(0, index);
            }
            return ret;
        }

        public static T? ParseNullable<T>(this string source) where T : struct
        {
            var result = new T?();
            try
            {
                if (!string.IsNullOrWhiteSpace(source))
                {
                    TypeConverter conv = TypeDescriptor.GetConverter(typeof(T));
                    result = (T)conv.ConvertFrom(source);
                }
            }
            catch { }
            return result;
        }

        /// <summary>
        /// Adds the specified System.String values to the beginning of this System.String.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string Prepend(this string source, params string[] values)
        {
            var items = new string[values.Length + 1];
            values.CopyTo(items, 0);
            items[items.Length - 1] = source;
            return string.Concat(items);
        }

        /// <summary>
        /// Adds the specified System.Object values to the beginning of this System.String.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string Prepend(this string source, params object[] values)
        {
            var items = new object[values.Length + 1];
            values.CopyTo(items, 0);
            items[items.Length - 1] = source;
            return string.Concat(items);
        }

        /// <summary>
        /// <para>Escapes a minimal set of metacharacters (\, *, +, ?, |, {, [, (,), ^, $,.,</para>
        /// <para>#, and white space) by replacing them with their escape codes.</para>
        /// </summary>
        /// <param name="source">The input string containing the text to convert.</param>
        /// <returns>A string of characters with any metacharacters converted to their escaped form.</returns>
        public static string RegexEscape(this string source)
        {
            return Regex.Escape(source);
        }

        /// <summary>
        /// Unescapes any escaped characters in the input string (for Regex).
        /// </summary>
        /// <param name="source">The input string containing the text to convert.</param>
        /// <returns>A string of characters with any escaped characters converted to their unescaped form.</returns>
        public static string RegexUnescape(this string source)
        {
            return Regex.Unescape(source);
        }

        public static string RemoveBetween(this string source, char begin, char end)
        {
            var regex = new Regex(string.Format("\\{0}.*?\\{1}", begin, end));
            return regex.Replace(source, string.Empty);
        }

        public static string RemoveTags(this string html)
        {
            if (string.IsNullOrEmpty(html))
            {
                return string.Empty;
            }

            var result = new char[html.Length];

            var cursor = 0;
            var inside = false;
            for (var i = 0; i < html.Length; i++)
            {
                char current = html[i];

                switch (current)
                {
                    case '<': inside = true; continue;
                    case '>': inside = false; continue;
                }

                if (!inside)
                {
                    result[cursor++] = current;
                }
            }

            return new string(result, 0, cursor);
        }

        /// <summary>
        /// <para>Takes a System.String and returns a new System.String of the original</para>
        /// <para>repeated [n] number of times</para>
        /// </summary>
        /// <param name="source">The String</param>
        /// <param name="count">The number of times to repeat the String</param>
        /// <returns>A new System.String of the original repeated [n] number of times</returns>
        public static string Repeat(this string source, byte count)
        {
            if (count == 0)
            {
                return string.Empty;
            }

            var sb = new StringBuilder(source.Length * count);
            for (int i = 0; i < count; i++)
            {
                sb.Append(source);
            }

            return sb.ToString();
        }

        /// <summary>
        /// <para>Replaces all occurrences of the specified System.Strings in this instance</para>
        /// <para>with specified System.Strings from the given System.Collections.Generic.IDictionary.</para>
        /// </summary>
        /// <param name="source">This System.String instance</param>
        /// <param name="replacements">The given IDictionary. Keys found in this System.String will be replaced by corresponding Values</param>
        /// <returns></returns>
        public static string Replace(this string source, IDictionary<string, string> replacements)
        {
            var regex = new Regex(replacements.Keys.Join(a => string.Concat("(", Regex.Escape(a), ")"), "|"));
            return regex.Replace(source, m => replacements[m.Value]);
        }

        /// <summary>
        /// Gets specified number of characters from right of string
        /// </summary>
        /// <param name="source"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static string Right(this string source, int count)
        {
            if (source == null)
            {
                return null;
            }

            if (source.Length <= count)
            {
                return source;
            }

            return source.Substring(source.Length - count, count);
        }

        /// <summary>
        /// Returns all characters to the right of the first occurrence of [value] in this System.String.
        /// </summary>
        /// <param name="source">This System.String.</param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string RightOf(this string source, char value)
        {
            if (source == null)
            {
                return null;
            }

            int index = source.IndexOf(value);
            if (index != -1)
            {
                return source.Substring(index + 1);
            }
            return source;
        }

        /// <summary>
        /// Returns all characters to the right of the [n]th occurrence of [value] in this System.String.
        /// </summary>
        /// <param name="source">This System.String.</param>
        /// <param name="value"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static string RightOf(this string source, char value, int n)
        {
            if (source == null)
            {
                return null;
            }

            int index = -1;
            while (n > 0)
            {
                index = source.IndexOf(value, index + 1);
                if (index == -1)
                { break; }
                --n;
            }

            if (index != -1)
            {
                return source.Substring(index + 1);
            }
            return source;
        }

        /// <summary>
        /// Returns all characters to the right of the first occurrence of [value] in this System.String.
        /// </summary>
        /// <param name="source">This System.String.</param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string RightOf(this string source, string value)
        {
            if (source == null)
            {
                return null;
            }

            int index = source.IndexOf(value);
            if (index != -1)
            {
                return source.Substring(index + 1);
            }
            return source;
        }

        /// <summary>
        /// Returns all characters to the right of the last occurrence of [value] in this System.String.
        /// </summary>
        /// <param name="source">This System.String.</param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string RightOfLastIndexOf(this string source, char value)
        {
            if (source == null)
            {
                return null;
            }

            string ret = string.Empty;
            int index = source.LastIndexOf(value);
            if (index != -1)
            {
                ret = source.Substring(index + 1);
            }
            return ret;
        }

        /// <summary>
        /// Returns all characters to the right of the last occurrence of [value] in this System.String.
        /// </summary>
        /// <param name="source">This System.String.</param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string RightOfLastIndexOf(this string source, string value)
        {
            if (source == null)
            {
                return null;
            }

            string ret = string.Empty;
            int index = source.LastIndexOf(value);
            if (index != -1)
            {
                ret = source.Substring(index + 1);
            }
            return ret;
        }

        public static string SafeTrim(this string source, params char[] trimChars)
        {
            if (string.IsNullOrEmpty(source))
            {
                return source;
            }
            return source.Trim(trimChars);
        }

        /// <summary>
        /// <para>Give Pascal Text and will return separate words. For example:</para>
        /// <para>MyPascalText will become "My Pascal Text"</para>
        /// </summary>
        /// <param name="pascalText"></param>
        /// <returns></returns>
        public static string SpacePascal(this string pascalText)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < pascalText.Length; i++)
            {
                char a = pascalText[i];
                if (char.IsUpper(a) && i + 1 < pascalText.Length && !char.IsUpper(pascalText[i + 1]))
                {
                    if (sb.Length > 0)
                    { sb.Append(' '); }
                    sb.Append(a);
                }
                else { sb.Append(a); }
            }

            return sb.ToString();
        }

        /// <summary>
        /// <para>Determines whether the beginning of this string instance matches</para>
        /// <para>one of the specified strings.</para>
        /// </summary>
        /// <param name="source">The string</param>
        /// <param name="values">The strings to compare</param>
        /// <returns>true if any value matches the beginning of this string; otherwise, false.</returns>
        public static bool StartsWithAny(this string source, params string[] values)
        {
            foreach (string value in values)
            {
                if (source.StartsWith(value))
                { return true; }
            }
            return false;
        }

        public static string ToCamelCase(this string source)
        {
            string pascal = source.ToPascalCase();
            return string.Concat(pascal[0].ToString().ToLower(), pascal.Substring(1));
        }

        /// <summary>
        /// Writes the instance of System.String to a new file or overwrites the existing file.
        /// </summary>
        /// <param name="source">The string to write to file.</param>
        /// <param name="filePath">The file to write the string to.</param>
        /// <returns>true if successful; otherwise false.</returns>
        public static bool ToFile(this string source, string filePath)
        {
            try
            {
                var directory = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                using (var sw = new StreamWriter(fs))
                {
                    sw.Write(source);
                    sw.Flush();
                    return true;
                }
            }
            catch (IOException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Splits the given string by new line characters and returns the result in an IEnumerable&lt;string&gt;.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<string> ToLines(this string source)
        {
            if (!string.IsNullOrEmpty(source))
            {
                return source.Split(new[] { "\r\n", Environment.NewLine, "\n" }, StringSplitOptions.None);
            }

            return new string[0];
        }

        /// <summary>
        ///  Converts the specified string to Pascal Case.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToPascalCase(this string source)
        {
            return source.ToTitleCase().Replace(" ", string.Empty);
            //return s.SpacePascal().ToTitleCase().Replace(" ", string.Empty);
        }

        public static MemoryStream ToStream(this string source)
        {
            return source.ToStream(Encoding.UTF8);
        }

        public static MemoryStream ToStream(this string source, Encoding encoding)
        {
            byte[] bytes = encoding.GetBytes(source);
            return new MemoryStream(bytes);
        }

        /// <summary>
        /// Converts the specified string to Title Case using the Current Culture.
        /// </summary>
        /// <param name="source">The string to convert.</param>
        /// <returns>The specified string converted to Title Case.</returns>
        public static string ToTitleCase(this string source)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(source);
        }

        /// <summary>
        /// Converts the specified string to Title Case.
        /// </summary>
        /// <param name="source">The string to convert.</param>
        /// <param name="cultureInfo">The System.Globalization.CultureInfo to use for converting to Title Case.</param>
        /// <returns>The specified string converted to Title Case.</returns>
        public static string ToTitleCase(this string source, CultureInfo cultureInfo)
        {
            return cultureInfo.TextInfo.ToTitleCase(source);
        }

        /// <summary>
        /// Encrypts the specified System.String using the TripleDES symmetric algorithm and returns the data as a System.Byte[].
        /// </summary>
        /// <param name="source">The System.String to encrypt.</param>
        /// <param name="encoding">The System.Text.Encoding to use.</param>
        /// <param name="key">The secret key to use for the symmetric algorithm.</param>
        /// <param name="initializationVector">The initialization vector to use for the symmetric algorithm.</param>
        /// <returns>Encryped System.String as a System.Byte[].</returns>
        public static byte[] TripleDESEncrypt(this string source, Encoding encoding, byte[] key, byte[] initializationVector)
        {
            using (var memoryStream = new MemoryStream())
            using (var cryptoStream = new CryptoStream(
                 memoryStream,
                 new TripleDESCryptoServiceProvider().CreateEncryptor(key, initializationVector),
                 CryptoStreamMode.Write))
            {
                byte[] bytes = encoding.GetBytes(source);

                cryptoStream.Write(bytes, 0, bytes.Length);
                cryptoStream.FlushFinalBlock();

                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// Converts a string that has been encoded for transmission in a URL into a decoded string.
        /// </summary>
        /// <param name="source">The string to decode.</param>
        /// <returns>A decoded string.</returns>
        public static string UrlDecode(this string source)
        {
            return HttpUtility.UrlDecode(source);
        }

        /// <summary>
        /// Encodes a URL string.
        /// </summary>
        /// <param name="source">The text to encode.</param>
        /// <returns>An encoded string.</returns>
        public static string UrlEncode(this string source)
        {
            return HttpUtility.UrlEncode(source);
        }

        /// <summary>
        /// Gets the number of words in the specified System.String.
        /// </summary>
        /// <param name="source">The string to get a word count from.</param>
        /// <returns>A System.Int32 specifying the number of words in the given System.String.</returns>
        public static int WordCount(this string source)
        {
            return source.Split(' ').Count();
        }

        /// <summary>
        /// Deserializes the XML data contained by the specified System.String
        /// </summary>
        /// <typeparam name="T">The type of System.Object to be deserialized</typeparam>
        /// <param name="source">The System.String containing XML data</param>
        /// <returns>The System.Object being deserialized.</returns>
        public static T XmlDeserialize<T>(this string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return default(T);
            }

            var locker = new object();
            var stringReader = new StringReader(source);
            var reader = new XmlTextReader(stringReader);
            try
            {
                var xmlSerializer = new XmlSerializer(typeof(T));
                lock (locker)
                {
                    var item = (T)xmlSerializer.Deserialize(reader);
                    reader.Close();
                    return item;
                }
            }
            //catch
            //{
            //    return default(T);
            //}
            finally
            {
                reader.Close();
            }
        }

        /// <summary>
        /// Deserializes the XML data contained by the specified System.String
        /// </summary>
        /// <param name="source">The System.String containing XML data</param>
        /// <param name="type"></param>
        /// <returns>The System.Object being deserialized.</returns>
        public static object XmlDeserialize(this string source, Type type)
        {
            if (string.IsNullOrEmpty(source))
            {
                return null;
            }

            var locker = new object();
            var stringReader = new StringReader(source);
            var reader = new XmlTextReader(stringReader);
            try
            {
                var xmlSerializer = new XmlSerializer(type);
                lock (locker)
                {
                    var item = xmlSerializer.Deserialize(reader);
                    reader.Close();
                    return item;
                }
            }
            //catch
            //{
            //    return null;
            //}
            finally
            {
                reader.Close();
            }
        }

        #region From: WebMatrix.WebData

        // TODO: Think about removing these...

        //public static TValue As<TValue>(this string source)
        //{
        //    return source.As(default(TValue));
        //}

        //public static TValue As<TValue>(this string source, TValue defaultValue)
        //{
        //    try
        //    {
        //        TypeConverter converter = TypeDescriptor.GetConverter(typeof(TValue));
        //        if (converter.CanConvertFrom(typeof(string)))
        //        {
        //            return (TValue)converter.ConvertFrom(source);
        //        }
        //        converter = TypeDescriptor.GetConverter(typeof(string));
        //        if (converter.CanConvertTo(typeof(TValue)))
        //        {
        //            return (TValue)converter.ConvertTo(source, typeof(TValue));
        //        }
        //    }
        //    catch
        //    {
        //        return defaultValue;
        //    }
        //    return defaultValue;
        //}

        //public static bool AsBool(this string source)
        //{
        //    return source.AsBool(false);
        //}

        //public static bool AsBool(this string source, bool defaultValue)
        //{
        //    bool flag;
        //    if (!bool.TryParse(source, out flag))
        //    {
        //        return defaultValue;
        //    }
        //    return flag;
        //}

        //public static DateTime AsDateTime(this string source)
        //{
        //    return source.AsDateTime(new DateTime());
        //}

        //public static DateTime AsDateTime(this string source, DateTime defaultValue)
        //{
        //    DateTime time;
        //    if (!DateTime.TryParse(source, out time))
        //    {
        //        return defaultValue;
        //    }
        //    return time;
        //}

        //public static decimal AsDecimal(this string source)
        //{
        //    return source.As<decimal>();
        //}

        //public static decimal AsDecimal(this string source, decimal defaultValue)
        //{
        //    return source.As(defaultValue);
        //}

        //public static float AsFloat(this string value)
        //{
        //    return value.AsFloat(0f);
        //}

        //public static float AsFloat(this string source, float defaultValue)
        //{
        //    float num;
        //    if (!float.TryParse(source, out num))
        //    {
        //        return defaultValue;
        //    }
        //    return num;
        //}

        //public static int AsInt(this string source)
        //{
        //    return source.AsInt(0);
        //}

        //public static int AsInt(this string source, int defaultValue)
        //{
        //    int num;
        //    if (!int.TryParse(source, out num))
        //    {
        //        return defaultValue;
        //    }
        //    return num;
        //}

        //public static bool Is<TValue>(this string source)
        //{
        //    TypeConverter converter = TypeDescriptor.GetConverter(typeof(TValue));
        //    try
        //    {
        //        if ((source == null) || converter.CanConvertFrom(null, source.GetType()))
        //        {
        //            converter.ConvertFrom(null, CultureInfo.CurrentCulture, source);
        //            return true;
        //        }
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //    return false;
        //}

        //public static bool IsBool(this string source)
        //{
        //    bool flag;
        //    return bool.TryParse(source, out flag);
        //}

        //public static bool IsDateTime(this string source)
        //{
        //    DateTime time;
        //    return DateTime.TryParse(source, out time);
        //}

        //public static bool IsDecimal(this string source)
        //{
        //    return source.Is<decimal>();
        //}

        //public static bool IsEmpty(this string source)
        //{
        //    return string.IsNullOrEmpty(source);
        //}

        //public static bool IsFloat(this string source)
        //{
        //    float num;
        //    return float.TryParse(source, out num);
        //}

        //public static bool IsInt(this string source)
        //{
        //    int num;
        //    return int.TryParse(source, out num);
        //}

        #endregion From: WebMatrix.WebData

        //#region Pluralization

        //private static IDictionary<string, PluralizationService> pluralizationServices;

        ///// <summary>
        ///// Determines whether the specified word is plural.
        ///// </summary>
        ///// <param name="word">The value to be analyzed.</param>
        ///// <returns>true if the word is plural; otherwise, false.</returns>
        //public static bool IsPlural(this string word, string cultureCode = "en")
        //{
        //    var pluralizationService = GetPluralizationService(cultureCode);
        //    return pluralizationService.IsPlural(word);
        //}

        ///// <summary>
        ///// Determines whether the specified word is singular.
        ///// </summary>
        ///// <param name="word">The value to be analyzed.</param>
        ///// <returns>true if the word is singular; otherwise, false.</returns>
        //public static bool IsSingular(this string word, string cultureCode = "en")
        //{
        //    var pluralizationService = GetPluralizationService(cultureCode);
        //    return pluralizationService.IsSingular(word);
        //}

        ///// <summary>
        ///// Returns the plural form of the specified word.
        ///// </summary>
        ///// <param name="word">The word to be made plural.</param>
        ///// <returns>The plural form of the input parameter.</returns>
        //public static string Pluralize(this string word, string cultureCode = "en")
        //{
        //    if (string.IsNullOrWhiteSpace(word))
        //    {
        //        return word;
        //    }

        //    if (word.IsSingular())
        //    {
        //        var pluralizationService = GetPluralizationService(cultureCode);
        //        return pluralizationService.Pluralize(word);
        //    }
        //    return word;
        //}

        ///// <summary>
        ///// Returns the singular form of the specified word.
        ///// </summary>
        ///// <param name="word">The word to be made singular.</param>
        ///// <returns>The singular form of the input parameter.</returns>
        //public static string Singularize(this string word, string cultureCode = "en")
        //{
        //    if (string.IsNullOrWhiteSpace(word))
        //    {
        //        return word;
        //    }
        //    if (word.IsPlural())
        //    {
        //        var pluralizationService = GetPluralizationService(cultureCode);
        //        return pluralizationService.Singularize(word);
        //    }
        //    return word;
        //}

        //private static PluralizationService GetPluralizationService(string cultureCode)
        //{
        //    if (pluralizationServices == null)
        //    {
        //        pluralizationServices = new Dictionary<string, PluralizationService>();
        //    }

        //    if (string.IsNullOrEmpty(cultureCode))
        //    {
        //        cultureCode = "en";
        //    }

        //    if (!pluralizationServices.ContainsKey(cultureCode))
        //    {
        //        var pluralizationService = PluralizationService.CreateService(new CultureInfo(cultureCode));
        //        pluralizationServices.Add(cultureCode, pluralizationService);
        //        return pluralizationService;
        //    }

        //    return pluralizationServices[cultureCode];
        //}

        //#endregion Pluralization
    }
}