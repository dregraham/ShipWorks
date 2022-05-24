using System;
using System.Linq;
using System.Text;
using log4net;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

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

        protected override string KeyCachePrefix => "V1";

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
        public override string Encrypt(string plaintext, string password)
        {
            // Use a random salt each time
            var salt = new byte[SecureTextVersion1.SaltLength];
            new SecureRandom().NextBytes(salt);

            // Derive a key from the salt and password that will be used to encrypt the aesKey
            var derivedKeySaltPair = GetKeySaltPair(
                password,
                salt,
                SecureTextVersion1.SCryptIterations,
                SecureTextVersion1.SCryptBlockSize,
                SecureTextVersion1.SCryptParallelismFactor,
                SecureTextVersion1.AESKeyLength);

            // Make sure we use the values from the cache if we have them
            salt = derivedKeySaltPair.salt;
            var derivedKey = derivedKeySaltPair.key;

            // Generate a random key that will be used to encrypt the plaintext
            var aesKey = new byte[SecureTextVersion1.AESKeyLength];
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

            var fullyEncrypted = Convert.ToBase64String(encryptedBytes);

            // Add the version to the encrypted string
            fullyEncrypted += ":1";

            log.Debug("Finished encrypting");

            return fullyEncrypted;
        }

        /// <summary>
        /// Encrypt using AES-GCM
        /// </summary>
        private static byte[] EncryptWithAesGcm(byte[] plaintext, byte[] key)
        {
            var nonce = new byte[SecureTextVersion1.NonceLength];
            new SecureRandom().NextBytes(nonce); // We can randomly generate a nonce since we use a new key each time

            byte[] ciphertext = new byte[plaintext.Length + SecureTextVersion1.TagLength];

            var cipher = new GcmBlockCipher(new AesEngine());
            var parameters = new AeadParameters(new KeyParameter(key), SecureTextVersion1.TagLength * 8, nonce);
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
