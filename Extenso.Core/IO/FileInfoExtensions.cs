using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Extenso.IO
{
    public static class FileInfoExtensions
    {
        /// <summary>
        /// Deserializes the Binary data contained in the specified file.
        /// </summary>
        /// <typeparam name="T">The type of System.Object to be deserialized.</typeparam>
        /// <param name="fileInfo">This System.IO.FileInfo instance.</param>
        /// <returns>The System.Object being deserialized.</returns>
        public static T BinaryDeserialize<T>(this FileInfo fileInfo) where T : ISerializable
        {
            using (Stream stream = File.Open(fileInfo.FullName, FileMode.Open))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                T item = (T)binaryFormatter.Deserialize(stream);
                stream.Close();
                return item;
            }
        }

        /// <summary>
        /// Compresses the file using the Deflate algorithm and returns the name of the compressed file.
        /// </summary>
        /// <param name="fileInfo">This System.IO.FileInfo instance.</param>
        /// <returns>The name of the new compressed file.</returns>
        public static void DeflateCompress(this FileInfo fileInfo)
        {
            using (FileStream fsIn = fileInfo.OpenRead())
            {
                if (fileInfo.Extension != ".cmp")
                {
                    using (FileStream fsOut = File.Create(fileInfo.FullName + ".cmp"))
                    {
                        using (DeflateStream deflateStream = new DeflateStream(fsOut, CompressionMode.Compress))
                        {
                            fsIn.CopyTo(deflateStream);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Decompresses the file using the Deflate algorithm and returns the name of the decompressed file.
        /// </summary>
        /// <param name="fileInfo">This System.IO.FileInfo instance.</param>
        /// <returns>The name of the new decompressed file.</returns>
        public static void DeflateDecompress(this FileInfo fileInfo)
        {
            using (FileStream fsIn = fileInfo.OpenRead())
            {
                string originalName = fileInfo.FullName.Remove(fileInfo.FullName.Length - fileInfo.Extension.Length);

                using (FileStream fsOut = File.Create(originalName))
                {
                    using (DeflateStream deflateStream = new DeflateStream(fsIn, CompressionMode.Decompress))
                    {
                        deflateStream.CopyTo(fsOut);
                    }
                }
            }
        }

        public static byte[] GetBytes(this FileInfo fileInfo)
        {
            return fileInfo.GetBytes(0x1000);
        }

        /// <summary>
        /// Gets the file data as an array of bytes
        /// </summary>
        /// <param name="fileInfo">This System.IO.FileInfo instance.</param>
        /// <param name="maxBufferSize">The buffer size.</param>
        /// <returns>System.Byte[] representing the file data.</returns>
        public static byte[] GetBytes(this FileInfo fileInfo, long maxBufferSize)
        {
            byte[] buffer = new byte[(fileInfo.Length > maxBufferSize ? maxBufferSize : fileInfo.Length)];
            using (FileStream fileStream = new FileStream(
                fileInfo.FullName,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read,
                buffer.Length))
            {
                fileStream.Read(buffer, 0, buffer.Length);
                //fileStream.Close();
            }
            return buffer;
        }

        /// <summary>
        /// Gets the file size in KiloBytes
        /// </summary>
        /// <param name="fileInfo">This System.IO.FileInfo instance.</param>
        /// <returns>System.Double representing the size of the file.</returns>
        public static long GetFileSizeInKiloBytes(this FileInfo fileInfo)
        {
            return fileInfo.Length / 1024;
        }

        /// <summary>
        /// Gets the file size in MegaBytes
        /// </summary>
        /// <param name="fileInfo">This System.IO.FileInfo instance.</param>
        /// <returns>System.Double representing the size of the file.</returns>
        public static long GetFileSizeInMegaBytes(this FileInfo fileInfo)
        {
            return fileInfo.GetFileSizeInKiloBytes() / 1024;
        }

        /// <summary>
        /// Gets the file size in GigaBytes
        /// </summary>
        /// <param name="fileInfo">This System.IO.FileInfo instance.</param>
        /// <returns>System.Double representing the size of the file.</returns>
        public static long GetFileSizeInGigaBytes(this FileInfo fileInfo)
        {
            return fileInfo.GetFileSizeInMegaBytes() / 1024;
        }

        public static string GetText(this FileInfo fileInfo)
        {
            using (FileStream fileStream = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader streamReader = new StreamReader(fileStream))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// Compresses the file using the GZip algorithm and returns the name of the compressed file.
        /// </summary>
        /// <param name="fileInfo">This System.IO.FileInfo instance.</param>
        /// <returns>The name of the new compressed file.</returns>
        public static void GZipCompress(this FileInfo fileInfo)
        {
            using (FileStream fsIn = fileInfo.OpenRead())
            {
                if (fileInfo.Extension != ".gz")
                {
                    using (FileStream fsOut = File.Create(fileInfo.FullName + ".gz"))
                    {
                        using (GZipStream gZipStream = new GZipStream(fsOut, CompressionMode.Compress))
                        {
                            fsIn.CopyTo(gZipStream);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Decompresses the file using the GZip algorithm and returns the name of the decompressed file.
        /// </summary>
        /// <param name="fileInfo">This System.IO.FileInfo instance.</param>
        /// <returns>The name of the new decompressed file.</returns>
        public static void GZipDecompress(this FileInfo fileInfo)
        {
            using (FileStream fsIn = fileInfo.OpenRead())
            {
                // Get original file extension, for example:
                // "doc" from report.doc.gz.
                string originalName = fileInfo.FullName.Remove(fileInfo.FullName.Length - fileInfo.Extension.Length);

                using (FileStream fsOut = File.Create(originalName))
                {
                    using (GZipStream gZipStream = new GZipStream(fsIn, CompressionMode.Decompress))
                    {
                        gZipStream.CopyTo(fsOut);
                    }
                }
            }
        }

        //public static DataTable ReadCsv(this FileInfo fileInfo, bool hasHeaderRow, params string[] delimeters)
        //{
        //    if (delimeters.IsNullOrEmpty())
        //    {
        //        delimeters = new[] { "," };
        //    }

        //    using (var parser = new TextFieldParser(fileInfo.FullName))
        //    {
        //        parser.TextFieldType = FieldType.Delimited;
        //        parser.SetDelimiters(delimeters);
        //        parser.HasFieldsEnclosedInQuotes = false;

        //        var table = new DataTable(Path.GetFileNameWithoutExtension(fileInfo.Name));

        //        int lineNumber = 1;
        //        while (!parser.EndOfData)
        //        {
        //            string[] fields;
        //            try
        //            {
        //                fields = parser.ReadFields();
        //            }
        //            catch (MalformedLineException x)
        //            {
        //                var logger = LoggingUtilities.Resolve();
        //                logger.Error("Error when reading CSV file: " + fileInfo.FullName, x);
        //                continue;
        //            }

        //            if (lineNumber == 1)
        //            {
        //                if (hasHeaderRow)
        //                {
        //                    foreach (string field in fields)
        //                    {
        //                        table.Columns.Add(field);
        //                    }
        //                }
        //                else
        //                {
        //                    for (int i = 0; i < fields.Count(); i++)
        //                    {
        //                        table.Columns.Add("Column" + (i + 1));
        //                    }
        //                    table.Rows.Add(fields);
        //                }
        //            }
        //            else
        //            {
        //                var row = table.NewRow();
        //                for (int i = 0; i < table.Columns.Count; i++)
        //                {
        //                    row[i] = fields[i];
        //                }
        //                table.Rows.Add(row);

        //                //table.Rows.Add(fields); //  <-- this version breaks when accidenteally have extra comma at end of line
        //            }

        //            lineNumber++;
        //        }

        //        return table;
        //    }
        //}

        /// <summary>
        /// Deserializes the XML data contained in the specified file.
        /// </summary>
        /// <typeparam name="T">The type of System.Object to be deserialized.</typeparam>
        /// <param name="fileInfo">This System.IO.FileInfo instance.</param>
        /// <returns>The System.Object being deserialized.</returns>
        public static T XmlDeserialize<T>(this FileInfo fileInfo)
        {
            string xml = string.Empty;
            using (FileStream fs = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    return sr.ReadToEnd().XmlDeserialize<T>();
                }
            }
        }
    }
}