using System;
using System.Security.Cryptography;
using Interapptive.Shared.Security;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Reads the license information from the database
    /// </summary>
    public class CustomerLicenseReader : ICustomerLicenseReader
    {
        private readonly IConfigurationData configurationData;
        private readonly IDatabaseIdentifier databaseIdentifier;
        private readonly IEncryptionProviderFactory encryptionProviderFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public CustomerLicenseReader(IEncryptionProviderFactory encryptionProviderFactory,
            IConfigurationData configurationData,
            IDatabaseIdentifier databaseIdentifier)
        {
            this.configurationData = configurationData;
            this.databaseIdentifier = databaseIdentifier;
            this.encryptionProviderFactory = encryptionProviderFactory;
        }

        /// <summary>
        /// Reads this License
        /// </summary>
        public string Read()
        {
            IEncryptionProvider encryptionProvider = encryptionProviderFactory.CreateLicenseEncryptionProvider();
            IConfigurationEntity config = configurationData.FetchReadOnly();

            // try to decrypt the key, if we get an ExcryptionException try setting the cached database identifier
            // and try again. This happens when switching between databases or restoring a database with a different database identifier
            try
            {
                return encryptionProvider.Decrypt(config.CustomerKey);
            }
            catch (EncryptionException)
            {
                databaseIdentifier.Reset();
                return encryptionProvider.Decrypt(config.CustomerKey);
            }
        }
    }
}
