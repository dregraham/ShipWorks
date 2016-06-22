using System.IO;

namespace Interapptive.Shared.Security
{
    /// <summary>
    /// Interface for encrypting and decrypting text
    /// </summary>
    public interface IEncryptionProvider
    {
        /// <summary>
        /// Encrypts the given decrypted text.
        /// </summary>
        string Encrypt(string plainText);

        /// <summary>
        /// Decrypts the given encrypted text.
        /// </summary>
        string Decrypt(string encryptedText);

        /// <summary>
        /// Encrypts the given decrypted stream.
        /// </summary>
        void Encrypt(Stream sourceStream, Stream outputStream);

        /// <summary>
        /// Decrypts the given encrypted stream.
        /// </summary>
        void Decrypt(Stream sourceStream, Stream outputStream);
    }
}