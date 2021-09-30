﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;

namespace Interapptive.Shared.Security.SecureTextVersions
{
    /// <summary>
    /// Base class to hold common functionality between different SecureText versions
    /// </summary>
    public abstract class SecureTextBase
    {
        /// <summary>
        /// Prefix to make the keys in the cache unique between versions
        /// </summary>
        protected abstract string KeyCachePrefix { get; }

        protected static Dictionary<string, byte[]> keyCache = new Dictionary<string, byte[]>();

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
        public abstract string Encrypt(string plaintext, string password);

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
        /// Get a derived key from the cache, or if it's not there, derive it and add it
        /// </summary>
        protected byte[] GetKey(string password, byte[] salt, int iterations, int blockSize, int parallelismFactor, int keyLength)
        {
            var cacheIndex = $"{KeyCachePrefix}-{password}";

            if (keyCache.TryGetValue(cacheIndex, out byte[] key))
            {
                log.Info("Found key in cache");
                return key;
            }

            // Derive a key from the salt and password that will be used to encrypt the aesKey
            log.Info("Deriving key");
            var derivedKey = SCrypt.Generate(Encoding.UTF8.GetBytes(password), salt, iterations, blockSize, parallelismFactor, keyLength);
            log.Info("Finished deriving key");

            keyCache.Add(cacheIndex, derivedKey);

            return derivedKey;
        }
    }
}
