using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using log4net;
using Newtonsoft.Json;
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
        private readonly WarehouseRequestClient warehouseRequestClient;
        private readonly ILog log;

        // Encryption Parameters
        private const int BlockBitSize = 128;
        private const int KeyBitSize = 256;

        /// <summary>
        /// Constructor
        /// </summary>
        public WarehouseEncryptionService(WarehouseRequestClient warehouseRequestClient, Func<Type, ILog> logFactory)
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

                byte[] encryptedBytes = EncryptWithAES(Encoding.UTF8.GetBytes(plainText), key, encryptedKey);

                return Convert.ToBase64String(encryptedBytes);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new WarehouseEncryptionException("Failed to encrypt using the warehouse encryption service", ex);
            }
        }

        /// <summary>
        /// Encrypt using AES
        /// </summary>
        private byte[] EncryptWithAES(byte[] plainText, byte[] key, byte[] encryptedKey)
        {
            byte[] encryptedText;
            byte[] iv;

            AesManaged aes = new AesManaged
            {
                KeySize = KeyBitSize,
                BlockSize = BlockBitSize,
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7
            };

            // Use AES to encrypt the plain text
            using (aes)
            {
                // Always use random IV
                aes.GenerateIV();

                // Save IV so we can save it with the encrypted message
                iv = aes.IV;

                using (ICryptoTransform encryptor = aes.CreateEncryptor(key, iv))
                {
                    using (MemoryStream cipherStream = new MemoryStream())
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(cipherStream, encryptor, CryptoStreamMode.Write))
                        {
                            // Encrypt Data
                            cryptoStream.Write(plainText, 0, plainText.Length);
                            cryptoStream.FlushFinalBlock();

                            encryptedText = cipherStream.ToArray();
                        }
                    }
                }
            }

            // Assemble encrypted message
            using (MemoryStream encryptedStream = new MemoryStream())
            {
                // Prepend with encrypted key
                encryptedStream.Write(encryptedKey, 0, encryptedKey.Length);
                // Prepend with IV
                encryptedStream.Write(iv, 0, iv.Length);
                // Write encrypted text
                encryptedStream.Write(encryptedText, 0, encryptedText.Length);

                return encryptedStream.ToArray();
            }
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