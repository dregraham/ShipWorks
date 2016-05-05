using Interapptive.Shared.Security;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data;
using ShipWorks.Data.Administration;
using System;
using ShipWorks.Stores.Platforms.Sears;

namespace ShipWorks.ApplicationCore.Security
{
    class EncryptionProviderFactory : IEncryptionProviderFactory
    {
        private readonly IDatabaseIdentifier databaseIdentifier;
        private readonly ISqlSchemaVersion sqlSchemaVersion;

        /// <summary>
        /// Initializes a new instance of the <see cref="EncryptionProviderFactory" /> class.
        /// </summary>
        /// <param name="databaseIdentifier">The database identifier.</param>
        /// <param name="sqlSchemaVersion">The SQL schema version.</param>
        public EncryptionProviderFactory(IDatabaseIdentifier databaseIdentifier, ISqlSchemaVersion sqlSchemaVersion)
        {
            this.databaseIdentifier = databaseIdentifier;
            this.sqlSchemaVersion = sqlSchemaVersion;
        }

        /// <summary>
        /// Creates the license encryption provider.
        /// </summary>
        /// <returns>An instance of LicenseEncryptionProvider.</returns>
        public IEncryptionProvider CreateLicenseEncryptionProvider()
        {
            bool isLegacy = false;
            
            Version installedSqlSchemaVersion = sqlSchemaVersion.GetInstalledSchemaVersion();
            if (installedSqlSchemaVersion < Version.Parse("4.8.0.0"))
            {
                isLegacy = true;
            }
            
            LicenseCipherKey cipherKey = new LicenseCipherKey(databaseIdentifier);
            return new LicenseEncryptionProvider(cipherKey, isLegacy);
        }

        /// <summary>
        /// Creates the Sears encryption provider.
        /// </summary>
        /// <returns>An instance of AesEncryptionProvider.</returns>
        public IEncryptionProvider CreateSearsEncryptionProvider()
        {
            return new AesEncryptionProvider(new SearsCipherKey());
        }

        /// <summary>
        /// Creates the secure text encryption provider.
        /// </summary>
        /// <param name="salt">The salt.</param>
        /// <returns>An instance of SecureTextEncryptionProvider.</returns>
        public IEncryptionProvider CreateSecureTextEncryptionProvider(string salt)
        {
            return new SecureTextEncryptionProvider(salt);
        }
    }
}