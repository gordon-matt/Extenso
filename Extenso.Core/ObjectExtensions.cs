using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Extenso.IO;
using Newtonsoft.Json;

namespace Extenso
{
    /// <summary>
    /// Provides a set of static methods for querying and manipulating objects
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Serializes the given object to binary format, converts it to a Base64 encoded string and returns the result.
        /// </summary>
        /// <typeparam name="T">The type of source.</typeparam>
        /// <param name="source">The object to serialize.</param>
        /// <param name="prependLength">
        /// Specifies whether to prepend the length of the byte array to the Base64 string.
        /// If true, the length and base64 encoded string will be separated by a colon.
        /// </param>
        /// <returns>A Base64 encoded string of the serialized object.</returns>
        public static string Base64Serialize<T>(this T source, bool prependLength = false)
        {
            using (var memoryStream = new MemoryStream())
            {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(memoryStream, source);
                var bytes = memoryStream.GetBuffer();
                return bytes.Base64Serialize(prependLength);
            }
        }

        /// <summary>
        /// Serializes the given object to a byte array.
        /// </summary>
        /// <typeparam name="T">The type of source.</typeparam>
        /// <param name="source">The object to serialize.</param>
        /// <returns>A byte array of the serialized object.</returns>
        public static byte[] BinarySerialize<T>(this T source)
        {
            using (var memoryStream = new MemoryStream())
            {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(memoryStream, source);
                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// Serializes the given object to a byte array and writes the data to the specified file.
        /// </summary>
        /// <typeparam name="T">The type of source.</typeparam>
        /// <param name="source">The object to serialize.</param>
        /// <param name="fileName">The full path of the file to save the serialized data to.</param>
        public static void BinarySerialize<T>(this T source, string fileName)
        {
            using (var stream = File.Open(fileName, FileMode.Create))
            {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(stream, source);
                stream.Close();
            }
        }

        /// <summary>
        /// Computes the hash value for the given byte array using the specified algorithm.
        /// </summary>
        /// <typeparam name="T">The type of algorithm to use for computing the hash.</typeparam>
        /// <param name="source">The object to compute the hash for.</param>
        /// <param name="hashAlgorithm">The algorithm to use for computing the hash.</param>
        /// <returns>The computed hash code.</returns>
        /// <example><code>byte[] hash = myObject.ComputeHash(new MD5CryptoServiceProvider());</code></example>
        public static byte[] ComputeHash<T>(this object source, T hashAlgorithm) where T : HashAlgorithm, new()
        {
            var serializer = new DataContractSerializer(source.GetType());
            using (var memoryStream = new MemoryStream())
            {
                serializer.WriteObject(memoryStream, source);
                hashAlgorithm.ComputeHash(memoryStream.ToArray());
                return hashAlgorithm.Hash;
            }
        }

        public static byte[] ComputeMD5Hash(this object source)
        {
            return ComputeHash(source, new MD5CryptoServiceProvider());
        }

        public static byte[] ComputeSHA1Hash(this object source)
        {
            return ComputeHash(source, new SHA1CryptoServiceProvider());
        }

        /// <summary>
        /// Returns an object of the specified type and whose value is equivalent to the specified object.
        /// </summary>
        /// <typeparam name="T">The type of object to return.</typeparam>
        /// <param name="source">An object that implements the System.IConvertible interface.</param>
        /// <returns>
        /// An object whose type is conversionType and whose value is equivalent to source.
        /// -or- A null reference (Nothing in Visual Basic), if source is null and conversionType
        /// is not a value type.
        /// </returns>
        public static T ConvertTo<T>(this object source)
        {
            return (T)ConvertTo(source, typeof(T));
        }

        /// <summary>
        /// Returns an object of the specified type and whose value is equivalent to the specified object.
        /// </summary>
        /// <param name="source">An object that implements the System.IConvertible interface.</param>
        /// <param name="conversionType">The type of object to return.</param>
        /// <returns>
        /// An object whose type is conversionType and whose value is equivalent to source.
        /// -or- A null reference (Nothing in Visual Basic), if source is null and conversionType
        /// is not a value type.
        /// </returns>
        public static object ConvertTo(this object source, Type conversionType)
        {
            if (conversionType == typeof(Guid))
            {
                return new Guid(source.ToString());
            }

            return Convert.ChangeType(source, conversionType);
        }

        /// <summary>
        /// Creates a deep clone of the given object.
        /// </summary>
        /// <typeparam name="T">The type of source.</typeparam>
        /// <param name="source">The object to clone.</param>
        /// <returns>A deep clone of source.</returns>
        public static T DeepClone<T>(this T source)
        {
            using (var memoryStream = new MemoryStream())
            {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(memoryStream, source);
                memoryStream.Seek(0, SeekOrigin.Begin);
                return (T)binaryFormatter.Deserialize(memoryStream);
            }
        }

        /// <summary>
        /// Uses the default equality comparer for the type specified by the generic argument to determine whether two objects are equal.
        /// </summary>
        /// <typeparam name="T">The type of source.</typeparam>
        /// <param name="source">The first object to compare.</param>
        /// <param name="other">The second object to compare.</param>
        /// <returns>true if the specified objects are equal; otherwise, false.</returns>
        public static bool GenericEquals<T>(this T source, T other)
        {
            return EqualityComparer<T>.Default.Equals(source, other);
        }

        /// <summary>
        /// Determines whether a sequence contains the given element by using the default equality comparer.
        /// </summary>
        /// <typeparam name="T">The type of source.</typeparam>
        /// <param name="source">The value to locate in the sequence.</param>
        /// <param name="values">The sequence in which to locate source.</param>
        /// <returns>true if values contains source; otherwise, false.</returns>
        /// <example>
        /// <code>
        /// var myValues = new[] { "val1", "val2", "val3" };
        /// if (myString.In(myValues))
        /// </code>
        /// </example>
        public static bool In<T>(this T source, IEnumerable<T> values)
        {
            return values.Contains(source);

            //foreach (T item in values)
            //{
            //    if (item.Equals(source))
            //    { return true; }
            //}
            //return false;
        }

        /// <summary>
        /// Determines whether an array of objects contains the given element by using the default equality comparer.
        /// </summary>
        /// <typeparam name="T">The type of source.</typeparam>
        /// <param name="source">The value to locate in the array of objects.</param>
        /// <param name="values">The array of objects in which to locate source.</param>
        /// <returns>true if values contains source; otherwise, false.</returns>
        /// <example><code>if (myString.In("val1", "val2", "val3"))</code></example>
        public static bool In<T>(this T source, params T[] values)
        {
            return values.Contains(source);

            //foreach (T item in values)
            //{
            //    if (item.Equals(source))
            //    { return true; }
            //}
            //return false;
        }

        /// <summary>
        /// Determines whether the value of source is equal to the default value of its type.
        /// </summary>
        /// <typeparam name="T">The type of source.</typeparam>
        /// <param name="source">The value to compare with the default for its type.</param>
        /// <returns>true if source is equal to the default value of its type; otherwise false.</returns>
        public static bool IsDefault<T>(this T source)
        {
            return source.GenericEquals(default);
        }

        /// <summary>
        /// Serializes the specified object to a JSON string. A parameter specifies the serializer settings.
        /// </summary>
        /// <typeparam name="T">The type of source.</typeparam>
        /// <param name="source">The object to serialize.</param>
        /// <param name="settings">
        /// The Newtonsoft.Json.JsonSerializerSettings used to serialize the object. If this
        /// is null, default serialization settings will be used.
        /// </param>
        /// <returns>A JSON string representation of the object.</returns>
        public static string JsonSerialize<T>(this T source, JsonSerializerSettings settings = null)
        {
            if (source == null)
            {
                return null;
            }

            return JsonConvert.SerializeObject(source, settings);
        }

        /// <summary>
        /// Creates a System.Dynamic.ExpandoObject from the given System.Object.
        /// </summary>
        /// <param name="source">The object to convert to an ExpandoObject.</param>
        /// <returns>An ExpandoObject containing elements whose keys and values represent the public properties of source and the values thereof.</returns>
        public static ExpandoObject ToExpando(this object source)
        {
            IDictionary<string, object> expando = new ExpandoObject();

            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(source.GetType()))
            {
                expando.Add(property.Name, property.GetValue(source));
            }

            return expando as ExpandoObject;
        }

        /// <summary>
        /// Serializes the specified object and writes the XML document to a file.
        /// </summary>
        /// <typeparam name="T">The type of source.</typeparam>
        /// <param name="source">The object to serialize.</param>
        /// <param name="fileName">The full path of the file to be written to.</param>
        /// <param name="omitXmlDeclaration">Indicates whether to omit an XML declaration.</param>
        /// <param name="removeNamespaces">
        /// Indicates whether to remove the XML namespaces during serialization. If any of the properties on the object
        /// are decorated with an XmlIncludeAttribute, then set this to false.
        /// </param>
        /// <param name="xmlns">
        /// XML namespaces and prefixes that the serializer should use to generate qualified names.
        /// If not null, removeNamespaces is ignored.
        /// </param>
        /// <param name="encoding">Specifies the character encoding to use.</param>
        /// <returns>A string containing the XML.</returns>
        /// <returns>true if successful; otherwise false.</returns>
        public static bool XmlSerialize<T>(
            this T source,
            string fileName,
            bool omitXmlDeclaration = true,
            bool removeNamespaces = true,
            XmlSerializerNamespaces xmlns = null,
            Encoding encoding = null)
        {
            string xml = source.XmlSerialize(omitXmlDeclaration, removeNamespaces, xmlns, encoding);
            return xml.ToFile(fileName);
        }

        /// <summary>
        /// Serializes the specified object and writes the XML document to a string.
        /// </summary>
        /// <typeparam name="T">The type of source.</typeparam>
        /// <param name="source">The object to serialize.</param>
        /// <param name="omitXmlDeclaration">Indicates whether to omit an XML declaration.</param>
        /// <param name="removeNamespaces">
        /// Indicates whether to remove the XML namespaces during serialization. If any of the properties on the object
        /// are decorated with an XmlIncludeAttribute, then set this to false.
        /// </param>
        /// <param name="xmlns">
        /// XML namespaces and prefixes that the serializer should use to generate qualified names.
        /// If not null, removeNamespaces is ignored.
        /// </param>
        /// <param name="encoding">Specifies the character encoding to use.</param>
        /// <returns>A string containing the XML.</returns>
        public static string XmlSerialize<T>(
            this T source,
            bool omitXmlDeclaration = true,
            bool removeNamespaces = true,
            XmlSerializerNamespaces xmlns = null,
            Encoding encoding = null)
        {
            object locker = new object();

            var xmlSerializer = new XmlSerializer(source.GetType());

            var settings = new XmlWriterSettings
            {
                Indent = true,
                OmitXmlDeclaration = omitXmlDeclaration
            };

            lock (locker)
            {
                var stringBuilder = new StringBuilder();
                using (var stringWriter = new CustomEncodingStringWriter(encoding, stringBuilder))
                using (var xmlWriter = XmlWriter.Create(stringWriter, settings))
                {
                    if (xmlns != null)
                    {
                        xmlSerializer.Serialize(xmlWriter, source, xmlns);
                    }
                    else
                    {
                        if (removeNamespaces)
                        {
                            xmlns = new XmlSerializerNamespaces();
                            xmlns.Add(string.Empty, string.Empty);

                            xmlSerializer.Serialize(xmlWriter, source, xmlns);
                        }
                        else { xmlSerializer.Serialize(xmlWriter, source); }
                    }

                    return stringBuilder.ToString();
                }
            }
        }
    }
}