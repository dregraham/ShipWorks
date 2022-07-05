using System;
using System.Linq;
using System.Text;
using log4net;
using Org.BouncyCastle.Security;

namespace Interapptive.Shared.Security.SecureTextVersions
{
    /// <summary>
    /// Class encrypting/decrypting with SecureText version 2
    /// </summary>
    public class SecureTextVersion2 : SecureTextBase
    {
        // Encryption Parameters
        private const int NonceLength = 12;
        private const int TagLength = 16;
        private const int SaltLength = 16;
        private const int EncryptedKeyLength = 60;
        private const int AESKeyLength = 32;

        protected override string Version => "2";

        private readonly byte[] StaticKey = new byte[] { 43, 12, 237, 140, 82, 171, 200, 27, 146, 55, 145, 83, 51, 85, 72, 27, 80, 117, 81, 128, 5, 151, 217, 150, 110, 86, 119, 49, 44, 140, 121, 212 };

        /// <summary>
        /// Constructor
        /// </summary>
        public SecureTextVersion2(ILog log) : base(log)
        {
        }

        /// <summary>
        /// Decrypt a string encrypted with version 1 of SecureText
        /// </summary>
        public override string Decrypt(string ciphertext, string password)
        {
            var encryptedBytes = Convert.FromBase64String(ciphertext);
            var encryptedKey = encryptedBytes.Take(EncryptedKeyLength).ToArray();
            var encryptedText = encryptedBytes
                .Skip(EncryptedKeyLength)
                .Take(encryptedBytes.Length - EncryptedKeyLength - SaltLength)
                .ToArray();
            var salt = encryptedBytes.Skip(EncryptedKeyLength + encryptedText.Length).Take(SaltLength).ToArray();

            return Decrypt(encryptedText, encryptedKey);

        }

        /// <summary>
        /// Perform decryption
        /// </summary>
        private string Decrypt(byte[] encryptedText, byte[] encryptedKey)
        {
            // Decrypt the aesKey with the derived key
            var decryptedKey = DecryptWithAesGcm(encryptedKey, StaticKey, NonceLength, TagLength);

            // Decrypt the encrypted text with the decrypted aesKey
            var plaintext = DecryptWithAesGcm(encryptedText, decryptedKey, NonceLength, TagLength);

            var decryptedText = Encoding.UTF8.GetString(plaintext);

            log.Debug("Finished decrypting");

            return decryptedText;
        }

        /// <summary>
        /// Encrypt a plain text string with Version 2 of SecureText
        /// </summary>
        public override string EncryptInternal(string plaintext, string password)
        {
            // Use a random salt each time
            var salt = new byte[SaltLength];
            new SecureRandom().NextBytes(salt);

            // Generate a random key that will be used to encrypt the plaintext
            var aesKey = new byte[AESKeyLength];
            new SecureRandom().NextBytes(aesKey);

            // Encrypt the aesKey with the StaticKey in order to save it with the encrypted text
            var encryptedKey = EncryptWithAesGcm(aesKey, StaticKey, NonceLength, TagLength);

            // Encrypt the plaintext with the randomly generated aesKey
            var encryptedText = EncryptWithAesGcm(Encoding.UTF8.GetBytes(plaintext), aesKey, NonceLength, TagLength);

            var encryptedBytes = new byte[encryptedKey.Length + encryptedText.Length + salt.Length];

            // The first 60 bytes are the encrypted key (which consists of a nonce, the encrypted key, and a tag)
            Buffer.BlockCopy(encryptedKey, 0, encryptedBytes, 0, encryptedKey.Length);

            // The next X bytes are the encrypted text (which consists of a nonce, the encrypted text, and a tag)
            Buffer.BlockCopy(encryptedText, 0, encryptedBytes, encryptedKey.Length, encryptedText.Length);

            // The last 16 bytes are the salt
            Buffer.BlockCopy(salt, 0, encryptedBytes, encryptedKey.Length + encryptedText.Length, salt.Length);

            return Convert.ToBase64String(encryptedBytes);
        }
    }
}
