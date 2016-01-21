using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Interapptive.Shared.Utility;
using Quartz.Util;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Class for encrypting and decrypting customer license keys. Not necessarily a secure
    /// implementation of AES, because the IV is not random. We mostly wanted to use AES to
    /// move away from the SecureText class.
    /// </summary>
    /// <remarks> We are encrypting the customer license key because for legacy customers,
    /// this field will be empty in the database. Meaning a new customer could fake being
    /// legacy by deleting the value. Since we are encrypting the key, even if it is empty,
    /// it will appear to have a value.
    /// </remarks>
    public class LicenseEncryptionProvider : IEncryptionProvider
    {
        private const string LegacyUserLicense = "ShipWorks legacy user";
        private readonly AesManaged aes;

        /// <summary>
        /// Initializes a new instance of the <see cref="LicenseEncryptionProvider"/> class.
        /// </summary>
        /// <param name="databaseID">The database identifier.</param>
        public LicenseEncryptionProvider(IDatabaseIdentifier databaseID)
        {
            aes = new AesManaged()
            {
                Key = databaseID.Get().ToByteArray(),
                IV = new byte[] { 125, 42, 69, 178, 253, 78, 1, 17, 77, 56, 129, 11, 25, 225, 201, 14 }
            };
        }

        /// <summary>
        /// Encrypts the given plain text.
        /// </summary>
        public string Encrypt(string plainText)
        {
            if (plainText.IsNullOrWhiteSpace())
            {
                plainText = LegacyUserLicense;
            }

            return GetEncryptedString(plainText);
        }

        /// <summary>
        /// Decrypts the given encrypted text.
        /// </summary>
        /// <exception cref="ShipWorksLicenseException"></exception>
        public string Decrypt(string encryptedText)
        {
            if (encryptedText.IsNullOrWhiteSpace())
            {
                throw new ShipWorksLicenseException("Cannot decrypt an empty string");
            }

            string decryptedText = GetDecryptedString(encryptedText);

            if (decryptedText.Equals(LegacyUserLicense))
            {
                decryptedText = "";
            }

            return decryptedText;
        }

        /// <summary>
        /// Gets the encrypted string.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        private string GetEncryptedString(string plainText)
        {
            try
            {
                ICryptoTransform encryptor = aes.CreateEncryptor();

                byte[] buffer = Encoding.ASCII.GetBytes(plainText);

                return Convert.ToBase64String(encryptor.TransformFinalBlock(buffer, 0, buffer.Length));
            }
            catch (Exception ex)
            {
                throw new EncryptionException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Gets the decrypted string.
        /// </summary>
        /// <param name="encryptedText">The encrypted text.</param>
        private string GetDecryptedString(string encryptedText)
        {
            try
            {
                ICryptoTransform decryptor = aes.CreateDecryptor();

                byte[] buffer = Convert.FromBase64String(encryptedText);

                return Encoding.ASCII.GetString(decryptor.TransformFinalBlock(buffer, 0, buffer.Length));
            }
            catch (DatabaseIdentifierException ex)
            {
                throw new ShipWorksLicenseException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new EncryptionException(ex.Message, ex);
            }
        }
    }
}