using System;
using System.IO;

namespace Interapptive.Shared.Security
{
    /// <summary>
    /// 
    /// </summary>
    public class SecureTextEncryptionProvider : IEncryptionProvider
    {
        private readonly string salt;

        /// <summary>
        /// Initializes a new instance of the <see cref="SecureTextEncryptionProvider"/> class.
        /// </summary>
        /// <param name="salt">The salt.</param>
        public SecureTextEncryptionProvider(string salt)
        {
            this.salt = salt;
        }

        /// <summary>
        /// Encrypts the given text.
        /// </summary>
        public string Encrypt(string plainText)
        {
            return SecureText.Encrypt(plainText, salt);
        }

        /// <summary>
        /// Decrypts the given encrypted text.
        /// </summary>
        public string Decrypt(string encryptedText)
        {
            return SecureText.Decrypt(encryptedText, salt);
        }

        /// <summary>
        /// Encrypts the given stream.
        /// </summary>
        public void Encrypt(Stream sourceStream, Stream outputStream)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Decrypts the given stream.
        /// </summary>
        public void Decrypt(Stream sourceStream, Stream outputStream)
        {
            throw new NotImplementedException();
        }
    }
}
