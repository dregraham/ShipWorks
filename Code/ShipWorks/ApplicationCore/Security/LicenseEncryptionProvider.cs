using System;
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
        private const string EmptyValue = "ShipWorks legacy user";
        private readonly bool isCustomerLicenseSupported;

        /// <summary>
        /// Initializes a new instance of the <see cref="LicenseEncryptionProvider"/> class.
        /// </summary>
        public LicenseEncryptionProvider(ICipherKey cipherKey, bool isCustomerLicenseSupported)
            : base(cipherKey)
        {
            this.isCustomerLicenseSupported = isCustomerLicenseSupported;
        }

        /// <summary>
        /// Gets the decrypted string
        /// </summary>
        /// <remarks>
        /// If legacy or decrypted value is "Shipworks legacy user" empty string is returned.
        /// </remarks>
        public override string Decrypt(string encryptedText)
        {
            if (!isCustomerLicenseSupported)
            {
                return string.Empty;
            }

            try
            {
                string decryptedResult = base.Decrypt(encryptedText);
                return decryptedResult == EmptyValue ? string.Empty : decryptedResult;
            }
            catch (EncryptionException ex) when (ex.InnerException.InnerException is DatabaseIdentifierException)
            {
                // If we cannot find the GetDatabaseGuid check to see if an upgrade is required
                // if an upgrade is required then we know that the new database does not have the
                // GetDatabaseGuid because it is of a schema version older than webreg, return empty string
                if (SqlSchemaUpdater.IsUpgradeRequired())
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
            if (!isCustomerLicenseSupported)
            {
                throw new EncryptionException("Can't encrypt a license when isLegacy.");
            }

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
