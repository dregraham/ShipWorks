using Interapptive.Shared.Security;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Administration;
using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace ShipWorks.ApplicationCore.Security
{
    /// <summary>
    /// Encryption Provider for Customer License
    /// </summary>
    public class LicenseEncryptionProvider : AesEncryptionProvider
    {
        private readonly ISqlSchemaVersion sqlSchemaVersion;

        /// <summary>
        /// Initializes a new instance of the <see cref="LicenseEncryptionProvider"/> class.
        /// </summary>
        /// <param name="aesParams">The aes parameters.</param>
        /// <param name="sqlSchemaVersion">The SQL schema version.</param>
        public LicenseEncryptionProvider(IAesParams aesParams, ISqlSchemaVersion sqlSchemaVersion) : base(aesParams)
        {
            this.sqlSchemaVersion = sqlSchemaVersion;
        }

        /// <summary>
        /// Gets the decrypted string.
        /// </summary>
        /// <param name="encryptedText">The encrypted text.</param>
        /// <returns></returns>
        protected override string GetDecryptedString(string encryptedText)
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
