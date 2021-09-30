using System;
using System.Buffers.Binary;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using log4net;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using RestSharp;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing.Warehouse;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.Warehouse.Encryption
{
    /// <summary>
    /// Service for encrypting/decrypting using a key from AWS KMS
    /// </summary>
    [Component]
    public class WarehouseEncryptionService : IWarehouseEncryptionService
    {
        private readonly IWarehouseRequestClient warehouseRequestClient;
        private readonly ILog log;

        // Encryption Parameters
        private const int NonceLength = 12;
        private const int TagLength = 16;

        /// <summary>
        /// Constructor
        /// </summary>
        public WarehouseEncryptionService(IWarehouseRequestClient warehouseRequestClient, Func<Type, ILog> logFactory)
        {
            this.warehouseRequestClient = warehouseRequestClient;
            log = logFactory(typeof(WarehouseEncryptionService));
        }

        /// <summary>
        /// Encrypt the given text locally, using a key from kms
        /// </summary>
        public async Task<string> Encrypt(string plainText)
        {
            if (InterapptiveOnly.IsInterapptiveUser && !InterapptiveOnly.Registry.GetValue("EncryptWarehouseCredentials", true))
            {
                return plainText;
            }

            GenerateDataKeyResponse keyResponse = await GenerateDataKey().ConfigureAwait(false);

            try
            {
                byte[] key = Convert.FromBase64String(keyResponse.Plaintext);
                byte[] encryptedKey = Convert.FromBase64String(keyResponse.CiphertextBlob);

                byte[] encryptedBytes = EncryptWithAesGcm(Encoding.UTF8.GetBytes(plainText), key, encryptedKey);

                return Convert.ToBase64String(encryptedBytes);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new WarehouseEncryptionException("Failed to encrypt using the warehouse encryption service", ex);
            }
        }

        /// <summary>
        /// Encrypt using AES-GCM
        /// </summary>
        private byte[] EncryptWithAesGcm(byte[] plaintext, byte[] key, byte[] encryptedKey)
        {
            var nonce = new byte[NonceLength];
            new SecureRandom().NextBytes(nonce); // We can randomly generate a nonce since we use a new key each time

            byte[] ciphertext = new byte[plaintext.Length + TagLength];

            var cipher = new GcmBlockCipher(new AesEngine());
            var parameters = new AeadParameters(new KeyParameter(key), TagLength * 8, nonce);
            cipher.Init(true, parameters);

            var offset = cipher.ProcessBytes(plaintext, 0, plaintext.Length, ciphertext, 0);
            cipher.DoFinal(ciphertext, offset);

            Span<byte> encryptedKeyLength = new byte[4];
            BinaryPrimitives.WriteInt32BigEndian(encryptedKeyLength, encryptedKey.Length);

            byte[] encryptedBytes = new byte[4 + encryptedKey.Length + nonce.Length + ciphertext.Length];

            // The first 4 bytes are an int indicating the length of the encrypted key
            Buffer.BlockCopy(encryptedKeyLength.ToArray(), 0, encryptedBytes, 0, 4);

            // The next X bytes are the encrypted key
            Buffer.BlockCopy(encryptedKey, 0, encryptedBytes, 4, encryptedKey.Length);

            // The next 12 bytes are the nonce (the max allowed nonce size in the AES-GCM spec)
            Buffer.BlockCopy(nonce, 0, encryptedBytes, 4 + encryptedKey.Length, nonce.Length);

            // The next X bytes are the encrypted text and tag
            Buffer.BlockCopy(ciphertext, 0, encryptedBytes, 4 + encryptedKey.Length + nonce.Length, ciphertext.Length);

            return encryptedBytes;
        }

        /// <summary>
        /// Generates a data key to use for encryption
        /// </summary>
        private async Task<GenerateDataKeyResponse> GenerateDataKey()
        {
            IRestRequest request = new RestRequest(WarehouseEndpoints.GenerateDataKey, Method.POST);

            GenericResult<IRestResponse> response = await warehouseRequestClient
                .MakeRequest(request, "Generate Data Key")
                .ConfigureAwait(false);

            if (response.Failure)
            {
                log.Error(response.Exception);
                throw new WarehouseEncryptionException($"Failed to generate data key.{Environment.NewLine}{Environment.NewLine}{response.Message}", response.Exception);
            }

            GenerateDataKeyResponse dataKeyResponse =
                JsonConvert.DeserializeObject<GenerateDataKeyResponse>(response.Value.Content);

            return dataKeyResponse;
        }
    }
}