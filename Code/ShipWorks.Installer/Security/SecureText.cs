using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using log4net;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace ShipWorks.Installer.Security
{
    /// <summary>
    /// Small utility to for decrypting \ encrypting text that will be saved locally.
    /// </summary>
    public static class SecureText
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(SecureText));

        // Encryption Parameters
        private const int NonceLength = 12;
        private const int TagLength = 16;
        private const int SaltLength = 16;
        private const int EncryptedKeyLength = 60;
        private const int AESKeyLength = 32;

        // SCrypt Parameters
        private const int SCryptIterations = 32768; // Should take less than 100ms on modern PCs
        private const int SCryptBlockSize = 8;
        private const int SCryptParallelismFactor = 1;

        /// <summary>
        /// Decrypts a string that was returned by the Encrypt method.
        /// </summary>
        public static string Decrypt(string ciphertext, string password)
        {
            if (ciphertext == null)
            {
                throw new ArgumentNullException("cipher");
            }

            if (password == null)
            {
                throw new ArgumentNullException("salt");
            }

            if (ciphertext.Length == 0)
            {
                return string.Empty;
            }

            try
            {
                var encryptedBytes = Convert.FromBase64String(ciphertext);

                var encryptedKey = encryptedBytes.Take(EncryptedKeyLength).ToArray();
                var encryptedText = encryptedBytes.Skip(EncryptedKeyLength).Take(encryptedBytes.Length - EncryptedKeyLength - SaltLength).ToArray();
                var salt = encryptedBytes.Skip(EncryptedKeyLength + encryptedText.Length).Take(SaltLength).ToArray();

                // Derive the key used to encrypt the aesKey using the salt and the password
                var derivedKey = SCrypt.Generate(Encoding.UTF8.GetBytes(password), salt, SCryptIterations, SCryptBlockSize, SCryptParallelismFactor, AESKeyLength);

                // Decrypt the aesKey with the derived key
                var decryptedKey = DecryptWithAesGcm(encryptedKey, derivedKey);

                // Decrypt the encrypted text with the decrypted aesKey
                var plaintext = DecryptWithAesGcm(encryptedText, decryptedKey);

                return Encoding.UTF8.GetString(plaintext);
            }
            // OverflowException is thrown when the encrypted text is too short
            catch (Exception e) when (e is OverflowException || e is InvalidCipherTextException)
            {
                log.Warn($"Failed to decrypt with AES-GCM: {e.Message}, trying RC2");

                try
                {
                    return DecryptWithRC2(ciphertext, password);
                }
                catch (Exception ex)
                {
                    if (ex is FormatException || ex is CryptographicException)
                    {
                        log.ErrorFormat("Failed to decrypt '{0}'.", ciphertext);
                        return string.Empty;
                    }

                    throw;
                }
            }
        }

        /// <summary>
        /// Decrypt with AES-GCM
        /// </summary>
        private static byte[] DecryptWithAesGcm(byte[] encryptedBytes, byte[] key)
        {
            var nonce = encryptedBytes.Take(NonceLength).ToArray();
            var ciphertext = encryptedBytes.Skip(NonceLength).Take(encryptedBytes.Length - NonceLength).ToArray();

            var plaintext = new byte[ciphertext.Length - TagLength];

            var cipher = new GcmBlockCipher(new AesEngine());
            var parameters = new AeadParameters(new KeyParameter(key), TagLength * 8, nonce);
            cipher.Init(false, parameters);

            var offset = cipher.ProcessBytes(ciphertext, 0, ciphertext.Length, plaintext, 0);
            cipher.DoFinal(plaintext, offset);

            return plaintext;
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
        /// Encrypts the string and returns the cipher text.
        /// </summary>
        public static string Encrypt(string plaintext, string password)
        {
            if (plaintext == null)
            {
                throw new ArgumentNullException("plaintext");
            }

            if (password == null)
            {
                throw new ArgumentNullException("salt");
            }

            // Use a random salt each time
            var salt = new byte[SaltLength];
            new SecureRandom().NextBytes(salt);

            // Derive a key from the salt and password that will be used to encrypt the aesKey
            var derivedKey = SCrypt.Generate(Encoding.UTF8.GetBytes(password), salt, SCryptIterations, SCryptBlockSize, SCryptParallelismFactor, AESKeyLength);

            // Generate a random key that will be used to encrypt the plaintext
            var aesKey = new byte[AESKeyLength];
            new SecureRandom().NextBytes(aesKey);

            // Encrypt the aesKey with the derivedKey in order to save it with the encrypted text
            var encryptedKey = EncryptWithAesGcm(aesKey, derivedKey);

            // Encrypt the plaintext with the randomly generated aesKey
            var encryptedText = EncryptWithAesGcm(Encoding.UTF8.GetBytes(plaintext), aesKey);

            var encryptedBytes = new byte[encryptedKey.Length + encryptedText.Length + salt.Length];

            // The first 60 bytes are the encrypted key (which consists of a nonce, the encrypted key, and a tag)
            Buffer.BlockCopy(encryptedKey, 0, encryptedBytes, 0, encryptedKey.Length);

            // The next X bytes are the encrypted text (which consists of a nonce, the encrypted text, and a tag)
            Buffer.BlockCopy(encryptedText, 0, encryptedBytes, encryptedKey.Length, encryptedText.Length);

            // The last 16 bytes are the salt
            Buffer.BlockCopy(salt, 0, encryptedBytes, encryptedKey.Length + encryptedText.Length, salt.Length);

            return Convert.ToBase64String(encryptedBytes);
        }

        /// <summary>
        /// Encrypt using AES-GCM
        /// </summary>
        private static byte[] EncryptWithAesGcm(byte[] plaintext, byte[] key)
        {
            var nonce = new byte[NonceLength];
            new SecureRandom().NextBytes(nonce); // We can randomly generate a nonce since we use a new key each time

            byte[] ciphertext = new byte[plaintext.Length + TagLength];

            var cipher = new GcmBlockCipher(new AesEngine());
            var parameters = new AeadParameters(new KeyParameter(key), TagLength * 8, nonce);
            cipher.Init(true, parameters);

            var offset = cipher.ProcessBytes(plaintext, 0, plaintext.Length, ciphertext, 0);
            cipher.DoFinal(ciphertext, offset);

            byte[] encryptedBytes = new byte[nonce.Length + ciphertext.Length];

            // The first 12 bytes are the nonce (the max allowed nonce size in the AES-GCM spec)
            Buffer.BlockCopy(nonce, 0, encryptedBytes, 0, nonce.Length);

            // The next X bytes are the encrypted text and tag
            Buffer.BlockCopy(ciphertext, 0, encryptedBytes, nonce.Length, ciphertext.Length);

            return encryptedBytes;
        }
    }
}
