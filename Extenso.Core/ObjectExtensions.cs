using System;
using System.Collections.Generic;
using System.IO;
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
    public static class ObjectExtensions
    {
        /// <summary>
        /// Serializes the specified System.Object and returns the data.
        /// </summary>
        /// <typeparam name="T">This item's type</typeparam>
        /// <param name="item">This item</param>
        /// <returns>Serialized data of specified System.Object as a Base64 encoded String</returns>
        public static string Base64Serialize<T>(this T item)
        {
            using (var memoryStream = new MemoryStream())
            {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(memoryStream, item);
                var bytes = memoryStream.GetBuffer();
                return string.Concat(bytes.Length, ":", Convert.ToBase64String(bytes, 0, bytes.Length, Base64FormattingOptions.None));
            }
        }

        /// <summary>
        /// Serializes the specified System.Object and returns the data.
        /// </summary>
        /// <typeparam name="T">This item's type</typeparam>
        /// <param name="item">This item</param>
        /// <returns>Serialized data of specified System.Object as System.Byte[]</returns>
        public static byte[] BinarySerialize<T>(this T item)
        {
            using (var memoryStream = new MemoryStream())
            {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(memoryStream, item);
                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// Serializes the specified System.Object and writes the data to the specified file.
        /// </summary>
        /// <typeparam name="T">This item's type</typeparam>
        /// <param name="item">This item</param>
        /// <param name="fileName">The name of the file to save the serialized data to.</param>
        public static void BinarySerialize<T>(this T item, string fileName)
        {
            using (var stream = File.Open(fileName, FileMode.Create))
            {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(stream, item);
                stream.Close();
            }
        }

        #region Compute Hash

        public static string ComputeHash<T>(this object instance, T cryptoServiceProvider) where T : HashAlgorithm, new()
        {
            var serializer = new DataContractSerializer(instance.GetType());
            using (var memoryStream = new MemoryStream())
            {
                serializer.WriteObject(memoryStream, instance);
                cryptoServiceProvider.ComputeHash(memoryStream.ToArray());
                return Convert.ToBase64String(cryptoServiceProvider.Hash);
            }
        }

        public static string ComputeMD5Hash(this object instance)
        {
            return instance.ComputeHash(new MD5CryptoServiceProvider());
        }

        public static string ComputeSHA1Hash(this object instance)
        {
            return instance.ComputeHash(new SHA1CryptoServiceProvider());
        }

        #endregion Compute Hash

        public static T ConvertTo<T>(this object source)
        {
            //return (T)Convert.ChangeType(source, typeof(T));
            return (T)ConvertTo(source, typeof(T));
        }

        public static object ConvertTo(this object source, Type type)
        {
            if (type == typeof(Guid))
            {
                return new Guid(source.ToString());
            }

            return Convert.ChangeType(source, type);
        }

        /// <summary>
        /// Creates a deep clone of the current System.Object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">The original object.</param>
        /// <returns>A clone of the original object</returns>
        public static T DeepClone<T>(this T item) where T : ISerializable
        {
            using (var memoryStream = new MemoryStream())
            {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(memoryStream, item);
                memoryStream.Seek(0, SeekOrigin.Begin);
                return (T)binaryFormatter.Deserialize(memoryStream);
            }
        }

        /// <summary>
        /// Determines whether this T is contained in the specified 'IEnumerable of T'
        /// </summary>
        /// <typeparam name="T">This System.Object's type</typeparam>
        /// <param name="t">This item</param>
        /// <param name="enumerable">The 'IEnumerable of T' to check</param>
        /// <returns>true if enumerable contains this item, otherwise false.</returns>
        public static bool In<T>(this T t, IEnumerable<T> enumerable)
        {
            foreach (T item in enumerable)
            {
                if (item.Equals(t))
                { return true; }
            }
            return false;
        }

        /// <summary>
        /// Determines whether this T is contained in the specified values
        /// </summary>
        /// <typeparam name="T">This System.Object's type</typeparam>
        /// <param name="t">This item</param>
        /// <param name="items">The values to compare</param>
        /// <returns>true if items contains this item, otherwise false.</returns>
        /// <example><code>if (myString.In("val1", "val2", "val3"))</code></example>
        public static bool In<T>(this T t, params T[] items)
        {
            foreach (T item in items)
            {
                if (item.Equals(t))
                { return true; }
            }
            return false;
        }

        public static bool IsDefault<T>(this T item)
        {
            return EqualityComparer<T>.Default.Equals(item, default(T));
        }

        public static bool GenericEquals<T>(this T item, T other)
        {
            return EqualityComparer<T>.Default.Equals(item, other);
        }

        public static string ToJson<T>(this T item, JsonSerializerSettings settings = null)
        {
            if (item == null)
            {
                return null;
            }

            if (settings == null)
            {
                return JsonConvert.SerializeObject(item);
            }

            return JsonConvert.SerializeObject(item, settings);
        }

        /// <summary>
        /// <para>Serializes the specified System.Object and writes the XML document</para>
        /// <para>to the specified file.</para>
        /// </summary>
        /// <typeparam name="T">This item's type</typeparam>
        /// <param name="item">This item</param>
        /// <param name="fileName">The file to which you want to write.</param>
        /// <param name="omitXmlDeclaration">False to keep the XML declaration. Otherwise, it will be removed.</param>
        /// <param name="removeNamespaces">
        ///     <para>Specify whether to remove xml namespaces.</para>para>
        ///     <para>If your object has any XmlInclude attributes, then set this to false</para>
        /// </param>
        /// <param name="xmlns">If not null, "removeNamespaces" is ignored and the provided namespaces are used.</param>
        /// <param name="encoding">Specify encoding, if required.</param>
        /// <returns>true if successful, otherwise false.</returns>
        public static bool XmlSerialize<T>(
            this T item,
            string fileName,
            bool omitXmlDeclaration = true,
            bool removeNamespaces = true,
            XmlSerializerNamespaces xmlns = null,
            Encoding encoding = null)
        {
            string xml = item.XmlSerialize(omitXmlDeclaration, removeNamespaces, xmlns, encoding);
            return xml.ToFile(fileName);
        }

        /// <summary>
        /// Serializes the specified System.Object and returns the serialized XML
        /// </summary>
        /// <typeparam name="T">This item's type</typeparam>
        /// <param name="item">This item</param>
        /// <param name="omitXmlDeclaration">False to keep the XML declaration. Otherwise, it will be removed.</param>
        /// <param name="removeNamespaces">
        ///     <para>Specify whether to remove xml namespaces.</para>para>
        ///     <para>If your object has any XmlInclude attributes, then set this to false</para>
        /// </param>
        /// <param name="xmlns">If not null, "removeNamespaces" is ignored and the provided namespaces are used.</param>
        /// <param name="encoding">Specify encoding, if required.</param>
        /// <returns>Serialized XML for specified System.Object</returns>
        public static string XmlSerialize<T>(
            this T item,
            bool omitXmlDeclaration = true,
            bool removeNamespaces = true,
            XmlSerializerNamespaces xmlns = null,
            Encoding encoding = null)
        {
            object locker = new object();

            var xmlSerializer = new XmlSerializer(item.GetType());

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
                        xmlSerializer.Serialize(xmlWriter, item, xmlns);
                    }
                    else
                    {
                        if (removeNamespaces)
                        {
                            xmlns = new XmlSerializerNamespaces();
                            xmlns.Add(string.Empty, string.Empty);

                            xmlSerializer.Serialize(xmlWriter, item, xmlns);
                        }
                        else { xmlSerializer.Serialize(xmlWriter, item); }
                    }

                    return stringBuilder.ToString();
                }
            }
        }
    }
}