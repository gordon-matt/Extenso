using System.Text;
using Extenso.IO;

namespace Extenso;

/// <summary>
/// Provides a set of static methods for querying and manipulating byte arrays.
/// </summary>
public static class ByteArrayExtensions
{
    extension(byte[] source)
    {
        /// <summary>
        /// Encodes the given object to a Base64 encoded string.
        /// </summary>
        /// <param name="prependLength">
        /// Specifies whether to prepend the length of the byte array to the Base64 string.
        /// If true, the length and base64 encoded string will be separated by a colon.
        /// </param>
        /// <returns>A Base64 encoded string of the byte array.</returns>
        public string Base64Serialize(bool prependLength = false) => prependLength
            ? $"{source.Length}:{Convert.ToBase64String(source, 0, source.Length, Base64FormattingOptions.None)}"
            : Convert.ToBase64String(source, 0, source.Length, Base64FormattingOptions.None);

        /// <summary>
        /// Deserializes the data contained in the given byte array to an object of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize the binary data to.</typeparam>
        /// <returns>The deserialized object from the byte array.</returns>
        public T BinaryDeserialize<T>()
        {
            using var stream = new MemoryStream(source);
            return stream.BinaryDeserialize<T>();
        }

        //public string Decrypt(Encoding encoding, ICryptoTransform cryptoTransform)
        //{
        //    using (var memoryStream = new MemoryStream(source))
        //    using (var cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Read))
        //    {
        //        byte[] bytes = new byte[source.Length];
        //        cryptoStream.Read(bytes, 0, bytes.Length);
        //        return encoding.GetString(bytes);
        //    }
        //}

        ///// <summary>
        ///// Encrypts data with the System.Security.Cryptography.RSA algorithm.
        ///// </summary>
        ///// <param name="parameters">The parameters for System.Security.Cryptography.RSA.</param>
        ///// <param name="fOAEP">
        ///// true to perform direct System.Security.Cryptography.RSA encryption using OAEP
        ///// padding (only available on a computer running Windows XP or later); otherwise,
        ///// false to use PKCS#1 v1.5 padding.
        ///// </param>
        ///// <returns>The encrypted data.</returns>
        //public byte[] RSAEncrypt(RSAParameters parameters, bool fOAEP)
        //{
        //    using (var rsaCryptoServiceProvider = new RSACryptoServiceProvider())
        //    {
        //        rsaCryptoServiceProvider.ImportParameters(parameters);
        //        return rsaCryptoServiceProvider.Encrypt(source, fOAEP);
        //    }
        //}

        ///// <summary>
        ///// Decrypts data with the System.Security.Cryptography.RSA algorithm.
        ///// </summary>
        ///// <param name="parameters">The parameters for System.Security.Cryptography.RSA.</param>
        ///// <param name="fOAEP">
        ///// true to perform direct System.Security.Cryptography.RSA decryption using OAEP
        ///// padding (only available on a computer running Microsoft Windows XP or later);
        ///// otherwise, false to use PKCS#1 v1.5 padding.
        ///// </param>
        ///// <returns>The decrypted data, which is the original plain text before encryption.</returns>
        //public byte[] RSADecrypt(RSAParameters parameters, bool fOAEP)
        //{
        //    using (var rsaCryptoServiceProvider = new RSACryptoServiceProvider())
        //    {
        //        rsaCryptoServiceProvider.ImportParameters(parameters);
        //        return rsaCryptoServiceProvider.Decrypt(source, fOAEP);
        //    }
        //}

        ///// <summary>
        ///// Decrypts the specified byte array using the TripleDES symmetric algorithm and returns the original string.
        ///// </summary>
        ///// <param name="encoding">The System.Text.Encoding to use.</param>
        ///// <param name="key">The secret key to use for the symmetric algorithm.</param>
        ///// <param name="initializationVector">The initialization vector to use for the symmetric algorithm.</param>
        ///// <returns>The decrypted data, which is the original plain text before encryption.</returns>
        //public string TripleDESDecrypt(Encoding encoding, byte[] key, byte[] initializationVector)
        //{
        //    using (var memoryStream = new MemoryStream(source))
        //    using (var cryptoStream = new CryptoStream(
        //        memoryStream,
        //        TripleDES.Create().CreateDecryptor(key, initializationVector),
        //        CryptoStreamMode.Read))
        //    {
        //        byte[] bytes = new byte[source.Length];
        //        cryptoStream.Read(bytes, 0, bytes.Length);
        //        return encoding.GetString(bytes);
        //    }
        //}

        /// <summary>
        /// Creates a new non-resizable instance of the System.IO.MemoryStream class based on the specified byte array.
        /// </summary>
        /// <returns>A System.IO.MemoryStream based on the specified byte array.</returns>
        public MemoryStream ToStream() => new(source);

        /// <summary>
        /// Decodes all the bytes in the specified byte array into a string and then deserializes the XML contained therein to an object of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize the XML to.</typeparam>
        /// <param name="encoding">The character encoding to be used for decoding the byte array.</param>
        /// <returns>The deserialized object from the XML data in the encoded byte array.</returns>
        public T XmlDeserialize<T>(Encoding encoding)
        {
            string s = encoding.GetString(source);
            return s.XmlDeserialize<T>();
        }

        /// <summary>
        /// Converts the numeric value of each element of the specified array of bytes to its equivalent hexadecimal string representation.
        /// </summary>
        /// <returns>A string of hexadecimal pairs separated by hyphens, where each pair represents the corresponding element in value; for example, "7F-2C-4A-00".</returns>
        public string ToHex() => BitConverter.ToString(source);
    }
}