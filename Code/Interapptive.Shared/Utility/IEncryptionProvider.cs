namespace Interapptive.Shared.Utility
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
    }
}