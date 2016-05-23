using System;
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
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            AesManaged.Dispose();
        }
    }
}