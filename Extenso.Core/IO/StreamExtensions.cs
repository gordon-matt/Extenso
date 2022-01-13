using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;

namespace Extenso.IO
{
    /// <summary>
    /// Provides a set of static methods for querying and manipulating a System.IO.Stream or objects that derived thereof.
    /// </summary>
    public static class StreamExtensions
    {
        /// <summary>
        /// Deserializes the binary data contained in the given stream to an object of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize the binary data to.</typeparam>
        /// <param name="stream">The stream to deserialize.</param>
        /// <returns>The deserialized object from the stream.</returns>
        public static T BinaryDeserialize<T>(this Stream stream)
        {
            return (T)new BinaryFormatter().Deserialize(stream);
        }

        /// <summary>
        /// Serializes the given object to the given stream
        /// </summary>
        /// <typeparam name="T">The type of obj</typeparam>
        /// <param name="stream">The stream to write to.</param>
        /// <param name="obj">The object to serialize.</param>
        public static void BinarySerialize<T>(this Stream stream, T obj)
        {
            new BinaryFormatter().Serialize(stream, obj);
        }

        /// <summary>
        /// Compresses the given stream using the Deflate algorithm and returns a System.IO.MemoryStream of compressed data.
        /// </summary>
        /// <param name="stream">The stream to compress.</param>
        /// <returns>A System.IO.MemoryStream of compressed data.</returns>
        public static MemoryStream DeflateCompress(this Stream stream)
        {
            var memoryStream = new MemoryStream();
            var deflateStream = new DeflateStream(memoryStream, CompressionMode.Compress);
            stream.CopyTo(deflateStream);
            return memoryStream;
        }

        /// <summary>
        /// Decompresses the given stream using the Deflate algorithm and returns a System.IO.MemoryStream of decompressed data.
        /// </summary>
        /// <param name="stream">The stream to decompress.</param>
        /// <returns>A System.IO.MemoryStream of decompressed data.</returns>
        public static MemoryStream DeflateDecompress(this Stream stream)
        {
            var memoryStream = new MemoryStream();
            var deflateStream = new DeflateStream(stream, CompressionMode.Decompress);
            deflateStream.CopyTo(memoryStream);
            return memoryStream;
        }

        /// <summary>
        /// Compresses the given stream using the GZip algorithm and returns a System.IO.MemoryStream of compressed data.
        /// </summary>
        /// <param name="stream">The stream to compress.</param>
        /// <returns>A System.IO.MemoryStream of compressed data.</returns>
        public static MemoryStream GZipCompress(this Stream stream)
        {
            var memoryStream = new MemoryStream();
            var gZipStream = new GZipStream(memoryStream, CompressionMode.Compress);
            stream.CopyTo(gZipStream);
            return memoryStream;
        }

        /// <summary>
        /// Decompresses the given stream using the GZip algorithm and returns a System.IO.MemoryStream of decompressed data.
        /// </summary>
        /// <param name="stream">The stream to decompress.</param>
        /// <returns>A System.IO.MemoryStream of decompressed data.</returns>
        public static MemoryStream GZipDecompress(this Stream stream)
        {
            var memoryStream = new MemoryStream();
            var gZipStream = new GZipStream(stream, CompressionMode.Decompress);
            gZipStream.CopyTo(memoryStream);
            return memoryStream;
        }
    }
}