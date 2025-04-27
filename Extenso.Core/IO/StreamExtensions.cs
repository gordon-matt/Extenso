using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Extenso.IO;

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
        byte[] bytes = stream.ToArray();
        return Encoding.UTF8.GetString(bytes).JsonDeserialize<T>();
    }

    /// <summary>
    /// Serializes the given object to the given stream
    /// </summary>
    /// <typeparam name="T">The type of obj</typeparam>
    /// <param name="stream">The stream to write to.</param>
    /// <param name="obj">The object to serialize.</param>
    public static void BinarySerialize<T>(this Stream stream, T obj)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(obj.JsonSerialize());
        stream.Write(bytes);
        stream.Position = 0;
    }

    /// <summary>
    /// Compresses the given stream using the Brotli algorithm and returns a System.IO.MemoryStream of compressed data.
    /// </summary>
    /// <param name="stream">The stream to compress.</param>
    /// <returns>A System.IO.MemoryStream of compressed data.</returns>
    public static MemoryStream BrotliCompress(this Stream stream) =>
        ProcessCompression(stream, dest => new BrotliStream(dest, CompressionMode.Compress, leaveOpen: true));

    /// <summary>
    /// Compresses the given stream using the Brotli algorithm and returns a System.IO.MemoryStream of compressed data.
    /// </summary>
    /// <param name="stream">The stream to compress.</param>
    /// <returns>A System.IO.MemoryStream of compressed data.</returns>
    public static Task<MemoryStream> BrotliCompressAsync(this Stream stream, CancellationToken cancellationToken = default) =>
        ProcessCompressionAsync(stream, dest => new BrotliStream(dest, CompressionMode.Compress, leaveOpen: true), cancellationToken);

    /// <summary>
    /// Decompresses the given stream using the Brotli algorithm and returns a System.IO.MemoryStream of decompressed data.
    /// </summary>
    /// <param name="stream">The stream to decompress.</param>
    /// <returns>A System.IO.MemoryStream of decompressed data.</returns>
    public static MemoryStream BrotliDecompress(this Stream stream) =>
        ProcessDecompression(stream, src => new BrotliStream(src, CompressionMode.Decompress, leaveOpen: true));

    /// <summary>
    /// Decompresses the given stream using the Brotli algorithm and returns a System.IO.MemoryStream of decompressed data.
    /// </summary>
    /// <param name="stream">The stream to decompress.</param>
    /// <returns>A System.IO.MemoryStream of decompressed data.</returns>
    public static Task<MemoryStream> BrotliDecompressAsync(this Stream stream, CancellationToken cancellationToken = default) =>
        ProcessDecompressionAsync(stream, src => new BrotliStream(src, CompressionMode.Decompress, leaveOpen: true), cancellationToken);

    /// <summary>
    /// Compresses the given stream using the Deflate algorithm and returns a System.IO.MemoryStream of compressed data.
    /// </summary>
    /// <param name="stream">The stream to compress.</param>
    /// <returns>A System.IO.MemoryStream of compressed data.</returns>
    public static MemoryStream DeflateCompress(this Stream stream) =>
        ProcessCompression(stream, dest => new DeflateStream(dest, CompressionMode.Compress, leaveOpen: true));

    /// <summary>
    /// Compresses the given stream using the Deflate algorithm and returns a System.IO.MemoryStream of compressed data.
    /// </summary>
    /// <param name="stream">The stream to compress.</param>
    /// <returns>A System.IO.MemoryStream of compressed data.</returns>
    public static Task<MemoryStream> DeflateCompressAsync(this Stream stream, CancellationToken cancellationToken = default) =>
        ProcessCompressionAsync(stream, dest => new DeflateStream(dest, CompressionMode.Compress, leaveOpen: true), cancellationToken);

    /// <summary>
    /// Decompresses the given stream using the Deflate algorithm and returns a System.IO.MemoryStream of decompressed data.
    /// </summary>
    /// <param name="stream">The stream to decompress.</param>
    /// <returns>A System.IO.MemoryStream of decompressed data.</returns>
    public static MemoryStream DeflateDecompress(this Stream stream) =>
        ProcessDecompression(stream, src => new DeflateStream(src, CompressionMode.Decompress, leaveOpen: true));

    /// <summary>
    /// Decompresses the given stream using the Deflate algorithm and returns a System.IO.MemoryStream of decompressed data.
    /// </summary>
    /// <param name="stream">The stream to decompress.</param>
    /// <returns>A System.IO.MemoryStream of decompressed data.</returns>
    public static Task<MemoryStream> DeflateDecompressAsync(this Stream stream, CancellationToken cancellationToken = default) =>
        ProcessDecompressionAsync(stream, src => new DeflateStream(src, CompressionMode.Decompress, leaveOpen: true), cancellationToken);

    /// <summary>
    /// Compresses the given stream using the GZip algorithm and returns a System.IO.MemoryStream of compressed data.
    /// </summary>
    /// <param name="stream">The stream to compress.</param>
    /// <returns>A System.IO.MemoryStream of compressed data.</returns>
    public static MemoryStream GZipCompress(this Stream stream) =>
        ProcessCompression(stream, dest => new GZipStream(dest, CompressionMode.Compress, leaveOpen: true));

    /// <summary>
    /// Compresses the given stream using the GZip algorithm and returns a System.IO.MemoryStream of compressed data.
    /// </summary>
    /// <param name="stream">The stream to compress.</param>
    /// <returns>A System.IO.MemoryStream of compressed data.</returns>
    public static Task<MemoryStream> GZipCompressAsync(this Stream stream, CancellationToken cancellationToken = default) =>
        ProcessCompressionAsync(stream, dest => new GZipStream(dest, CompressionMode.Compress, leaveOpen: true));

    /// <summary>
    /// Decompresses the given stream using the GZip algorithm and returns a System.IO.MemoryStream of decompressed data.
    /// </summary>
    /// <param name="stream">The stream to decompress.</param>
    /// <returns>A System.IO.MemoryStream of decompressed data.</returns>
    public static MemoryStream GZipDecompress(this Stream stream) =>
        ProcessDecompression(stream, src => new GZipStream(src, CompressionMode.Decompress, leaveOpen: true));

    /// <summary>
    /// Decompresses the given stream using the GZip algorithm and returns a System.IO.MemoryStream of decompressed data.
    /// </summary>
    /// <param name="stream">The stream to decompress.</param>
    /// <returns>A System.IO.MemoryStream of decompressed data.</returns>
    public static Task<MemoryStream> GZipDecompressAsync(this Stream stream, CancellationToken cancellationToken = default) =>
        ProcessDecompressionAsync(stream, src => new GZipStream(src, CompressionMode.Decompress, leaveOpen: true));

    /// <summary>
    /// Writes the stream content to a byte array.
    /// </summary>
    /// <param name="stream">The stream to convert to a byte array.</param>
    /// <returns>A new byte array.</returns>
    public static byte[] ToArray(this Stream stream)
    {
        using var memoryStream = new MemoryStream();
        stream.CopyTo(memoryStream);
        return memoryStream.ToArray();
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