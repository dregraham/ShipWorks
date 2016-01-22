using System;
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
        private readonly IDatabaseIdentifier databaseId;
        private const string LegacyUserLicense = "ShipWorks legacy user";

        /// <summary>
        /// Initializes a new instance of the <see cref="LicenseEncryptionProvider"/> class.
        /// </summary>
        /// <param name="databaseId">The database identifier.</param>
        public LicenseEncryptionProvider(IDatabaseIdentifier databaseId)
        {
            this.databaseId = databaseId;
        }

        /// <summary>
        /// Encrypts the given plain text.
        /// </summary>
        public string Encrypt(string plainText)
        {
            // AES can not encrypt empty strings. So when we get
            // a legacy customer license, set the key to a fixed string,
            // so that when we decrypt later, we know it is actually
            // supposed to be an empty string.
            if (plainText.IsNullOrWhiteSpace())
            {
                plainText = LegacyUserLicense;
            }

            return GetEncryptedString(plainText);
        }

        /// <summary>
        /// Decrypts the given encrypted text.
        /// </summary>
        public string Decrypt(string encryptedText)
        {
            if (encryptedText.IsNullOrWhiteSpace())
            {
                throw new EncryptionException("Cannot decrypt an empty string");
            }

            string decryptedText = GetDecryptedString(encryptedText);

            // If we get the fixed legacy user string, we know it is
            // a legacy customer. So return a blank license key.
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
                ICryptoTransform encryptor = Aes.CreateEncryptor();

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
                ICryptoTransform decryptor = Aes.CreateDecryptor();

                byte[] buffer = Convert.FromBase64String(encryptedText);

                return Encoding.ASCII.GetString(decryptor.TransformFinalBlock(buffer, 0, buffer.Length));
            }
            catch (Exception ex)
            {
                throw new EncryptionException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Get DatabaseId value
        /// </summary>
        private AesManaged Aes
        {
            get
            {
                byte[] key;
                try
                {
                    key = databaseId.Get().ToByteArray();
                }
                catch (DatabaseIdentifierException ex)
                {
                    throw new EncryptionException(ex.Message, ex);
                }

                return new AesManaged()
                {
                    IV = new byte[] {125, 42, 69, 178, 253, 78, 1, 17, 77, 56, 129, 11, 25, 225, 201, 14},
                    Key = key
                };
            }
        }
    }
}