using System;
using Interapptive.Shared.Security.SecureTextVersions;
using log4net;
using Org.BouncyCastle.Crypto;

namespace Interapptive.Shared.Security
{
    /// <summary>
    /// Small utility to for decrypting \ encrypting text that will be saved locally.
    /// </summary>
    public static class SecureText
    {
        // Logger
        private static readonly ILog log = LogManager.GetLogger(typeof(SecureText));

        /// <summary>
        /// Decrypts a string that was returned by the Encrypt method.
        /// </summary>
        public static string Decrypt(string ciphertext) => Decrypt(ciphertext, null);

        /// <summary>
        /// Decrypts a string that was returned by the Encrypt method.
        /// </summary>
        public static string Decrypt(string ciphertext, string password)
        {
            log.Debug("Beginning decryption");

            if (ciphertext == null)
            {
                log.Info("Cipher text was null");
                throw new ArgumentNullException("cipher");
            }

            if (ciphertext.Length == 0)
            {
                log.Info("Cipher text was empty. Returning empty string");
                return string.Empty;
            }

            try
            {
                var encryptedVersion = ciphertext.Split(':');

                if (encryptedVersion.Length == 1)
                {
                    // If there is no version, use version 0
                    log.Debug("Decrypting with SecureText version 0");
                    return new SecureTextVersion0(log).Decrypt(ciphertext, password);
                }

                switch (encryptedVersion[1])
                {
                    case "1":
                        log.Debug("Decrypting with SecureText version 1");
                        return new SecureTextVersion1(log).Decrypt(encryptedVersion[0], password);
                    case "2":
                        log.Debug("Decrypting with SecureText version 2");
                        return new SecureTextVersion2(log).Decrypt(encryptedVersion[0], password);
                    default:
                        throw new InvalidCipherTextException($"Unknown SecureText version: '{encryptedVersion[1]}'");
                }
            }
            // OverflowException is thrown when the encrypted text is too short
            catch (Exception ex) when (ex is OverflowException || ex is InvalidCipherTextException)
            {
                log.Error($"Failed to decrypt '{ciphertext}'.");
                return string.Empty;
            }
        }

        /// <summary>
        /// Encrypts the string and returns the cipher text.
        /// </summary>
        public static string Encrypt(string plaintext) => Encrypt(plaintext, null);

        /// <summary>
        /// Encrypts the string and returns the cipher text.
        /// </summary>
        public static string Encrypt(string plaintext, string password)
        {
            log.Debug("Beginning encryption");

            if (plaintext == null)
            {
                throw new ArgumentNullException("plaintext");
            }

            // This should always be updated to the latest version
            return new SecureTextVersion2(log).Encrypt(plaintext, password);
        }

        /// <summary>
        /// Clear the key cache
        /// We can use any versioned secure text class because
        /// the cache is in the base class
        /// </summary>
        public static void ClearCache()
        {
            new SecureTextVersion0(log).ClearCache();
        }
    }
}
