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
        public static string Decrypt(string ciphertext, string password)
        {
            log.Debug("Beginning decryption");

            if (ciphertext == null)
            {
                log.Info("Cipher text was null");
                throw new ArgumentNullException("cipher");
            }

            if (password == null)
            {
                log.Info("Password was null");
                throw new ArgumentNullException("salt");
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

                // When new versions are created, this should become a switch statement based on encryptedVersion[1]
                log.Debug("Decrypting with SecureText version 1");
                return new SecureTextVersion1(log).Decrypt(encryptedVersion[0], password);
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
        public static string Encrypt(string plaintext, string password)
        {
            log.Debug("Beginning encryption");

            if (plaintext == null)
            {
                throw new ArgumentNullException("plaintext");
            }

            if (password == null)
            {
                throw new ArgumentNullException("salt");
            }

            return new SecureTextVersion1(log).Encrypt(plaintext, password);
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
