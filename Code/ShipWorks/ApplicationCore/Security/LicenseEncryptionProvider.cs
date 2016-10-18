using Interapptive.Shared.Security;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Administration;

namespace ShipWorks.ApplicationCore.Security
{
    /// <summary>
    /// Encryption Provider for Customer License
    /// </summary>
    public class LicenseEncryptionProvider : AesEncryptionProvider
    {
        private readonly ISqlSchemaVersion sqlSchemaVersion;
        private const string EmptyValue = "ShipWorks legacy user";

        /// <summary>
        /// Initializes a new instance of the <see cref="LicenseEncryptionProvider"/> class.
        /// </summary>
        public LicenseEncryptionProvider(ICipherKey cipherKey, ISqlSchemaVersion sqlSchemaVersion)
            : base(cipherKey)
        {
            this.sqlSchemaVersion = sqlSchemaVersion;
        }

        /// <summary>
        /// Gets the decrypted string
        /// </summary>
        /// <remarks>
        /// If legacy or decrypted value is "Shipworks legacy user" empty string is returned.
        /// </remarks>
        public override string Decrypt(string encryptedText)
        {
            try
            {
                string decryptedResult = base.Decrypt(encryptedText);
                return decryptedResult == EmptyValue ? string.Empty : decryptedResult;
            }
            catch (EncryptionException ex) when (ex.InnerException.InnerException is DatabaseIdentifierException)
            {
                // refresh isCustomerLicenseSupported as we may have restored the database to a version
                // that does not support customer licenses yet, the database is in a state where it needs
                // to be upgraded
                // if the database does not yet support customer licenses return an empty string
                if (!sqlSchemaVersion.IsCustomerLicenseSupported())
                {
                    return string.Empty;
                }

                throw;
            }
        }

        /// <summary>
        /// Encrypts the given plain text.
        /// </summary>
        /// <remarks>
        /// If plainTest is empty, we encrypt "ShipWorks legacy user"
        /// </remarks>
        public override string Encrypt(string plainText)
        {
            // AES can not encrypt empty strings. So when we get an empty string, set it to a fixed string,
            // so that when we decrypt later, we know it is actually supposed to be an empty string.
            if (string.IsNullOrWhiteSpace(plainText))
            {
                plainText = EmptyValue;
            }

            return base.Encrypt(plainText);
        }
    }
}
