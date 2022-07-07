using System;
using System.Linq;
using System.Text;
using log4net;
using Org.BouncyCastle.Crypto;

namespace Interapptive.Shared.Security.SecureTextVersions
{
    /// <summary>
    /// Class encrypting/decrypting with SecureText version 1
    /// </summary>
    public class SecureTextVersion1 : SecureTextBase
    {
        // Encryption Parameters
        private const int NonceLength = 12;
        private const int TagLength = 16;
        private const int SaltLength = 16;
        private const int EncryptedKeyLength = 60;
        private const int AESKeyLength = 32;

        // SCrypt Parameters
        private const int SCryptIterations = 8192;
        private const int SCryptBlockSize = 8;
        private const int SCryptParallelismFactor = 1;

        protected override string Version => "1";

        /// <summary>
        /// Constructor
        /// </summary>
        public SecureTextVersion1(ILog log) : base(log)
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

            // Derive the key used to encrypt the aesKey using the salt and the password
            var derivedKey = GetKeySaltPair(
                password,
                salt,
                SCryptIterations,
                SCryptBlockSize,
                SCryptParallelismFactor,
                AESKeyLength).key;

            try
            {
                return Decrypt(encryptedText, encryptedKey, derivedKey);
            }
            catch (Exception ex) when (ex is OverflowException || ex is InvalidCipherTextException)
            {
                log.Debug("An error occured decrypting with a cached key. Re-trying without caching.");

                // Derive the key used to encrypt the aesKey using the salt and the password
                derivedKey = GetKeySaltPair(
                    password,
                    salt,
                    SCryptIterations,
                    SCryptBlockSize,
                    SCryptParallelismFactor,
                    AESKeyLength,
                    true).key;

                return Decrypt(encryptedText, encryptedKey, derivedKey);
            }
        }

        /// <summary>
        /// Perform decryption
        /// </summary>
        private string Decrypt(byte[] encryptedText, byte[] encryptedKey, byte[] derivedKey)
        {
            // Decrypt the aesKey with the derived key
            var decryptedKey = DecryptWithAesGcm(encryptedKey, derivedKey, NonceLength, TagLength);

            // Decrypt the encrypted text with the decrypted aesKey
            var plaintext = DecryptWithAesGcm(encryptedText, decryptedKey, NonceLength, TagLength);

            var decryptedText = Encoding.UTF8.GetString(plaintext);

            log.Debug("Finished decrypting");

            return decryptedText;
        }

        /// <summary>
        /// Encrypt a plain text string with Version 1 of SecureText
        /// </summary>
        public override string EncryptInternal(string plaintext, string password)
        {
            throw new NotImplementedException("SecureTextVersion1 is outdated and should only be used for decryption");
        }
    }
}
