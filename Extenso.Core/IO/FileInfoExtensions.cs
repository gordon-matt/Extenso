using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Extenso.IO
{
    /// <summary>
    /// Provides a set of static methods for querying and manipulating instances of System.IO.FileInfo.
    /// </summary>
    public static class FileInfoExtensions
    {
        /// <summary>
        /// Deserializes the binary data contained in the given file to an object of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize the binary data to.</typeparam>
        /// <param name="fileInfo">The binary file to deserialize.</param>
        /// <returns>The deserialized object from the binary data.</returns>
        public static T BinaryDeserialize<T>(this FileInfo fileInfo) where T : ISerializable
        {
            using (var fileStream = File.Open(fileInfo.FullName, FileMode.Open))
            {
                return fileStream.BinaryDeserialize<T>();
            }
        }

        /// <summary>
        /// Compresses the given file using the Deflate algorithm and returns the name of the compressed file.
        /// </summary>
        /// <param name="fileInfo">The file to be compressed.</param>
        /// <returns>The name of the new compressed file.</returns>
        public static string DeflateCompress(this FileInfo fileInfo)
        {
            string compressedFileName = $"{fileInfo.FullName}.cmp";

            using (var fileStreamIn = fileInfo.OpenRead())
            using (var fileStreamOut = File.Create(compressedFileName))
            using (var deflateStream = new DeflateStream(fileStreamOut, CompressionMode.Compress))
            {
                fileStreamIn.CopyTo(deflateStream);
            }

            return compressedFileName;
        }

        /// <summary>
        /// Decompresses the given file using the Deflate algorithm and returns the name of the decompressed file.
        /// </summary>
        /// <param name="fileInfo">The file to be decompressed.</param>
        /// <returns>The name of the new decompressed file.</returns>
        public static string DeflateDecompress(this FileInfo fileInfo)
        {
            string deCompressedFileName = fileInfo.FullName.Remove(fileInfo.FullName.Length - fileInfo.Extension.Length);

            using (var fileStreamIn = fileInfo.OpenRead())
            using (var fileStreamOut = File.Create(deCompressedFileName))
            using (var deflateStream = new DeflateStream(fileStreamIn, CompressionMode.Decompress))
            {
                deflateStream.CopyTo(fileStreamOut);
            }

            return deCompressedFileName;
        }

        /// <summary>
        /// Returns the given file's data as an array of bytes
        /// </summary>
        /// <param name="fileInfo">This System.IO.FileInfo instance.</param>
        /// <param name="maxBufferSize">The buffer size.</param>
        /// <returns>System.Byte[] representing the file data.</returns>
        public static byte[] GetBytes(this FileInfo fileInfo, long maxBufferSize = 0x1000)
        {
            byte[] buffer = new byte[(fileInfo.Length > maxBufferSize ? maxBufferSize : fileInfo.Length)];
            using (var fileStream = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.Read, buffer.Length))
            {
                fileStream.Read(buffer, 0, buffer.Length);
            }
            return buffer;
        }

        /// <summary>
        /// Gets the file size in GigaBytes
        /// </summary>
        /// <param name="fileInfo">The file which is to be measured in GB.</param>
        /// <returns>A System.Double representing the size of the file in GB.</returns>
        public static long GetFileSizeInGigaBytes(this FileInfo fileInfo)
        {
            return fileInfo.GetFileSizeInMegaBytes() / 1024;
        }

        /// <summary>
        /// Gets the file size in KiloBytes
        /// </summary>
        /// <param name="fileInfo">The file which is to be measured in KB.</param>
        /// <returns>A System.Double representing the size of the file in KB.</returns>
        public static long GetFileSizeInKiloBytes(this FileInfo fileInfo)
        {
            return fileInfo.Length / 1024;
        }

        /// <summary>
        /// Gets the file size in MegaBytes
        /// </summary>
        /// <param name="fileInfo">The file which is to be measured in MB.</param>
        /// <returns>A System.Double representing the size of the file in MB.</returns>
        public static long GetFileSizeInMegaBytes(this FileInfo fileInfo)
        {
            return fileInfo.GetFileSizeInKiloBytes() / 1024;
        }

        /// <summary>
        /// Compresses the file using the GZip algorithm and returns the name of the compressed file.
        /// </summary>
        /// <param name="fileInfo">The file to be compressed.</param>
        /// <returns>The name of the new compressed file.</returns>
        public static string GZipCompress(this FileInfo fileInfo)
        {
            string compressedFileName = $"{fileInfo.FullName}.gz";

            using (var fileStreamIn = fileInfo.OpenRead())
            using (var fileStreamOut = File.Create(compressedFileName))
            using (var gZipStream = new GZipStream(fileStreamOut, CompressionMode.Compress))
            {
                fileStreamIn.CopyTo(gZipStream);
            }

            return compressedFileName;
        }

        /// <summary>
        /// Decompresses the file using the GZip algorithm and returns the name of the decompressed file.
        /// </summary>
        /// <param name="fileInfo">This System.IO.FileInfo instance.</param>
        /// <returns>The name of the new decompressed file.</returns>
        public static string GZipDecompress(this FileInfo fileInfo)
        {
            // Get original file extension, for example:
            // "doc" from report.doc.gz.
            string deCompressedFileName = fileInfo.FullName.Remove(fileInfo.FullName.Length - fileInfo.Extension.Length);

            using (var fileStreamIn = fileInfo.OpenRead())
            using (var fileStreamOut = File.Create(deCompressedFileName))
            using (var gZipStream = new GZipStream(fileStreamIn, CompressionMode.Decompress))
            {
                gZipStream.CopyTo(fileStreamOut);
            }

            return deCompressedFileName;
        }

        /// <summary>
        /// Opens the text file, reads all lines of the file, and then closes the file.
        /// </summary>
        /// <param name="fileInfo">The file to read all text from.</param>
        /// <returns>A string containing all lines of the file.</returns>
        public static string ReadAllText(this FileInfo fileInfo)
        {
            return File.ReadAllText(fileInfo.FullName);

            //using (var fileStream = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read))
            //using (var streamReader = new StreamReader(fileStream))
            //{
            //    return streamReader.ReadToEnd();
            //}
        }

        /// <summary>
        /// Deserializes the XML data contained in the given file to an object of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize the XML data to.</typeparam>
        /// <param name="fileInfo">The XML file to deserialize.</param>
        /// <returns>The deserialized object from the XML data.</returns>
        public static T XmlDeserialize<T>(this FileInfo fileInfo)
        {
            using (var fileStream = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read))
            using (var streamReader = new StreamReader(fileStream))
            {
                return streamReader.ReadToEnd().XmlDeserialize<T>();
            }
        }
    }
}