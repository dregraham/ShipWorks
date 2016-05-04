using Interapptive.Shared.Security;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data;
using ShipWorks.Data.Administration;
using System;
using System.Data.SqlClient;

namespace ShipWorks.ApplicationCore.Security
{
    class EncryptionProviderFactory : IEncryptionProviderFactory
    {
        private readonly IDatabaseIdentifier databaseIdentifier;
        private readonly ISqlSchemaVersion sqlSchemaVersion;

        public EncryptionProviderFactory(IDatabaseIdentifier databaseIdentifier, ISqlSchemaVersion sqlSchemaVersion)
        {
            this.databaseIdentifier = databaseIdentifier;
            this.sqlSchemaVersion = sqlSchemaVersion;
        }

        public IEncryptionProvider CreateLicenseEncryptionProvider()
        {
            byte[] iv = { 125, 42, 69, 178, 253, 78, 1, 17, 77, 56, 129, 11, 25, 225, 201, 14 };
            byte[] key = null;
            bool isLegacy = false;

            try
            {
                key = databaseIdentifier.Get().ToByteArray();
            }
            catch (DatabaseIdentifierException ex)
            {

                SqlException sqlEx = (SqlException) ex.InnerException?.InnerException;

                Version installedSqlSchemaVersion = sqlSchemaVersion.GetInstalledSchemaVersion();

                // Could not find stored procedure GetDataGuid, we must be in the process
                // of upgrading, or pre 4.8.0.0 schema version
                if (sqlEx?.Number == 2812 && installedSqlSchemaVersion < Version.Parse("4.8.0.0"))
                {
                    isLegacy = true;
                }
                else
                {
                    throw new EncryptionException(ex.Message, ex);
                }
            }


            return new LicenseEncryptionProvider(iv, key, isLegacy);
        }

        public IEncryptionProvider CreateSearsEncryptionProvider()
        {
            byte[] iv = {84, 104, 101, 68, 111, 111, 115, 107, 101, 114, 110, 111, 111, 100, 108, 101};
            byte[] key = new Guid("{A2FC95D9-F255-4D23-B86C-756889A51C6A}").ToByteArray();

            return new AesEncryptionProvider(key, iv);
        }

        public IEncryptionProvider CreateSecureTextEncryptionProvider(string salt) => 
            new SecureTextEncryptionProvider(salt);
    }
}