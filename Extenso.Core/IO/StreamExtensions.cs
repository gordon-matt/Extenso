using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;

namespace Extenso.IO
{
    public static class StreamExtensions
    {
        public static T BinaryDeserialize<T>(this Stream stream)
        {
            return (T)new BinaryFormatter().Deserialize(stream);
        }

        public static void BinarySerialize<T>(this Stream stream, T item)
        {
            new BinaryFormatter().Serialize(stream, item);
        }

        public static MemoryStream DeflateCompress(this Stream stream)
        {
            var memoryStream = new MemoryStream();
            var deflateStream = new DeflateStream(memoryStream, CompressionMode.Compress);
            stream.CopyTo(deflateStream);
            return memoryStream;
        }

        public static MemoryStream DeflateDecompress(this Stream stream)
        {
            var memoryStream = new MemoryStream();
            var deflateStream = new DeflateStream(stream, CompressionMode.Decompress);
            deflateStream.CopyTo(memoryStream);
            return memoryStream;
        }

        public static MemoryStream GZipCompress(this Stream stream)
        {
            var memoryStream = new MemoryStream();
            var gZipStream = new GZipStream(memoryStream, CompressionMode.Compress);
            stream.CopyTo(gZipStream);
            return memoryStream;
        }

        public static MemoryStream GZipDecompress(this Stream stream)
        {
            var memoryStream = new MemoryStream();
            var gZipStream = new GZipStream(stream, CompressionMode.Decompress);
            gZipStream.CopyTo(memoryStream);
            return memoryStream;
        }
    }
}