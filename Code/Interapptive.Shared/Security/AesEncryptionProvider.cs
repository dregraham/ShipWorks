using System;
using System.Security.Cryptography;
using System.Text;

namespace Interapptive.Shared.Security
{
    /// <summary>
    /// Class for encrypting and decrypting using AES
    /// </summary>
    public class AesEncryptionProvider : IEncryptionProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AesEncryptionProvider"/> class.
        /// </summary>
        public AesEncryptionProvider(byte[] key, byte[] initializationVector)
        {
            Aes = new AesManaged()
            {
                IV = initializationVector,
                Key = key
            };
        }

        /// <summary>
        /// AES encryption algorithm to use
        /// </summary>
        protected AesManaged Aes { get; } 

        /// <summary>
        /// Encrypts the given plain text.
        /// </summary>
        public virtual string Encrypt(string plainText)
        {
            try
            {
                ICryptoTransform encryptor = Aes.CreateEncryptor();

                byte[] buffer = Encoding.ASCII.GetBytes(plainText);

                return Convert.ToBase64String(encryptor.TransformFinalBlock(buffer, 0, buffer.Length));
            }
            catch (Exception ex) when (ex.GetType() != typeof(EncryptionException))
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