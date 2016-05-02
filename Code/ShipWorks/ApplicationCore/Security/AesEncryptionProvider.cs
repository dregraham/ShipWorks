using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using Interapptive.Shared.Security;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Administration;

namespace ShipWorks.ApplicationCore.Security
{
    public class AesEncryptionProvider : IEncryptionProvider
    {
        private const string LegacyUserLicense = "ShipWorks legacy user";
        private readonly IDatabaseIdentifier databaseId;
        private readonly ISqlSchemaVersion sqlSchemaVersion;

        public AesEncryptionProvider(IDatabaseIdentifier databaseId, ISqlSchemaVersion sqlSchemaVersion, IInitializationVector iv)
        {
            this.databaseId = databaseId;
            this.sqlSchemaVersion = sqlSchemaVersion;
            InitializationVector = iv.Value;
        }

        protected byte[] InitializationVector { get; }

        /// <summary>
        /// Get DatabaseId value
        /// </summary>
        private AesManaged Aes
        {
            get
            {
                try
                {
                    byte[] key = databaseId.Get().ToByteArray();

                    return new AesManaged()
                    {
                        IV = InitializationVector,
                        Key = key
                    };
                }
                catch (DatabaseIdentifierException ex)
                {
                    throw new EncryptionException(ex.Message, ex);
                }
            }
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
            if (string.IsNullOrWhiteSpace(plainText))
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
            if (string.IsNullOrWhiteSpace(encryptedText))
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
                SqlException sqlEx = (SqlException)ex.InnerException?.InnerException;

                Version installedSqlSchemaVersion = sqlSchemaVersion.GetInstalledSchemaVersion();

                // Could not find stored procedure GetDataGuid, we must be in the process
                // of upgrading, or pre 4.8.0.0 schema version
                if (sqlEx?.Number == 2812 && installedSqlSchemaVersion < Version.Parse("4.8.0.0"))
                {
                    return string.Empty;
                }

                throw new EncryptionException(ex.Message, ex);
            }
        }
    }
}