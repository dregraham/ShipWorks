using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using log4net;

namespace Interapptive.Shared.Security.SecureTextVersions
{
    /// <summary>
    /// Class for decrypting strings encrypted with SecureText version 0
    /// </summary>
    public class SecureTextVersion0 : SecureTextBase
    {
        // Encryption Parameters
        private const int NonceLength = 12;
        private const int TagLength = 16;
        private const int SaltLength = 16;
        private const int EncryptedKeyLength = 60;
        private const int AESKeyLength = 32;

        // SCrypt Parameters
        private const int SCryptIterations = 32768;
        private const int SCryptBlockSize = 8;
        private const int SCryptParallelismFactor = 1;

        /// <summary>
        /// Constructor
        /// </summary>
        public SecureTextVersion0(ILog log) : base(log)
        {
        }

        /// <summary>
        /// Decrypt a string encrypted with version 0 of SecureText
        /// </summary>
        public override string Decrypt(string ciphertext, string password)
        {
            try
            {
                log.Info("Decrypting with RC2");
                var decrypted = DecryptWithRC2(ciphertext, password);
                log.Info("Decrypting with RC2 succeeded");
                return decrypted;
            }
            catch (Exception ex) when (ex is CryptographicException || ex is FormatException)
            {
                log.Info("Decrypting with RC2 failed. Trying AES-GCM");

                log.Info("Converting cipher text from base64");
                var encryptedBytes = Convert.FromBase64String(ciphertext);

                log.Info("Getting encrypted key");
                var encryptedKey = encryptedBytes.Take(EncryptedKeyLength).ToArray();

                log.Info("Getting encrypted text");
                var encryptedText = encryptedBytes
                    .Skip(EncryptedKeyLength)
                    .Take(encryptedBytes.Length - EncryptedKeyLength - SaltLength)
                    .ToArray();

                log.Info("Getting salt");
                var salt = encryptedBytes.Skip(EncryptedKeyLength + encryptedText.Length).Take(SaltLength).ToArray();

                // Derive the key used to encrypt the aesKey using the salt and the password
                var derivedKey = GetKey(
                    password,
                    salt,
                    SCryptIterations,
                    SCryptBlockSize,
                    SCryptParallelismFactor,
                    AESKeyLength);

                // Decrypt the aesKey with the derived key
                log.Info("Decrypting key");
                var decryptedKey = DecryptWithAesGcm(encryptedKey, derivedKey, NonceLength, TagLength);
                log.Info("Finished decrypting key");

                // Decrypt the encrypted text with the decrypted aesKey
                log.Info("Decrypting text");
                var plaintext = DecryptWithAesGcm(encryptedText, decryptedKey, NonceLength, TagLength);
                log.Info("Finished decrypting text");

                var decryptedText = Encoding.UTF8.GetString(plaintext);

                log.Info("Finished building decrypted text string");

                return decryptedText;
            }
        }

        /// <summary>
        /// Decrypt using our old RC2 implementation
        /// </summary>
        private static string DecryptWithRC2(string cipher, string salt)
        {
            RC2CryptoServiceProvider crypto = new RC2CryptoServiceProvider();

            // Create IV from salt
            byte[] saltTotal = Encoding.UTF8.GetBytes(salt + salt.Length.ToString());
            byte[] iv = new byte[8];
            for (int i = 0; i < 8 && i < saltTotal.Length; i++)
            {
                iv[i] = saltTotal[i];
            }

            // Derive the key
            crypto.IV = iv;
            crypto.Key = new PasswordDeriveBytes(salt, new byte[0]).CryptDeriveKey("RC2", "MD5", 56, crypto.IV);

            byte[] encryptedBytes = Convert.FromBase64String(cipher);
            byte[] plainBytes = new Byte[1];

            MemoryStream plain = new MemoryStream();
            using (CryptoStream decoder = new CryptoStream(
                       plain,
                       crypto.CreateDecryptor(),
                       CryptoStreamMode.Write))
            {
                decoder.Write(encryptedBytes, 0, encryptedBytes.Length);
                decoder.FlushFinalBlock();

                plainBytes = new byte[plain.Length];
                plain.Position = 0;
                plain.Read(plainBytes, 0, (int) plain.Length);
            }

            return Encoding.UTF8.GetString(plainBytes);
        }

        /// <summary>
        /// Encrypt a plain text string with Version 0 of SecureText
        /// </summary>
        public override string Encrypt(string plaintext, string password)
        {
            throw new NotImplementedException("SecureTextVersion0 is outdated and should only be used for decryption");
        }
    }
}
