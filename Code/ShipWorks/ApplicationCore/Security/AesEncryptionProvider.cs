using System;
using System.Security.Cryptography;
using System.Text;
using Interapptive.Shared.Security;
using ShipWorks.ApplicationCore.Licensing;

namespace ShipWorks.ApplicationCore.Security
{
    /// <summary>
    /// Class for encrypting and decrypting using AES
    /// </summary>
    public class AesEncryptionProvider : IEncryptionProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AesEncryptionProvider"/> class.
        /// </summary>
        public AesEncryptionProvider(IAesParams aesParams)
        {
            AesParams = aesParams;
        }

        /// <summary>
        /// The AES parameters.
        /// </summary>
        protected IAesParams AesParams { get; }

        /// <summary>
        /// AES encryption algorithm to use
        /// </summary>
        protected AesManaged Aes => new AesManaged()
        {
            IV = AesParams.InitializationVector,
            Key = AesParams.Key
        };

        /// <summary>
        /// Encrypts the given plain text.
        /// </summary>
        public string Encrypt(string plainText)
        {
            // AES can not encrypt empty strings. So when we get an empty string, set it to a fixed string,
            // so that when we decrypt later, we know it is actually supposed to be an empty string.
            if (string.IsNullOrWhiteSpace(plainText))
            {
                plainText = AesParams.EmptyValue;
            }

            return GetEncryptedString(plainText);
        }

        /// <summary>
        /// Decrypts the given encrypted text.
        /// </summary>
        public string Decrypt(string encryptedText)
        {
            if (string.IsNullOrWhiteSpace(encryptedText))
            {
                throw new EncryptionException("Cannot decrypt an empty string");
            }

            string decryptedText = GetDecryptedString(encryptedText);

            // If we get our fixed empty string value, we need to return an empty string.
            if (decryptedText.Equals(AesParams.EmptyValue))
            {
                decryptedText = "";
            }

            return decryptedText;
        }

        /// <summary>
        /// Gets the encrypted string.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        private string GetEncryptedString(string plainText)
        {
            try
            {
                ICryptoTransform encryptor = Aes.CreateEncryptor();

                byte[] buffer = Encoding.ASCII.GetBytes(plainText);

                return Convert.ToBase64String(encryptor.TransformFinalBlock(buffer, 0, buffer.Length));
            }
            catch (Exception ex)
            {
                throw new EncryptionException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Gets the decrypted string.
        /// </summary>
        /// <param name="encryptedText">The encrypted text.</param>
        protected virtual string GetDecryptedString(string encryptedText)
        {
            try
            {
                ICryptoTransform decryptor = Aes.CreateDecryptor();

                byte[] buffer = Convert.FromBase64String(encryptedText);

                return Encoding.ASCII.GetString(decryptor.TransformFinalBlock(buffer, 0, buffer.Length));
            }
            catch (Exception ex)
            {
                throw new EncryptionException(ex.Message, ex);
            }
        }
    }
}