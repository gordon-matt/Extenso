using System.Globalization;
using System.IO.Compression;

namespace Extenso.IO;

/// <summary>
/// Provides a set of static methods for querying and manipulating instances of System.IO.FileInfo.
/// </summary>
public static class FileInfoExtensions
{
    extension(FileInfo source)
    {
        /// <summary>
        /// Deserializes the binary data contained in the given file to an object of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize the binary data to.</typeparam>
        /// <returns>The deserialized object from the binary data.</returns>
        public T BinaryDeserialize<T>()
        {
            using var fileStream = File.Open(source.FullName, FileMode.Open);
            return fileStream.BinaryDeserialize<T>();
        }

        /// <summary>
        /// Compresses the given file using the Brotli algorithm and returns the name of the compressed file.
        /// </summary>
        /// <returns>The name of the new compressed file.</returns>
        public string BrotliCompress(CompressionLevel compressionLevel = CompressionLevel.Optimal)
            => source.Compress(CompressionAlgorithm.Brotli, compressionLevel);

        public async Task<string> BrotliCompressAsync(CompressionLevel compressionLevel = CompressionLevel.Optimal)
            => await source.CompressAsync(CompressionAlgorithm.Brotli, compressionLevel);

        /// <summary>
        /// Decompresses the given file using the Brotli algorithm and returns the name of the decompressed file.
        /// </summary>
        /// <returns>The name of the new decompressed file.</returns>
        public string BrotliDecompress()
            => source.Decompress(CompressionAlgorithm.Brotli);

        public async Task<string> BrotliDecompressAsync()
            => await source.DecompressAsync(CompressionAlgorithm.Brotli);

        /// <summary>
        /// Compresses the given file using the Deflate algorithm and returns the name of the compressed file.
        /// </summary>
        /// <returns>The name of the new compressed file.</returns>
        public string DeflateCompress(CompressionLevel compressionLevel = CompressionLevel.Optimal)
            => source.Compress(CompressionAlgorithm.Deflate, compressionLevel);

        public async Task<string> DeflateCompressAsync(CompressionLevel compressionLevel = CompressionLevel.Optimal)
            => await source.CompressAsync(CompressionAlgorithm.Deflate, compressionLevel);

        /// <summary>
        /// Decompresses the given file using the Deflate algorithm and returns the name of the decompressed file.
        /// </summary>
        /// <returns>The name of the new decompressed file.</returns>
        public string DeflateDecompress()
            => source.Decompress(CompressionAlgorithm.Deflate);

        public async Task<string> DeflateDecompressAsync()
            => await source.DecompressAsync(CompressionAlgorithm.Deflate);

        /// <summary>
        /// Gets the file size and includes the unit of measurement suffix.
        /// </summary>
        /// <returns>A System.String representing the size of the file.</returns>
        /// <example><code>string size = new FileInfo(path).FileSize();</code></example>
        public string FileSize()
        {
            long length = source.Length;
            string[] suffixes = { "B", "KB", "MB", "GB", "TB", "PB", "EB" };

            if (length == 0)
            {
                return "0B";
            }

            long bytes = Math.Abs(length);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);

            return (Math.Sign(length) * num).ToString(CultureInfo.InvariantCulture) + suffixes[place];
        }

        /// <summary>
        /// Gets the file size in GigaBytes
        /// </summary>
        /// <returns>A System.Double representing the size of the file in GB.</returns>
        public long FileSizeInGigaBytes() => source.FileSizeInMegaBytes() / 1024;

        /// <summary>
        /// Gets the file size in KiloBytes
        /// </summary>
        /// <returns>A System.Double representing the size of the file in KB.</returns>
        public long FileSizeInKiloBytes() => source.Length / 1024;

        /// <summary>
        /// Gets the file size in MegaBytes
        /// </summary>
        /// <returns>A System.Double representing the size of the file in MB.</returns>
        public long FileSizeInMegaBytes() => source.FileSizeInKiloBytes() / 1024;

        /// <summary>
        /// Compresses the file using the GZip algorithm and returns the name of the compressed file.
        /// </summary>
        /// <returns>The name of the new compressed file.</returns>
        public string GZipCompress(CompressionLevel compressionLevel = CompressionLevel.Optimal)
            => source.Compress(CompressionAlgorithm.GZip, compressionLevel);

        public async Task<string> GZipCompressAsync(CompressionLevel compressionLevel = CompressionLevel.Optimal)
            => await source.CompressAsync(CompressionAlgorithm.GZip, compressionLevel);

        /// <summary>
        /// Decompresses the file using the GZip algorithm and returns the name of the decompressed file.
        /// </summary>
        /// <returns>The name of the new decompressed file.</returns>
        public string GZipDecompress()
            => source.Decompress(CompressionAlgorithm.GZip);

        public async Task<string> GZipDecompressAsync()
            => await source.DecompressAsync(CompressionAlgorithm.GZip);

        /// <summary>
        /// Opens a binary file, reads the contents of the file into a byte array, and then closes the file.
        /// </summary>
        /// <returns> A byte array containing the contents of the file.</returns>
        public byte[] ReadAllBytes() => File.ReadAllBytes(source.FullName);

        /// <summary>
        /// Opens the text file, reads all lines of the file, and then closes the file.
        /// </summary>
        /// <returns>A string containing all lines of the file.</returns>
        public string ReadAllText() => File.ReadAllText(source.FullName);//using (var fileStream = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read))//using (var streamReader = new StreamReader(fileStream))//{//    return streamReader.ReadToEnd();//}
        /// <summary>
        /// Opens a binary file, reads the specified amount of contents of the file into a byte array, and then closes the file.
        /// </summary>
        /// <param name="maxBufferSize">A maximum number of bytes to read.</param>
        /// <returns> A byte array containing the contents of the file. If maxBufferSize is less than the file size, not all data will be returned.</returns>
        public byte[] ReadBytes(long maxBufferSize = 0x1000)
        {
            int bufferSize = (int)Math.Min(source.Length, maxBufferSize);
            byte[] buffer = new byte[bufferSize];

            using (var fileStream = new FileStream(source.FullName, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize))
            {
                fileStream.ReadExactly(buffer);
            }

            return buffer;
        }

        /// <summary>
        /// Deserializes the XML data contained in the given file to an object of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize the XML data to.</typeparam>
        /// <returns>The deserialized object from the XML data.</returns>
        public T XmlDeserialize<T>()
        {
            using var fileStream = new FileStream(source.FullName, FileMode.Open, FileAccess.Read);
            using var streamReader = new StreamReader(fileStream);
            return streamReader.ReadToEnd().XmlDeserialize<T>();
        }
    }

    private static string Compress(this FileInfo fileInfo, CompressionAlgorithm algorithm, CompressionLevel compressionLevel)
    {
        string compressedFileName = $"{fileInfo.FullName}.{GetCompressionExtension(algorithm)}";

        using var inputStream = fileInfo.OpenRead();
        using var compressedStream = CompressStream(inputStream, algorithm, compressionLevel);

        using var outputStream = File.Create(compressedFileName);
        compressedStream.CopyTo(outputStream);

        return compressedFileName;
    }

    private static async Task<string> CompressAsync(this FileInfo fileInfo, CompressionAlgorithm algorithm, CompressionLevel compressionLevel)
    {
        string compressedFileName = $"{fileInfo.FullName}.{GetCompressionExtension(algorithm)}";

        using var inputStream = fileInfo.OpenRead();
        using var compressedStream = await CompressStreamAsync(inputStream, algorithm, compressionLevel);

        using var outputStream = File.Create(compressedFileName);
        await compressedStream.CopyToAsync(outputStream);

        return compressedFileName;
    }

    private static MemoryStream CompressStream(Stream inputStream, CompressionAlgorithm algorithm, CompressionLevel compressionLevel) => algorithm switch
    {
        CompressionAlgorithm.Deflate => inputStream.DeflateCompress(compressionLevel),
        CompressionAlgorithm.GZip => inputStream.GZipCompress(compressionLevel),
        CompressionAlgorithm.Brotli => inputStream.BrotliCompress(compressionLevel),
        _ => throw new ArgumentOutOfRangeException(nameof(algorithm), $"Unsupported compression algorithm: {algorithm}")
    };

    private static async Task<MemoryStream> CompressStreamAsync(Stream inputStream, CompressionAlgorithm algorithm, CompressionLevel compressionLevel) => algorithm switch
    {
        CompressionAlgorithm.Deflate => await inputStream.DeflateCompressAsync(compressionLevel),
        CompressionAlgorithm.GZip => await inputStream.GZipCompressAsync(compressionLevel),
        CompressionAlgorithm.Brotli => await inputStream.BrotliCompressAsync(compressionLevel),
        _ => throw new ArgumentOutOfRangeException(nameof(algorithm), $"Unsupported compression algorithm: {algorithm}")
    };

    private static string Decompress(this FileInfo fileInfo, CompressionAlgorithm algorithm)
    {
        string decompressedFileName = GetDecompressedFileName(fileInfo);

        using var inputStream = fileInfo.OpenRead();
        using var decompressedStream = DecompressStream(inputStream, algorithm);

        using var outputStream = File.Create(decompressedFileName);
        decompressedStream.CopyTo(outputStream);

        return decompressedFileName;
    }

    private static async Task<string> DecompressAsync(this FileInfo fileInfo, CompressionAlgorithm algorithm)
    {
        string decompressedFileName = GetDecompressedFileName(fileInfo);

        using var inputStream = fileInfo.OpenRead();
        using var decompressedStream = await DecompressStreamAsync(inputStream, algorithm);

        using var outputStream = File.Create(decompressedFileName);
        await decompressedStream.CopyToAsync(outputStream);

        return decompressedFileName;
    }

    private static MemoryStream DecompressStream(Stream inputStream, CompressionAlgorithm algorithm) => algorithm switch
    {
        CompressionAlgorithm.Deflate => inputStream.DeflateDecompress(),
        CompressionAlgorithm.GZip => inputStream.GZipDecompress(),
        CompressionAlgorithm.Brotli => inputStream.BrotliDecompress(),
        _ => throw new ArgumentOutOfRangeException(nameof(algorithm), $"Unsupported decompression algorithm: {algorithm}")
    };

    private static async Task<MemoryStream> DecompressStreamAsync(Stream inputStream, CompressionAlgorithm algorithm) => algorithm switch
    {
        CompressionAlgorithm.Deflate => await inputStream.DeflateDecompressAsync(),
        CompressionAlgorithm.GZip => await inputStream.GZipDecompressAsync(),
        CompressionAlgorithm.Brotli => await inputStream.BrotliDecompressAsync(),
        _ => throw new ArgumentOutOfRangeException(nameof(algorithm), $"Unsupported decompression algorithm: {algorithm}")
    };

    private static string GetCompressionExtension(CompressionAlgorithm algorithm) =>
        algorithm switch
        {
            CompressionAlgorithm.Deflate => "cmp",
            CompressionAlgorithm.GZip => "gz",
            CompressionAlgorithm.Brotli => "br",
            _ => throw new ArgumentOutOfRangeException(nameof(algorithm), $"Unsupported compression algorithm: {algorithm}")
        };

    private static string GetDecompressedFileName(FileInfo fileInfo) =>
        fileInfo.FullName[..^fileInfo.Extension.Length];

    private enum CompressionAlgorithm
    {
        Deflate,
        GZip,
        Brotli
    }
}