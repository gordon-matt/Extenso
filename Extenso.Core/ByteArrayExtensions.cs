using System;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;

//using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Extenso
{
    public static class ByteArrayExtensions
    {
        /// <summary>
        /// Deserializes the Binary data contained in the specified System.Byte[].
        /// </summary>
        /// <typeparam name="T">The type of System.Object to be deserialized.</typeparam>
        /// <param name="data">This System.Byte[] instance.</param>
        /// <returns>The System.Object being deserialized.</returns>
        public static T BinaryDeserialize<T>(this byte[] data)
        {
            using (var stream = new MemoryStream(data))
            {
                var binaryFormatter = new BinaryFormatter();
                stream.Seek(0, SeekOrigin.Begin);
                var item = (T)binaryFormatter.Deserialize(stream);
                stream.Close();
                return item;
            }
        }
        
        /// <summary>
        /// Encrypts data with the System.Security.Cryptography.RSA algorithm.
        /// </summary>
        /// <param name="bytes">The data to be encrypted.</param>
        /// <param name="parameters">The parameters for System.Security.Cryptography.RSA.</param>
        /// <param name="doOAEPPadding">
        /// <para>true to perform direct System.Security.Cryptography.RSA encryption using</para>
        /// <para>OAEP padding (only available on a computer running Microsoft Windows XP or</para>
        /// <para>later); otherwise, false to use PKCS#1 v1.5 padding.</para>
        /// </param>
        /// <returns>The encrypted data.</returns>
        public static byte[] RSAEncrypt(this byte[] bytes, RSAParameters parameters, bool doOAEPPadding)
        {
            using (var rsaCryptoServiceProvider = new RSACryptoServiceProvider())
            {
                rsaCryptoServiceProvider.ImportParameters(parameters);
                return rsaCryptoServiceProvider.Encrypt(bytes, doOAEPPadding);
            }
        }

        /// <summary>
        /// Decrypts data with the System.Security.Cryptography.RSA algorithm.
        /// </summary>
        /// <param name="bytes">The data to be decrypted.</param>
        /// <param name="parameters">The parameters for System.Security.Cryptography.RSA.</param>
        /// <param name="doOAEPPadding">
        /// <para>true to perform direct System.Security.Cryptography.RSA encryption using</para>
        /// <para>OAEP padding (only available on a computer running Microsoft Windows XP or</para>
        /// <para>later); otherwise, false to use PKCS#1 v1.5 padding.</para>
        /// </param>
        /// <returns>The decrypted data, which is the original plain text before encryption.</returns>
        public static byte[] RSADecrypt(this byte[] bytes, RSAParameters parameters, bool doOAEPPadding)
        {
            using (var rsaCryptoServiceProvider = new RSACryptoServiceProvider())
            {
                rsaCryptoServiceProvider.ImportParameters(parameters);
                return rsaCryptoServiceProvider.Decrypt(bytes, doOAEPPadding);
            }
        }

        /// <summary>
        /// Encrypts the specified System.Byte[] using the TripleDES symmetric algorithm and returns the original System.String.
        /// </summary>
        /// <param name="data">The encrypted data to decrypt.</param>
        /// <param name="encoding">The System.Text.Encoding to use.</param>
        /// <param name="key">The secret key to use for the symmetric algorithm.</param>
        /// <param name="initializationVector">The initialization vector to use for the symmetric algorithm.</param>
        /// <returns>Decrypted System.Byte[] as a System.String.</returns>
        public static string TripleDESDecrypt(this byte[] data, Encoding encoding, byte[] key, byte[] initializationVector)
        {
            using (var memoryStream = new MemoryStream(data))
            using (var cryptoStream = new CryptoStream(
                memoryStream,
                new TripleDESCryptoServiceProvider().CreateDecryptor(key, initializationVector),
                CryptoStreamMode.Read))
            {
                byte[] bytes = new byte[data.Length];
                cryptoStream.Read(bytes, 0, bytes.Length);
                return encoding.GetString(bytes);
            }
        }

        public static MemoryStream ToStream(this byte[] bytes)
        {
            return new MemoryStream(bytes);
        }

        public static T XmlDeserialize<T>(this byte[] bytes)
        {
            return bytes.XmlDeserialize<T>(new UTF8Encoding());
        }

        public static T XmlDeserialize<T>(this byte[] bytes, Encoding encoding)
        {
            string s = encoding.GetString(bytes);
            return s.XmlDeserialize<T>();
        }

        public static string ToHexString(this byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", "");
        }

        public static string ToFileSize(this long byteCount)
        {
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" };
            if (byteCount == 0)
                return "0" + suf[0];
            var bytes = Math.Abs(byteCount);
            var place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            var num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(byteCount) * num).ToString(CultureInfo.InvariantCulture) + suf[place];
        }
    }
}