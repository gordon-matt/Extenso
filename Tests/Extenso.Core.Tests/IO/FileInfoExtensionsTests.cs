using Extenso.IO;

namespace Extenso.Core.Tests.IO;

public class FileInfoExtensionsTests
{
    private static string CreateTempFileWithContent(string content)
    {
        string tempFile = Path.GetTempFileName();
        File.WriteAllText(tempFile, content);
        return tempFile;
    }

    private static void CleanupFiles(params ReadOnlySpan<string> filePaths)
    {
        foreach (string filePath in filePaths)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }

    [Theory]
    [InlineData("Hello Deflate!", CompressionAlgorithm.Deflate)]
    [InlineData("Hello GZip!", CompressionAlgorithm.GZip)]
    [InlineData("Hello Brotli!", CompressionAlgorithm.Brotli)]
    public void CompressAndDecompress_FileContentMatches(string content, CompressionAlgorithm algorithm)
    {
        // Arrange
        string originalFile = CreateTempFileWithContent(content);
        var fileInfo = new FileInfo(originalFile);

        string compressedFile = null;
        string decompressedFile = null;

        try
        {
            // Act
            compressedFile = algorithm switch
            {
                CompressionAlgorithm.Deflate => fileInfo.DeflateCompress(),
                CompressionAlgorithm.GZip => fileInfo.GZipCompress(),
                CompressionAlgorithm.Brotli => fileInfo.BrotliCompress(),
                _ => throw new InvalidOperationException("Unsupported algorithm")
            };

            var compressedFileInfo = new FileInfo(compressedFile);

            decompressedFile = algorithm switch
            {
                CompressionAlgorithm.Deflate => compressedFileInfo.DeflateDecompress(),
                CompressionAlgorithm.GZip => compressedFileInfo.GZipDecompress(),
                CompressionAlgorithm.Brotli => compressedFileInfo.BrotliDecompress(),
                _ => throw new InvalidOperationException("Unsupported algorithm")
            };

            string decompressedContent = File.ReadAllText(decompressedFile);

            // Assert
            Assert.Equal(content, decompressedContent);
        }
        finally
        {
            // Cleanup
            CleanupFiles(originalFile, compressedFile, decompressedFile);
        }
    }

    public enum CompressionAlgorithm
    {
        Deflate,
        GZip,
        Brotli
    }
}