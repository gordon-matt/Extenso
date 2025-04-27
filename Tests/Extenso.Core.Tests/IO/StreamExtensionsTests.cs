using System.Text;
using Extenso.IO;

namespace Extenso.Core.Tests.IO;

public class StreamExtensionsTests
{
    [Fact]
    public void BinarySerialization()
    {
        var testObj = new TestObject
        {
            Value1 = 42,
            Value2 = "Hello World",
            Value3 = 123.45m
        };

        using var stream = new MemoryStream();

        // Serialize the TestObject to stream
        stream.BinarySerialize(testObj);

        // Deserialize it back
        var result = stream.BinaryDeserialize<TestObject>();

        Assert.Equal(testObj.Value1, result.Value1);
        Assert.Equal(testObj.Value2, result.Value2);
        Assert.Equal(testObj.Value3, result.Value3);
    }

    [Fact]
    public void BrotliCompression()
        => CompressionRoundtripTest(s => s.BrotliCompress(), s => s.BrotliDecompress());

    [Fact]
    public async Task BrotliCompressionAsync()
        => await CompressionRoundtripTestAsync((s, ct) => s.BrotliCompressAsync(cancellationToken: ct), (s, ct) => s.BrotliDecompressAsync(ct));

    [Fact]
    public void DeflateCompression()
        => CompressionRoundtripTest(s => s.DeflateCompress(), s => s.DeflateDecompress());

    [Fact]
    public async Task DeflateCompressionAsync()
        => await CompressionRoundtripTestAsync((s, ct) => s.DeflateCompressAsync(cancellationToken: ct), (s, ct) => s.DeflateDecompressAsync(ct));

    [Fact]
    public void GZipCompression()
        => CompressionRoundtripTest(s => s.GZipCompress(), s => s.GZipDecompress());

    [Fact]
    public async Task GZipCompressionAsync()
        => await CompressionRoundtripTestAsync((s, ct) => s.GZipCompressAsync(cancellationToken: ct), (s, ct) => s.GZipDecompressAsync(ct));

    [Fact]
    public void ToArray()
    {
        string originalText = "Sample text content";
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(originalText));

        byte[] byteArray = stream.ToArray();
        string resultText = Encoding.UTF8.GetString(byteArray);

        Assert.Equal(originalText, resultText);
    }

    private static void CompressionRoundtripTest(
        Func<Stream, MemoryStream> compress,
        Func<Stream, MemoryStream> decompress)
    {
        string source = "the quick brown fox jumped over the lazy dog";
        using var sourceStream = source.ToStream();
        using var compressedStream = compress(sourceStream);
        using var decompressedStream = decompress(compressedStream);
        using var reader = new StreamReader(decompressedStream);
        string decompressed = reader.ReadToEnd();
        Assert.Equal(source, decompressed);
    }

    private static async Task CompressionRoundtripTestAsync(
        Func<Stream, CancellationToken, Task<MemoryStream>> compress,
        Func<Stream, CancellationToken, Task<MemoryStream>> decompress)
    {
        string source = "the quick brown fox jumped over the lazy dog";
        using var sourceStream = source.ToStream();
        using var compressedStream = await compress(sourceStream, CancellationToken.None);
        using var decompressedStream = await decompress(compressedStream, CancellationToken.None);
        using var reader = new StreamReader(decompressedStream);
        string decompressed = await reader.ReadToEndAsync();
        Assert.Equal(source, decompressed);
    }
}