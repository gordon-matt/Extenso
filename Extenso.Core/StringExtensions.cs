using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Serialization;
using Extenso.Collections;
using Extenso.IO;
using Newtonsoft.Json;

namespace Extenso;

/// <summary>
/// Provides a set of static methods for querying and manipulating strings
/// </summary>
public static class StringExtensions
{
    #region Fields

    private const string RegexArabicAndHebrew = /*lang=regex*/ @"[\u0600-\u06FF,\u0590-\u05FF]+";
    //private static readonly char[] validSegmentChars = "/?#[]@\"^{}|`<>\t\r\n\f ".ToCharArray();

    #endregion Fields

    /// <summary>
    /// Appends copies of the specified strings to the given string.
    /// </summary>
    /// <param name="source">The string to append values to.</param>
    /// <param name="values">An array of strings to append to source.</param>
    /// <returns>A new string after the append operation has completed.</returns>
    public static string Append(this string source, params ReadOnlySpan<object> values) => string.Concat([source, .. values]);

    /// <summary>
    /// Gets a value indicating whether any of the given values are null or an empty string.
    /// </summary>
    /// <param name="values">The strings to test.</param>
    /// <returns>true if any of the values are null or an empty string (""); otherwise, false.</returns>
    /// <example><code>bool valid = StringExtensions.AreAnyNullOrEmpty(foo, bar, baz);</code></example>
    public static bool AreAnyNullOrEmpty(params string[] values) => values.Any(x => string.IsNullOrEmpty(x));

    /// <summary>
    /// Gets a value indicating whether any of the given values are null, empty or consists only of white-space characters.
    /// </summary>
    /// <param name="values">The strings to test.</param>
    /// <returns>true if any of the values are null, an empty string ("") or consist exclusively of white-space characters.</returns>
    /// <example><code>bool valid = StringExtensions.AreAnyNullOrWhiteSpace(foo, bar, baz);</code></example>
    public static bool AreAnyNullOrWhiteSpace(params string[] values) => values.Any(x => string.IsNullOrWhiteSpace(x));

    /// <summary>
    /// Decodes the Base64 encoded string to a byte array.
    /// </summary>
    /// <param name="source">The Base64 encoded string to decode.</param>
    /// <returns>The decoded bytes from the Base64 encoded string.</returns>
    public static byte[] Base64Deserialize(this string source)
    {
        // We need to know the exact length of the string - Base64 can sometimes pad us by a byte or two
        int lengthDelimiterPosition = source.IndexOf(':');

        return lengthDelimiterPosition == -1
            ? Convert.FromBase64String(source)
            : Convert.FromBase64String(source[(lengthDelimiterPosition + 1)..]);
    }

    /// <summary>
    /// Decodes and deserializes the Base64 encoded string to an object of the specified type.
    /// </summary>
    /// <typeparam name="T">The type of object to decode and deserialize the Base64 encoded string to.</typeparam>
    /// <param name="source">The Base64 encoded string to decode and deserialize.</param>
    /// <returns>The decoded and deserialized object from the Base64 encoded string.</returns>
    public static T Base64Deserialize<T>(this string source)
    {
        // We need to know the exact length of the string - Base64 can sometimes pad us by a byte or two
        int lengthDelimiterPosition = source.IndexOf(':');

        if (lengthDelimiterPosition == -1)
        {
            byte[] bytes = Convert.FromBase64String(source);
            return bytes.BinaryDeserialize<T>();
        }
        else
        {
            int length = int.Parse(source[..lengthDelimiterPosition]);
            byte[] bytes = Convert.FromBase64String(source[(lengthDelimiterPosition + 1)..]);
            using var memoryStream = new MemoryStream(bytes, 0, length);
            return memoryStream.BinaryDeserialize<T>();
        }
    }

    /// <summary>
    /// Gets the characters between and exclusive of the two search characters; [left] and [right].
    /// </summary>
    /// <param name="source">The string from which to extract the result.</param>
    /// <param name="left">The character to the left of the desired result.</param>
    /// <param name="right">The character to the right of the desired result.</param>
    /// <returns>All characters in source that occur between [left] and [right].</returns>
    /// <example>
    /// <code>
    /// string test = "[Numero Uno]";
    /// string result = test.Between('[', ']');
    /// </code>
    /// </example>
    public static string Between(this string source, char left, char right)
    {
        int indexFrom = source.IndexOf(left);
        if (indexFrom != -1)
        {
            ++indexFrom;
            int indexTo = source.IndexOf(right, indexFrom);
            if (indexTo != -1)
            {
                return source[indexFrom..indexTo];
            }
        }
        return string.Empty;
    }

    /// <summary>
    /// Compresses the given string using the Brotli algorithm and returns a Base64 encoded string of compressed data.
    /// </summary>
    /// <param name="source">The string to compress.</param>
    /// <returns>A Base64 encoded string of compressed data.</returns>
    public static string BrotliCompress(this string source)
    {
        if (string.IsNullOrEmpty(source))
        {
            return null;
        }

        byte[] bytes = Encoding.UTF8.GetBytes(source);

        using var memoryStream = new MemoryStream();
        using (var gZipStream = new BrotliStream(memoryStream, CompressionMode.Compress, true))
        {
            gZipStream.Write(bytes, 0, bytes.Length);
        }
        memoryStream.Position = 0;
        byte[] compressed = new byte[memoryStream.Length];
        memoryStream.Read(compressed, 0, compressed.Length);
        byte[] gZipBuffer = new byte[compressed.Length + 4];
        Buffer.BlockCopy(compressed, 0, gZipBuffer, 4, compressed.Length);
        Buffer.BlockCopy(BitConverter.GetBytes(bytes.Length), 0, gZipBuffer, 0, 4);
        return Convert.ToBase64String(gZipBuffer);
    }

    /// <summary>
    /// Decompresses the given Base64 encoded string using the Brotli algorithm and returns a string of decompressed data.
    /// </summary>
    /// <param name="source">The Base64 encoded string to decompress.</param>
    /// <returns>A string of decompressed data.</returns>
    public static string BrotliDecompress(this string source)
    {
        byte[] gZipBuffer = Convert.FromBase64String(source);
        using var memoryStream = new MemoryStream();
        int dataLength = BitConverter.ToInt32(gZipBuffer, 0);
        memoryStream.Write(gZipBuffer, 4, gZipBuffer.Length - 4);
        byte[] buffer = new byte[dataLength];
        memoryStream.Position = 0;
        using (var gZipStream = new BrotliStream(memoryStream, CompressionMode.Decompress))
        {
            gZipStream.Read(buffer, 0, buffer.Length);
        }
        return Encoding.UTF8.GetString(buffer);
    }

    /// <summary>
    /// Gets a value indicating the number of times that the specified Unicode character appears in the given string.
    /// </summary>
    /// <param name="source">The string to search for the specified Unicode character.</param>
    /// <param name="c">The Unicode character to search the string for.</param>
    /// <returns>A System.Int32 indicating the number of times the specified Unicode character appears in the given string.</returns>
    public static int CharacterCount(this string source, char c) => source.ToCharArray().Where(x => x == c).Count();

    /// <summary>
    /// Returns the first value in the given list that has a valid value.
    /// </summary>
    /// <param name="values">The strings to seek.</param>
    /// <para>The first value in the given list that is not null, empty or consisting only of whitespace.</para>
    /// <para>If none of the values are valid, then null is returned.</para>
    public static string Coalesce(params Span<string> values) => Coalesce(allowWhitespace: false, values);

    /// <summary>
    /// Returns the first value in the given list that has a valid value.
    /// </summary>
    /// <param name="allowWhitespace">Whether values consisting only of whitespace are considered valid or not.</param>
    /// <param name="values">The strings to seek.</param>
    /// <returns>
    /// <para>If allowWhitespace is true, then the first value in the given list that is not null or empty.</para>
    /// <para>Else, the first value in the given list that is not null, empty or consisting only of whitespace.</para>
    /// <para>If none of the values are valid, then null is returned.</para>
    /// </returns>
    public static string Coalesce(bool allowWhitespace, Span<string> values)
    {
        Func<string, bool> operation = allowWhitespace
            ? string.IsNullOrEmpty : string.IsNullOrWhiteSpace;

        foreach (string value in values)
        {
            if (!operation(value))
            {
                return value;
            }
        }

        return null;
    }

    /// <summary>
    /// Returns a value indicating whether a specified substring occurs within the given string.
    /// A parameter specifies the type of search to use for the specified substring.
    /// </summary>
    /// <param name="source">The string to examine.</param>
    /// <param name="value">The string to seek.</param>
    /// <param name="comparisonType">One of the enumeration values that specifies the rules for the search.</param>
    /// <returns>
    /// true if the value parameter occurs within the given string; otherwise, false.
    /// </returns>
    public static bool Contains(this string source, string value, StringComparison comparisonType) => source.IndexOf(value, comparisonType) != -1;

    /// <summary>
    /// Gets a value indicating whether all of the specified strings occur within the given string.
    /// </summary>
    /// <param name="source">The string to examine.</param>
    /// <param name="values">The strings to seek.</param>
    /// <returns>true if all of the specified strings are contained in source; otherwise, false.</returns>
    public static bool ContainsAll(this string source, params Span<string> values)
    {
        foreach (string value in values)
        {
            if (!source.Contains(value))
            { return false; }
        }
        return true;
    }

    /// <summary>
    /// Gets a value indicating whether all of the specified Unicode characters occur within the given string.
    /// </summary>
    /// <param name="source">The string to examine.</param>
    /// <param name="values">The Unicode characters to seek</param>
    /// <returns>true if all of the specified characters are contained in source; otherwise, false.</returns>
    public static bool ContainsAll(this string source, params Span<char> values)
    {
        foreach (char value in values)
        {
            if (!source.Any(x => x == value))
            { return false; }
        }
        return true;
    }

    /// <summary>
    /// Gets a value indicating whether any of the specified strings occur within the given string.
    /// </summary>
    /// <param name="source">The string to examine.</param>
    /// <param name="values">The strings to seek.</param>
    /// <returns>true if any of the specified strings are contained in source; otherwise, false.</returns>
    public static bool ContainsAny(this string source, params Span<string> values)
    {
        foreach (string value in values)
        {
            if (source.Contains(value))
            { return true; }
        }
        return false;
    }

    /// <summary>
    /// Gets a value indicating whether any of the specified Unicode characters occur within the given string.
    /// </summary>
    /// <param name="source">The string to examine.</param>
    /// <param name="chars">The Unicode characters to seek.</param>
    /// <returns>true if any of the specified characters occur within source; otherwise, false.</returns>
    public static bool ContainsAny(this string source, params ReadOnlySpan<char> chars)
    {
        if (string.IsNullOrEmpty(source) || chars.IsEmpty)
        {
            return false;
        }

        Span<char> sortedChars = stackalloc char[chars.Length];
        chars.CopyTo(sortedChars);
        sortedChars.Sort();

        for (int i = 0; i < source.Length; i++)
        {
            if (sortedChars.BinarySearch(source[i]) >= 0)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Gets a value indicating whether any of the specified strings occur within the given string.
    /// </summary>
    /// <param name="source">The string to examine.</param>
    /// <param name="values">The strings to seek.</param>
    /// <returns>true if any of the specified strings are contained in source; otherwise, false.</returns>
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

    /// <summary>
    /// Gets a value indicating whether the given string contains any white-space characters.
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static bool ContainsWhiteSpace(this string source) => source.Any(char.IsWhiteSpace);

    /// <summary>
    /// Compresses the given string using the Deflate algorithm and returns a Base64 encoded string of compressed data.
    /// </summary>
    /// <param name="source">The string to compress.</param>
    /// <returns>A Base64 encoded string of compressed data.</returns>
    public static string DeflateCompress(this string source)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(source);

        using var memoryStream = new MemoryStream();
        using (var deflateStream = new DeflateStream(memoryStream, CompressionMode.Compress, true))
        {
            deflateStream.Write(bytes, 0, bytes.Length);
        }

        memoryStream.Position = 0;
        byte[] compressed = new byte[memoryStream.Length];
        memoryStream.Read(compressed, 0, compressed.Length);
        byte[] gZipBuffer = new byte[compressed.Length + 4];
        Buffer.BlockCopy(compressed, 0, gZipBuffer, 4, compressed.Length);
        Buffer.BlockCopy(BitConverter.GetBytes(bytes.Length), 0, gZipBuffer, 0, 4);
        return Convert.ToBase64String(gZipBuffer);
    }

    /// <summary>
    /// Decompresses the given Base64 encoded string using the Deflate algorithm and returns a string of decompressed data.
    /// </summary>
    /// <param name="source">The Base64 encoded string to decompress.</param>
    /// <returns>A string of decompressed data.</returns>
    public static string DeflateDecompress(this string source)
    {
        byte[] compressedBuffer = Convert.FromBase64String(source);
        using var memoryStream = new MemoryStream();
        int dataLength = BitConverter.ToInt32(compressedBuffer, 0);
        memoryStream.Write(compressedBuffer, 4, compressedBuffer.Length - 4);
        byte[] buffer = new byte[dataLength];
        memoryStream.Position = 0;
        using (var deflateStream = new DeflateStream(memoryStream, CompressionMode.Decompress))
        {
            deflateStream.Read(buffer, 0, buffer.Length);
        }
        return Encoding.UTF8.GetString(buffer);
    }

    ///// <summary>
    ///// Encrypts the given string using the specified System.Security.Cryptography.ICryptoTransform and returns
    ///// the data as a byte array. A parameter specifies the character encoding to use.
    ///// </summary>
    ///// <param name="source">The string to encrypt.</param>
    ///// <param name="encoding">The character encoding to use.</param>
    ///// <param name="cryptoTransform">The System.Security.Cryptography.ICryptoTransform to use.</param>
    ///// <returns>An encryped string as a byte array.</returns>
    //public static byte[] Encrypt(this string source, Encoding encoding, ICryptoTransform cryptoTransform)
    //{
    //    using (var memoryStream = new MemoryStream())
    //    using (var cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
    //    {
    //        byte[] bytes = encoding.GetBytes(source);
    //        cryptoStream.Write(bytes, 0, bytes.Length);
    //        cryptoStream.FlushFinalBlock();
    //        return memoryStream.ToArray();
    //    }
    //}

    ////TODO: More of these (for each SymmetricAlgorithm)? Will need Decrypt() methods in ByteArrayExtensions as well...
    ///// <summary>
    ///// Encrypts the given string using the TripleDES symmetric algorithm and returns the data as a byte array.
    ///// A parameter specifies the character encoding to use.
    ///// </summary>
    ///// <param name="source">The string to encrypt.</param>
    ///// <param name="encoding">The character encoding to use.</param>
    ///// <param name="key">The secret key to use for the symmetric algorithm.</param>
    ///// <param name="initializationVector">The initialization vector to use for the symmetric algorithm.</param>
    ///// <returns>An encryped string as a byte array.</returns>
    //public static byte[] EncryptTripleDES(this string source, Encoding encoding, byte[] key, byte[] initializationVector)
    //{
    //    return source.Encrypt(encoding, TripleDES.Create().CreateEncryptor(key, initializationVector));
    //}

    /// <summary>
    /// Determines whether the end of the given string matches any of the specified strings.
    /// </summary>
    /// <param name="source">The string to examine.</param>
    /// <param name="values">The strings to compare to the substring at the end of [source].</param>
    /// <returns>true if any of the specified strings match the end of the given string; otherwise, false.</returns>
    public static bool EndsWithAny(this string source, params Span<string> values)
    {
        foreach (string value in values)
        {
            if (source.EndsWith(value))
            { return true; }
        }
        return false;
    }

    /// <summary>
    /// Encloses the given System.String in double quotes.
    /// </summary>
    /// <param name="source">The string to be enquoted.</param>
    /// <returns>A new System.String consisting of the original enquoted in double quotes.</returns>
    public static string EnquoteDouble(this string source) => $"\"{source}\"";

    /// <summary>
    /// Encloses the given System.String in single quotes.
    /// </summary>
    /// <param name="source">The string to be enquoted.</param>
    /// <returns>A new System.String consisting of the original enquoted in single quotes.</returns>
    public static string EnquoteSingle(this string source) => $"'{source}'";

    /// <summary>
    /// Compresses the given string using the GZip algorithm and returns a Base64 encoded string of compressed data.
    /// </summary>
    /// <param name="source">The string to compress.</param>
    /// <returns>A Base64 encoded string of compressed data.</returns>
    public static string GZipCompress(this string source)
    {
        if (string.IsNullOrEmpty(source))
        {
            return null;
        }

        byte[] bytes = Encoding.UTF8.GetBytes(source);

        using var memoryStream = new MemoryStream();
        using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
        {
            gZipStream.Write(bytes, 0, bytes.Length);
        }
        memoryStream.Position = 0;
        byte[] compressed = new byte[memoryStream.Length];
        memoryStream.Read(compressed, 0, compressed.Length);
        byte[] gZipBuffer = new byte[compressed.Length + 4];
        Buffer.BlockCopy(compressed, 0, gZipBuffer, 4, compressed.Length);
        Buffer.BlockCopy(BitConverter.GetBytes(bytes.Length), 0, gZipBuffer, 0, 4);
        return Convert.ToBase64String(gZipBuffer);
    }

    /// <summary>
    /// Decompresses the given Base64 encoded string using the GZip algorithm and returns a string of decompressed data.
    /// </summary>
    /// <param name="source">The Base64 encoded string to decompress.</param>
    /// <returns>A string of decompressed data.</returns>
    public static string GZipDecompress(this string source)
    {
        byte[] gZipBuffer = Convert.FromBase64String(source);
        using var memoryStream = new MemoryStream();
        int dataLength = BitConverter.ToInt32(gZipBuffer, 0);
        memoryStream.Write(gZipBuffer, 4, gZipBuffer.Length - 4);
        byte[] buffer = new byte[dataLength];
        memoryStream.Position = 0;
        using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
        {
            gZipStream.Read(buffer, 0, buffer.Length);
        }
        return Encoding.UTF8.GetString(buffer);
    }

    /// <summary>
    /// Converts a string that has been HTML-encoded for HTTP transmission into a decoded string.
    /// </summary>
    /// <param name="source">The string to decode.</param>
    /// <returns>A decoded string</returns>
    public static string HtmlDecode(this string source) => HttpUtility.HtmlDecode(source);

    /// <summary>
    /// Converts a string to an HTML-encoded string.
    /// </summary>
    /// <param name="source">The string to encode.</param>
    /// <returns>An encoded string.</returns>
    public static string HtmlEncode(this string source) => HttpUtility.HtmlEncode(source);

    /// <summary>
    /// Gets a value indicating whether the given string consists of any right-to-left text.
    /// Note: Only Hebrew and Arabic are supported
    /// </summary>
    /// <param name="source">The string to test.</param>
    /// <returns>true if the given string consists of any right-to-left text; otherwise false.</returns>
    public static bool IsRightToLeft(this string source) => Regex.IsMatch(source, RegexArabicAndHebrew, RegexOptions.IgnoreCase);

    /// <summary>
    /// Encodes a string for JavaScript.
    /// </summary>
    /// <param name="source">The string to encode.</param>
    /// <returns>An encoded string.</returns>
    public static string JavaScriptStringEncode(this string source) => HttpUtility.JavaScriptStringEncode(source);

    /// <summary>
    /// Deserializes the JSON to the specified .NET type using Newtonsoft.Json.JsonSerializerSettings.
    /// </summary>
    /// <typeparam name="T">The type of the object to deserialize to.</typeparam>
    /// <param name="source">The JSON to deserialize.</param>
    /// <param name="settings">
    /// The Newtonsoft.Json.JsonSerializerSettings used to deserialize the object. If
    /// this is null, default serialization settings will be used.
    /// </param>
    /// <returns>The deserialized object from the JSON string.</returns>
    public static T JsonDeserialize<T>([StringSyntax(StringSyntaxAttribute.Json)] this string source, JsonSerializerSettings settings = null) => JsonConvert.DeserializeObject<T>(source, settings);

    /// <summary>
    /// Deserializes the JSON to the specified .NET type using Newtonsoft.Json.JsonSerializerSettings.
    /// </summary>
    /// <param name="source">The JSON to deserialize.</param>
    /// <param name="type">The type of the object to deserialize to.</param>
    /// <param name="settings">
    /// The Newtonsoft.Json.JsonSerializerSettings used to deserialize the object. If
    /// this is null, default serialization settings will be used.
    /// </param>
    /// <returns></returns>
    public static object JsonDeserialize([StringSyntax(StringSyntaxAttribute.Json)] this string source, Type type, JsonSerializerSettings settings = null) => JsonConvert.DeserializeObject(source, type, settings);

    /// <summary>
    /// Retrieves a substring from the given string. The substring starts at 0 and has a specified length.
    /// </summary>
    /// <param name="source">The string from which to extract a substring.</param>
    /// <param name="length">The number of characters in the substring.</param>
    /// <returns>A string that is equivalent to the substring of length that starts at 0.</returns>
    public static string Left(this string source, int length) => source == null ? null : source.Length <= length ? source : source[..length];

    /// <summary>
    /// Retrieves a substring from the given string. The substring starts at 0 and ends at the first occurrence of [value].
    /// </summary>
    /// <param name="source">The string from which to extract a substring.</param>
    /// <param name="value">The Unicode character to seek the first occurrence of.</param>
    /// <returns>A substring of [source] that starts at 0 and ends at the first occurrence of [value].</returns>
    public static string LeftOf(this string source, char value)
    {
        if (source == null)
        {
            return null;
        }

        int index = source.IndexOf(value);
        return index != -1 ? source[..index] : source;
    }

    /// <summary>
    /// Retrieves a substring from the given string. The substring starts at 0 and ends at the [n]th occurrence of [value].
    /// </summary>
    /// <param name="source">The string from which to extract a substring.</param>
    /// <param name="value">The Unicode character to seek the [n]th occurrence of.</param>
    /// <param name="n">A System.Int32 indicating which occurence of [value] to seek.</param>
    /// <returns>A substring of [source] that starts at 0 and ends at the [n]th occurrence of [value].</returns>
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
        return index != -1 ? source[..index] : source;
    }

    /// <summary>
    /// Retrieves a substring from the given string. The substring starts at 0 and ends at the first occurrence of [value].
    /// </summary>
    /// <param name="source">The string from which to extract a substring.</param>
    /// <param name="value">The substring to seek the first occurrence of.</param>
    /// <returns>A substring of [source] that starts at 0 and ends at the first occurrence of [value].</returns>
    public static string LeftOf(this string source, string value)
    {
        if (value == null)
        {
            return null;
        }

        int index = source.IndexOf(value);
        return index != -1 ? source[..index] : source;
    }

    /// <summary>
    /// Retrieves a substring from the given string. The substring starts at 0 and ends at the last occurrence of [value].
    /// </summary>
    /// <param name="source">The string from which to extract a substring.</param>
    /// <param name="value">The Unicode character to seek the last occurrence of.</param>
    /// <returns>A substring of [source] that starts at 0 and ends at the last occurrence of [value].</returns>
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
            ret = source[..index];
        }
        return ret;
    }

    /// <summary>
    ///  Retrieves a substring from the given string. The substring starts at 0 and ends at the last occurrence of [value].
    /// </summary>
    /// <param name="source">The string from which to extract a substring.</param>
    /// <param name="value">The substring to seek the last occurrence of.</param>
    /// <returns>A substring of [source] that starts at 0 and ends at the last occurrence of [value].</returns>
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
            ret = source[..index];
        }
        return ret;
    }

    //public static T? ParseNullable<T>(this string source) where T : struct
    //{
    //    var result = new T?();
    //    try
    //    {
    //        if (!string.IsNullOrWhiteSpace(source))
    //        {
    //            TypeConverter conv = TypeDescriptor.GetConverter(typeof(T));
    //            result = (T)conv.ConvertFrom(source);
    //        }
    //    }
    //    catch { }
    //    return result;
    //}

    /// <summary>
    /// Prepends the string representations of the specified objects to the given string.
    /// </summary>
    /// <param name="source">The string to prepend values to.</param>
    /// <param name="values">An array of objects to prepend to source.</param>
    /// <returns>A new string after the prepend operation has completed.</returns>
    public static string Prepend(this string source, params ReadOnlySpan<object> values) => string.Concat([.. values, source]);

    /// <summary>
    /// Escapes a minimal set of characters (\, *, +, ?, |, {, [, (,), ^, $,., #, and
    /// white space) by replacing them with their escape codes. This instructs the regular
    /// expression engine to interpret these characters literally rather than as metacharacters.
    /// </summary>
    /// <param name="source">The input string that contains the text to convert.</param>
    /// <returns>A string of characters with metacharacters converted to their escaped form.</returns>
    public static string RegexEscape(this string source) => Regex.Escape(source);

    /// <summary>
    /// Converts any escaped characters in the input string.
    /// </summary>
    /// <param name="source">The input string containing the text to convert.</param>
    /// <returns>A string of characters with any escaped characters converted to their unescaped form.</returns>
    public static string RegexUnescape(this string source) => Regex.Unescape(source);

    /// <summary>
    /// Initializes a new string to the value indicated by [source] repeated a specified number of times.
    /// </summary>
    /// <param name="source">The string to repeat [count] times.</param>
    /// <param name="count">The number of times to repeat [source]</param>
    /// <returns>A System.String consisting of [source] repeated [count] times.</returns>
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
    /// Replaces all occurrences of the specified strings in [source] with specified strings from the given System.Collections.Generic.IDictionary`2.
    /// </summary>
    /// <param name="source">The string to modify.</param>
    /// <param name="replacements">The given dictionary. Dictionary Keys found in [source] will be replaced by the corresponding Values.</param>
    /// <returns>A System.String equal to [source] where all occurrences of Keys in [replacements] are replaced with the corresponding Values.</returns>
    /// <example>
    /// <code>
    /// string test = "Hello {FirstName} {LastName}!";
    /// var replacements = new Dictionary&lt;string, string&gt;
    /// {
    ///     { "{FirstName}", "John" },
    ///     { "{LastName}", "Smith" }
    /// };
    /// string result = test.Replace(replacements);
    /// </code>
    /// </example>
    public static string Replace(this string source, IDictionary<string, string> replacements)
    {
        var regex = new Regex(replacements.Keys.Join(a => string.Concat("(", Regex.Escape(a), ")"), "|"));
        return regex.Replace(source, m => replacements[m.Value]);
    }

    /// <summary>
    /// Retrieves a substring from the given string. The substring starts at [length] characters before the end of [source].
    /// </summary>
    /// <param name="source">The string from which to extract a substring.</param>
    /// <param name="length">The number of characters in the substring.</param>
    /// <returns>A string that is equivalent to the substring of length that starts at [length] characters before the end of [source].</returns>
    public static string Right(this string source, int length) => source == null ? null : source.Length <= length ? source : source.Substring(source.Length - length, length);

    /// <summary>
    /// Retrieves a substring from the given string. The substring starts at first occurrence of [value].
    /// </summary>
    /// <param name="source">The string from which to extract a substring.</param>
    /// <param name="value">The Unicode character to seek the first occurrence of.</param>
    /// <returns>A substring of [source] that starts at the first occurrence of [value].</returns>
    public static string RightOf(this string source, char value)
    {
        if (source == null)
        {
            return null;
        }

        int index = source.IndexOf(value);
        return index != -1 ? source[(index + 1)..] : source;
    }

    /// <summary>
    /// Retrieves a substring from the given string. The substring starts at the [n]th occurrence of [value].
    /// </summary>
    /// <param name="source">The string from which to extract a substring.</param>
    /// <param name="value">The Unicode character to seek the [n]th occurrence of.</param>
    /// <param name="n">A System.Int32 indicating which occurence of [value] to seek.</param>
    /// <returns>A substring of [source] that starts at the [n]th occurrence of [value].</returns>
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

        return index != -1 ? source[(index + 1)..] : source;
    }

    /// <summary>
    /// Retrieves a substring from the given string. The substring starts at the first occurrence of [value].
    /// </summary>
    /// <param name="source">The string from which to extract a substring.</param>
    /// <param name="value">The substring to seek the first occurrence of.</param>
    /// <returns>A substring of [source] that starts at the first occurrence of [value].</returns>
    public static string RightOf(this string source, string value)
    {
        if (source == null)
        {
            return null;
        }

        int index = source.IndexOf(value);
        return index != -1 ? source[(index + 1)..] : source;
    }

    /// <summary>
    /// Retrieves a substring from the given string. The substring starts at the last occurrence of [value].
    /// </summary>
    /// <param name="source">The string from which to extract a substring.</param>
    /// <param name="value">The Unicode character to seek the last occurrence of.</param>
    /// <returns>A substring of [source] that starts at the last occurrence of [value].</returns>
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
            ret = source[(index + 1)..];
        }
        return ret;
    }

    /// <summary>
    ///  Retrieves a substring from the given string. The substring starts at the last occurrence of [value].
    /// </summary>
    /// <param name="source">The string from which to extract a substring.</param>
    /// <param name="value">The substring to seek the last occurrence of.</param>
    /// <returns>A substring of [source] that starts at the last occurrence of [value].</returns>
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
            ret = source[(index + 1)..];
        }
        return ret;
    }

    /// <summary>
    /// Removes all leading and trailing occurrences of a set of characters specified
    /// in an array from the given string. No exception is thrown if [source] is null or empty.
    /// </summary>
    /// <param name="source">The string to trim.</param>
    /// <param name="trimChars">An array of Unicode characters to remove, or null.</param>
    /// <returns>
    /// The string that remains after all occurrences of the characters in the trimChars
    /// parameter are removed from the start and end of [source]. If trimChars
    /// is null or an empty array, white-space characters are removed instead. If no
    /// characters can be trimmed from [source], the method returns [source] unchanged.
    /// </returns>
    public static string SafeTrim(this string source, params char[] trimChars) => string.IsNullOrEmpty(source) ? source : source.Trim(trimChars);

    /// <summary>
    /// Modifies the given string to split Pascal-cased text into separate words.
    /// For example: "MyPascalText" will become "My Pascal Text".
    /// </summary>
    /// <param name="source">The string to modify.</param>
    /// <returns>
    /// A new string that contains the same content as [source], but where all Pascal-cased text
    /// has been split into separate words.
    /// </returns>
    /// <example><code>string name = MyEnum.SomePascalCasedMember.ToString().SplitPascal();</code></example>
    public static string SplitPascal(this string source)
    {
        var sb = new StringBuilder();
        for (int i = 0; i < source.Length; i++)
        {
            char a = source[i];
            if (char.IsUpper(a) && i + 1 < source.Length && !char.IsUpper(source[i + 1]))
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
    /// Determines whether the beginning of the given string matches any of the specified strings.
    /// </summary>
    /// <param name="source">The string to examine.</param>
    /// <param name="values">The strings to compare to the substring at the beginning of [source].</param>
    /// <returns>true if any of the specified strings match the beginning of the given string; otherwise, false.</returns>
    public static bool StartsWithAny(this string source, params Span<string> values)
    {
        foreach (string value in values)
        {
            if (source.StartsWith(value))
            { return true; }
        }
        return false;
    }

    /// <summary>
    /// Converts the specified string to camel case.
    /// </summary>
    /// <param name="source">The string to convert to camel case.</param>
    /// <returns>The specified string converted to camel case.</returns>
    [Obsolete("Use Humanizer instead. https://github.com/Humanizr/Humanizer")]
    public static string ToCamelCase(this string source)
    {
        string pascal = source.ToPascalCase();
        return string.Concat(pascal[0].ToString().ToLower(), pascal[1..]);
    }

    /// <summary>
    /// Creates a new file, writes the given string to the file, and then closes
    /// the file. If the target file already exists, it is overwritten.
    /// </summary>
    /// <param name="source">The string to write to file.</param>
    /// <param name="filePath">The file to write to.</param>
    /// <returns>true if successful; otherwise false.</returns>
    public static bool ToFile(this string source, string filePath)
    {
        try
        {
            string directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            using var sw = new StreamWriter(fs);
            sw.Write(source);
            sw.Flush();
            return true;
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
    /// Splits the given string by newline characters and returns the result as a collection of strings.
    /// </summary>
    /// <param name="source">The string to split into lines.</param>
    /// <returns>A collection of strings.</returns>
    public static IEnumerable<string> ToLines(this string source) => !string.IsNullOrEmpty(source)
        ? source.Split(["\r\n", Environment.NewLine, "\n"], StringSplitOptions.None)
        : [];

    /// <summary>
    /// Converts the specified string to pascal case.
    /// </summary>
    /// <param name="source">The string to convert to pascal case.</param>
    /// <returns>The specified string converted to pascal case.</returns>
    [Obsolete("Use Humanizer instead. https://github.com/Humanizr/Humanizer")]
    public static string ToPascalCase(this string source) => source.ToTitleCase().Replace(" ", string.Empty);

    /// <summary>
    ///  Initializes a new non-resizable instance of the System.IO.MemoryStream class
    ///  based on the encoded sequence of bytes of the given string. A parameter
    ///  specifies the character encoding to use.
    /// </summary>
    /// <param name="source">The string to create the new System.IO.MemoryStream from.</param>
    /// <param name="encoding">The character encoding to use.</param>
    /// <returns>A new non-resizable instance of System.IO.MemoryStream initialized with the encoded sequence of bytes from [source].</returns>
    public static MemoryStream ToStream(this string source, Encoding encoding = null)
    {
        encoding ??= Encoding.UTF8;

        byte[] bytes = encoding.GetBytes(source);
        return new MemoryStream(bytes);
    }

    /// <summary>
    /// Converts the specified string to title case using the current culture (except
    /// for words that are entirely in uppercase, which are considered to be acronyms).
    /// </summary>
    /// <param name="source">The string to convert to title case.</param>
    /// <returns>The specified string converted to title case.</returns>
    [Obsolete("Use Humanizer instead. https://github.com/Humanizr/Humanizer")]
    public static string ToTitleCase(this string source) => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(source);

    /// <summary>
    /// Converts the specified string to title case using the specified culture (except
    /// for words that are entirely in uppercase, which are considered to be acronyms).
    /// </summary>
    /// <param name="source">The string to convert to title case.</param>
    /// <param name="cultureInfo">The System.Globalization.CultureInfo to use for converting to title case.</param>
    /// <returns>The specified string converted to title case.</returns>
    [Obsolete("Use Humanizer instead. https://github.com/Humanizr/Humanizer")]
    public static string ToTitleCase(this string source, CultureInfo cultureInfo) => cultureInfo.TextInfo.ToTitleCase(source);

    /// <summary>
    /// Converts a string that has been encoded for transmission in a URL into a decoded string.
    /// </summary>
    /// <param name="source">The string to decode.</param>
    /// <returns>A decoded string.</returns>
    public static string UrlDecode([StringSyntax(StringSyntaxAttribute.Uri)] this string source) => HttpUtility.UrlDecode(source);

    /// <summary>
    /// Encodes a URL string.
    /// </summary>
    /// <param name="source">The text to encode.</param>
    /// <returns>An encoded string.</returns>
    public static string UrlEncode([StringSyntax(StringSyntaxAttribute.Uri)] this string source) => HttpUtility.UrlEncode(source);

    /// <summary>
    /// Gets a value indicating the number of words in the given string.
    /// </summary>
    /// <param name="source">The string to get a word count from.</param>
    /// <returns>A System.Int32 specifying the number of words in the given string.</returns>
    public static int WordCount(this string source) => source.Split(' ').Count();

    /// <summary>
    /// Gets a value indicating the number of times that the specified word appears in the given string.
    /// </summary>
    /// <param name="source">The string to search for the specified word.</param>
    /// <param name="word">The word to search the string for.</param>
    /// <returns>A System.Int32 indicating the number of times the specified word appears in the given string.</returns>
    public static int WordCount(this string source, string word) => source.Split(' ').Where(x => x == word).Count();

    /// <summary>
    /// Deserializes the XML contained within the given string to an object of the specified type.
    /// </summary>
    /// <typeparam name="T">The type of object to deserialize the XML to.</typeparam>
    /// <param name="source">The string to deserialize.</param>
    /// <returns>The deserialized object from the XML data in [source].</returns>
    public static T XmlDeserialize<T>([StringSyntax(StringSyntaxAttribute.Xml)] this string source)
    {
        if (string.IsNullOrWhiteSpace(source)) return default;

        var serializer = new XmlSerializer(typeof(T));
        using var reader = new StringReader(source);
        return (T)serializer.Deserialize(reader);
    }

    /// <summary>
    /// Deserializes the XML contained within the given string to an object of the specified type.
    /// </summary>
    /// <param name="source">The string to deserialize.</param>
    /// <param name="type">The type of object to deserialize the XML to.</param>
    /// <returns>The deserialized object from the XML data in [source].</returns>
    public static object XmlDeserialize([StringSyntax(StringSyntaxAttribute.Xml)] this string source, Type type)
    {
        if (string.IsNullOrWhiteSpace(source)) return null;

        var serializer = new XmlSerializer(type);
        using var reader = new StringReader(source);
        return serializer.Deserialize(reader);
    }
}