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
        private readonly ICipherKey cipherKey;
        private AesManaged aesManaged;

        /// <summary>
        /// Initializes a new instance of the <see cref="AesEncryptionProvider"/> class.
        /// </summary>
        public AesEncryptionProvider(ICipherKey cipherKey)
        {
            this.cipherKey = cipherKey;
        }

        /// <summary>
        /// AES encryption algorithm to use
        /// </summary>
        protected AesManaged Aes
        {
            get
            {
                if (aesManaged == null)
                {
                    aesManaged = new AesManaged()
                    {
                        IV = cipherKey.InitializationVector,
                        Key = cipherKey.Key
                    };
                }

                return aesManaged;
            }
        }
        
        /// <summary>
        /// Encrypts the given plain text.
        /// </summary>
        public virtual string Encrypt(string plainText)
        {
            try
            {
                byte[] buffer = Encoding.ASCII.GetBytes(plainText);

                ICryptoTransform encryptor = Aes.CreateEncryptor();
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

                ICryptoTransform decryptor = Aes.CreateDecryptor();
                return Encoding.ASCII.GetString(decryptor.TransformFinalBlock(buffer, 0, buffer.Length));
            }
            catch (Exception ex)
            {
                throw new EncryptionException(ex.Message, ex);
            }
        }
    }
}