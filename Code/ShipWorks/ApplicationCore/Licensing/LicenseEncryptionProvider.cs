using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Interapptive.Shared.Utility;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Class for encrypting and decrypting license keys
    /// </summary>
    public class LicenseEncryptionProvider : IEncryptionProvider
    {
        private readonly byte[] key;
        private readonly byte[] iv;
        readonly UTF8Encoding byteTransform = new UTF8Encoding();


        /// <summary>
        /// Initializes a new instance of the <see cref="LicenseEncryptionProvider"/> class.
        /// </summary>
        /// <param name="databaseID">The database identifier.</param>
        public LicenseEncryptionProvider(IDatabaseIdentifier databaseID)
        {
            key = byteTransform.GetBytes(databaseID.Get().ToString());
            iv = CreateIV();
        }

        private byte[] CreateIV()
        {
            byte[] bytes = {125, 42, 69, 178, 253, 78, 1, 17, 77, 56, 129, 11, 25, 225, 201, 14};

            return bytes;
        }

        /// <summary>
        /// Encrypts the given decrypted text.
        /// </summary>
        public string Encrypt(string plainText)
       {
            byte[] encryptedBytes = GetEncryptedBytes(plainText, key, iv);

            return Encoding.UTF8.GetString(encryptedBytes);
        }

        /// <summary>
        /// Decrypts the given encrypted text.
        /// </summary>
        /// <exception cref="ShipWorksLicenseException"></exception>
        public string Decrypt(string encryptedText)
        {
            if (string.IsNullOrWhiteSpace(encryptedText))
            {
                throw new ShipWorksLicenseException("Cannot decrypt an empty string");
            }

            byte[] encryptedBytes = byteTransform.GetBytes(encryptedText);

            return GetDecryptedString(encryptedBytes, key, iv);
        }

        private byte[] GetEncryptedBytes(string plainText, byte[] key, byte[] iv)
        {
            byte[] plainBytes = byteTransform.GetBytes(plainText);

            try
            {
                AesManaged aes = new AesManaged
                {
                    Key = key,
                    IV = iv
                };

                MemoryStream memStream = new MemoryStream();

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                CryptoStream cryptoStream = new CryptoStream(memStream, encryptor, CryptoStreamMode.Write);

                cryptoStream.Write(plainBytes, 0, plainBytes.Length);

                return memStream.ToArray();
            }
            catch (Exception)
            {
                throw new Exception("Encryption error");
            }
        }

        private string GetDecryptedString(byte[] encryptedText, byte[] key, byte[] iv)
        {
            try
            {
                MemoryStream memStream = new MemoryStream(encryptedText);

                AesManaged aes = new AesManaged
                {
                    Key = key,
                    IV = iv
                };

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                CryptoStream cryptoStream = new CryptoStream(memStream, decryptor, CryptoStreamMode.Read);

                StreamReader streamReader = new StreamReader(cryptoStream);

                return streamReader.ReadToEnd();
            }
            catch (DatabaseIdentifierException ex)
            {
                throw new ShipWorksLicenseException(ex.Message, ex);
            }
        }
    }
}