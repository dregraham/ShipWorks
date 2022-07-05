using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace Interapptive.Shared.Security.SecureTextVersions
{
    /// <summary>
    /// Base class to hold common functionality between different SecureText versions
    /// </summary>
    public abstract class SecureTextBase
    {
        protected abstract string Version { get; }

        /// <summary>
        /// Prefix to make the keys in the cache unique between versions
        /// </summary>
        private string KeyCachePrefix => $"V{Version}";

        protected static Dictionary<string, (byte[] key, byte[] salt)> keyCache = new Dictionary<string, (byte[] key, byte[] salt)>();

        protected readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public SecureTextBase(ILog log)
        {
            this.log = log;
        }

        /// <summary>
        /// Decrypt the given ciphertext with the given password
        /// </summary>
        public abstract string Decrypt(string ciphertext, string password);

        /// <summary>
        /// Encrypts the given string with the given password
        /// </summary>
        public abstract string EncryptInternal(string plaintext, string password);

        /// <summary>
        /// Encrypts the given string with the given password
        /// </summary>
        public string Encrypt(string plaintext, string password)
        {
            var encrypted = EncryptInternal(plaintext, password);

            // Add the version to the encrypted string
            encrypted += $":{Version}";

            log.Debug("Finished encrypting");

            return encrypted;
        }

        /// <summary>
        /// Decrypt with AES-GCM
        /// </summary>
        protected byte[] DecryptWithAesGcm(byte[] encryptedBytes, byte[] key, int nonceLength, int tagLength)
        {
            var nonce = encryptedBytes.Take(nonceLength).ToArray();
            var ciphertext = encryptedBytes.Skip(nonceLength).Take(encryptedBytes.Length - nonceLength).ToArray();

            var plaintext = new byte[ciphertext.Length - tagLength];

            var cipher = new GcmBlockCipher(new AesEngine());
            var parameters = new AeadParameters(new KeyParameter(key), tagLength * 8, nonce);
            cipher.Init(false, parameters);

            var offset = cipher.ProcessBytes(ciphertext, 0, ciphertext.Length, plaintext, 0);
            cipher.DoFinal(plaintext, offset);

            return plaintext;
        }

        /// <summary>
        /// Encrypt using AES-GCM
        /// </summary>
        protected byte[] EncryptWithAesGcm(byte[] plaintext, byte[] key, int nonceLength, int tagLength)
        {
            var nonce = new byte[nonceLength];
            new SecureRandom().NextBytes(nonce); // We can randomly generate a nonce since we use a new key each time

            byte[] ciphertext = new byte[plaintext.Length + tagLength];

            var cipher = new GcmBlockCipher(new AesEngine());
            var parameters = new AeadParameters(new KeyParameter(key), tagLength * 8, nonce);
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

        /// <summary>
        /// Get a derived key and salt from the cache, or if it's not there, derive it and add it
        /// </summary>
        protected (byte[] key, byte[] salt) GetKeySaltPair(string password, byte[] salt, int iterations, int blockSize, int parallelismFactor, int keyLength, bool bypassCache = false)
        {
            byte[] derivedKey;

            if (bypassCache)
            {
                derivedKey = DeriveKey(password, salt, iterations, blockSize, parallelismFactor, keyLength);
                return (derivedKey, salt);
            }

            var cacheIndex = $"{KeyCachePrefix}-{password}";

            (byte[] key, byte[] salt) cachedKey;

            if (keyCache.TryGetValue(cacheIndex, out cachedKey))
            {
                log.Debug("Found key in cache");
                return cachedKey;
            }

            derivedKey = DeriveKey(password, salt, iterations, blockSize, parallelismFactor, keyLength);

            keyCache.Add(cacheIndex, (derivedKey, salt));

            return (derivedKey, salt);
        }

        /// <summary>
        /// Clear the key cache
        /// </summary>
        public void ClearCache()
        {
            keyCache = new Dictionary<string, (byte[] key, byte[] salt)>();
        }

        /// <summary>
        /// Derive a key from a password
        /// </summary>
        protected byte[] DeriveKey(string password, byte[] salt, int iterations, int blockSize, int parallelismFactor, int keyLength)
        {
            // Derive a key from the salt and password that will be used to encrypt the aesKey
            log.Debug("Deriving key");
            var derivedKey = SCrypt.Generate(Encoding.UTF8.GetBytes(password), salt, iterations, blockSize, parallelismFactor, keyLength);
            log.Debug("Finished deriving key");

            return derivedKey;
        }
    }
}
