using System.IO.Compression;
using System.Text;

namespace Extenso.IO;

/// <summary>
/// Provides a set of static methods for querying and manipulating a System.IO.Stream or objects that derived thereof.
/// </summary>
public static class StreamExtensions
{
    extension(Stream stream)
    {
        /// <summary>
        /// Deserializes the binary data contained in the given stream to an object of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize the binary data to.</typeparam>
        /// <returns>The deserialized object from the stream.</returns>
        public T BinaryDeserialize<T>()
        {
            byte[] bytes = stream.ToArray();
            return Encoding.UTF8.GetString(bytes).JsonDeserialize<T>();
        }

        /// <summary>
        /// Serializes the given object to the given stream
        /// </summary>
        /// <typeparam name="T">The type of obj</typeparam>
        /// <param name="obj">The object to serialize.</param>
        public void BinarySerialize<T>(T obj)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(obj.JsonSerialize());
            stream.Write(bytes);
            stream.Position = 0;
        }

        /// <summary>
        /// Compresses the given stream using the Brotli algorithm and returns a System.IO.MemoryStream of compressed data.
        /// </summary>
        /// <returns>A System.IO.MemoryStream of compressed data.</returns>
        public MemoryStream BrotliCompress(CompressionLevel compressionLevel = CompressionLevel.Optimal) =>
            ProcessCompression(stream, dest => new BrotliStream(dest, compressionLevel, leaveOpen: true));

        /// <summary>
        /// Compresses the given stream using the Brotli algorithm and returns a System.IO.MemoryStream of compressed data.
        /// </summary>
        /// <returns>A System.IO.MemoryStream of compressed data.</returns>
        public Task<MemoryStream> BrotliCompressAsync(CompressionLevel compressionLevel = CompressionLevel.Optimal, CancellationToken cancellationToken = default) =>
            ProcessCompressionAsync(stream, dest => new BrotliStream(dest, compressionLevel, leaveOpen: true), cancellationToken);

        /// <summary>
        /// Decompresses the given stream using the Brotli algorithm and returns a System.IO.MemoryStream of decompressed data.
        /// </summary>
        /// <returns>A System.IO.MemoryStream of decompressed data.</returns>
        public MemoryStream BrotliDecompress() =>
            ProcessDecompression(stream, src => new BrotliStream(src, CompressionMode.Decompress, leaveOpen: true));

        /// <summary>
        /// Decompresses the given stream using the Brotli algorithm and returns a System.IO.MemoryStream of decompressed data.
        /// </summary>
        /// <returns>A System.IO.MemoryStream of decompressed data.</returns>
        public Task<MemoryStream> BrotliDecompressAsync(CancellationToken cancellationToken = default) =>
            ProcessDecompressionAsync(stream, src => new BrotliStream(src, CompressionMode.Decompress, leaveOpen: true), cancellationToken);

        /// <summary>
        /// Compresses the given stream using the Deflate algorithm and returns a System.IO.MemoryStream of compressed data.
        /// </summary>
        /// <returns>A System.IO.MemoryStream of compressed data.</returns>
        public MemoryStream DeflateCompress(CompressionLevel compressionLevel = CompressionLevel.Optimal) =>
            ProcessCompression(stream, dest => new DeflateStream(dest, compressionLevel, leaveOpen: true));

        /// <summary>
        /// Compresses the given stream using the Deflate algorithm and returns a System.IO.MemoryStream of compressed data.
        /// </summary>
        /// <returns>A System.IO.MemoryStream of compressed data.</returns>
        public Task<MemoryStream> DeflateCompressAsync(CompressionLevel compressionLevel = CompressionLevel.Optimal, CancellationToken cancellationToken = default) =>
            ProcessCompressionAsync(stream, dest => new DeflateStream(dest, compressionLevel, leaveOpen: true), cancellationToken);

        /// <summary>
        /// Decompresses the given stream using the Deflate algorithm and returns a System.IO.MemoryStream of decompressed data.
        /// </summary>
        /// <returns>A System.IO.MemoryStream of decompressed data.</returns>
        public MemoryStream DeflateDecompress() =>
            ProcessDecompression(stream, src => new DeflateStream(src, CompressionMode.Decompress, leaveOpen: true));

        /// <summary>
        /// Decompresses the given stream using the Deflate algorithm and returns a System.IO.MemoryStream of decompressed data.
        /// </summary>
        /// <param name="stream">The stream to decompress.</param>
        /// <returns>A System.IO.MemoryStream of decompressed data.</returns>
        public Task<MemoryStream> DeflateDecompressAsync(CancellationToken cancellationToken = default) =>
            ProcessDecompressionAsync(stream, src => new DeflateStream(src, CompressionMode.Decompress, leaveOpen: true), cancellationToken);

        /// <summary>
        /// Compresses the given stream using the GZip algorithm and returns a System.IO.MemoryStream of compressed data.
        /// </summary>
        /// <returns>A System.IO.MemoryStream of compressed data.</returns>
        public MemoryStream GZipCompress(CompressionLevel compressionLevel = CompressionLevel.Optimal) =>
            ProcessCompression(stream, dest => new GZipStream(dest, compressionLevel, leaveOpen: true));

        /// <summary>
        /// Compresses the given stream using the GZip algorithm and returns a System.IO.MemoryStream of compressed data.
        /// </summary>
        /// <returns>A System.IO.MemoryStream of compressed data.</returns>
        public Task<MemoryStream> GZipCompressAsync(CompressionLevel compressionLevel = CompressionLevel.Optimal, CancellationToken cancellationToken = default) =>
            ProcessCompressionAsync(stream, dest => new GZipStream(dest, compressionLevel, leaveOpen: true));

        /// <summary>
        /// Decompresses the given stream using the GZip algorithm and returns a System.IO.MemoryStream of decompressed data.
        /// </summary>
        /// <returns>A System.IO.MemoryStream of decompressed data.</returns>
        public MemoryStream GZipDecompress() =>
            ProcessDecompression(stream, src => new GZipStream(src, CompressionMode.Decompress, leaveOpen: true));

        /// <summary>
        /// Decompresses the given stream using the GZip algorithm and returns a System.IO.MemoryStream of decompressed data.
        /// </summary>
        /// <returns>A System.IO.MemoryStream of decompressed data.</returns>
        public Task<MemoryStream> GZipDecompressAsync(CancellationToken cancellationToken = default) =>
            ProcessDecompressionAsync(stream, src => new GZipStream(src, CompressionMode.Decompress, leaveOpen: true));

        /// <summary>
        /// Writes the stream content to a byte array.
        /// </summary>
        /// <returns>A new byte array.</returns>
        public byte[] ToArray()
        {
            using var memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }
    }

    private static MemoryStream ProcessCompression(
        Stream source,
        Func<Stream, Stream> streamFactory)
    {
        if (source.Position != 0 && source.CanSeek)
            source.Position = 0;

        var destination = new MemoryStream();
        using (var processingStream = streamFactory(destination))
        {
            source.CopyTo(processingStream);
        }

        destination.Position = 0;
        return destination;
    }

    private static async Task<MemoryStream> ProcessCompressionAsync(
        Stream source,
        Func<Stream, Stream> streamFactory,
        CancellationToken cancellationToken = default)
    {
        if (source.Position != 0 && source.CanSeek)
            source.Position = 0;

        var destination = new MemoryStream();
        await using (var processingStream = streamFactory(destination))
        {
            await source.CopyToAsync(processingStream, cancellationToken);
        }
        destination.Position = 0;
        return destination;
    }

    private static MemoryStream ProcessDecompression(
        Stream source,
        Func<Stream, Stream> decompressionStreamFactory)
    {
        if (source.Position != 0 && source.CanSeek)
            source.Position = 0;

        using var decompressionStream = decompressionStreamFactory(source);
        var destination = new MemoryStream();
        decompressionStream.CopyTo(destination);
        destination.Position = 0;
        return destination;
    }

    private static async Task<MemoryStream> ProcessDecompressionAsync(
        Stream source,
        Func<Stream, Stream> decompressionStreamFactory,
        CancellationToken cancellationToken = default)
    {
        if (source.Position != 0 && source.CanSeek)
            source.Position = 0;

        await using var decompressionStream = decompressionStreamFactory(source);
        var destination = new MemoryStream();
        await decompressionStream.CopyToAsync(destination, cancellationToken);
        destination.Position = 0;
        return destination;
    }
}