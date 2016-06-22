using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Interapptive.Shared.Security
{
    /// <summary>
    /// Class for encrypting and decrypting using AES
    /// </summary>
    public class AesEncryptionProvider : IEncryptionProvider, IDisposable
    {
        private readonly ICipherKey cipherKey;

        private AesManaged AesManaged => new AesManaged { IV = cipherKey.InitializationVector, Key = cipherKey.Key };

        /// <summary>
        /// Initializes a new instance of the <see cref="AesEncryptionProvider"/> class.
        /// </summary>
        public AesEncryptionProvider(ICipherKey cipherKey)
        {
            this.cipherKey = cipherKey;
        }

        /// <summary>
        /// Encrypts the given plain text.
        /// </summary>
        public virtual string Encrypt(string plainText)
        {
            if (string.IsNullOrWhiteSpace(plainText))
            {
                throw new EncryptionException("Cannot encrypt an empty string");
            }

            try
            {
                byte[] buffer = Encoding.ASCII.GetBytes(plainText);
                ICryptoTransform encryptor = AesManaged.CreateEncryptor();
                return Convert.ToBase64String(encryptor.TransformFinalBlock(buffer, 0, buffer.Length));
            }
            catch (Exception ex)
            {
                throw new EncryptionException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Decrypts the given encrypted text.
        /// </summary>
        public virtual string Decrypt(string encryptedText)
        {
            if (string.IsNullOrWhiteSpace(encryptedText))
            {
                throw new EncryptionException("Cannot decrypt an empty string");
            }

            try
            {
                byte[] buffer = Convert.FromBase64String(encryptedText);
                ICryptoTransform decryptor = AesManaged.CreateDecryptor();
                return Encoding.ASCII.GetString(decryptor.TransformFinalBlock(buffer, 0, buffer.Length));
            }
            catch (Exception ex)
            {
                throw new EncryptionException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Encrypts the source stream and writes the encrypted stream to the output stream.
        /// </summary>
        public void Encrypt(Stream sourceStream, Stream outputStream)
        {
            UpdateStream(sourceStream, outputStream, Encrypt);
        }

        /// <summary>
        /// Decrypts the source stream and writes the decrypted stream to the output stream.
        /// </summary>
        public void Decrypt(Stream sourceStream, Stream outputStream)
        {
            UpdateStream(sourceStream, outputStream, Decrypt);
        }

        /// <summary>
        /// Updates the given stream using the function provided.
        /// </summary>
        private void UpdateStream(Stream sourceStream, Stream outputStream, Func<string, string> updateAction)
        {
            MemoryStream sourceMemoryStream = sourceStream as MemoryStream;
            MemoryStream outputMemoryStream = outputStream as MemoryStream;

            if (sourceMemoryStream == null)
            {
                throw new ArgumentException(@"sourceMemoryStream must be of type MemoryStream for AesEncryptionProvider", nameof(sourceMemoryStream));
            }
            if (outputMemoryStream == null)
            {
                throw new ArgumentException(@"outputMemoryStream must be of type MemoryStream for AesEncryptionProvider", nameof(outputMemoryStream));
            }

            sourceMemoryStream.Position = 0;

            StreamReader reader = new StreamReader(sourceMemoryStream);
            string text = reader.ReadToEnd();

            text = updateAction(text);
            byte[] byteArray = Encoding.ASCII.GetBytes(text);

            ClearStream(outputMemoryStream);

            outputMemoryStream.Write(byteArray, 0, byteArray.Length);
            outputMemoryStream.Position = 0;
        }

        /// <summary>
        /// Clear out the given stream so that we can write the new values to it.
        /// </summary>
        private void ClearStream(MemoryStream source)
        {
            byte[] buffer = source.GetBuffer();
            Array.Clear(buffer, 0, buffer.Length);
            source.Position = 0;
            source.SetLength(0);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            AesManaged.Dispose();
        }
    }
}