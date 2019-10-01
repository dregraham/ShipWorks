using System;
using System.Security.Cryptography;
using Interapptive.Shared.Security;
using Interapptive.Shared.Utility;
using log4net;
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
        private readonly IEncryptionProvider encryptionProvider;
        private readonly ILog log;
        private string currentCustomerKey = string.Empty;
        private string decryptedCustomerKey = string.Empty;

        /// <summary>
        /// Constructor
        /// </summary>
        public CustomerLicenseReader(IEncryptionProviderFactory encryptionProviderFactory,
            IConfigurationData configurationData,
            IDatabaseIdentifier databaseIdentifier,
            Func<Type, ILog> createLog)
        {
            this.configurationData = configurationData;
            this.databaseIdentifier = databaseIdentifier;
            encryptionProvider = encryptionProviderFactory.CreateLicenseEncryptionProvider();
            log = createLog(typeof(CustomerLicenseReader));
        }

        /// <summary>
        /// Reads this License
        /// </summary>
        public string Read()
        {
            string customerKey = configurationData.FetchCustomerKey();

            if (!decryptedCustomerKey.IsNullOrWhiteSpace() &&
                customerKey.Equals(currentCustomerKey, StringComparison.InvariantCultureIgnoreCase))
            {
                return decryptedCustomerKey;
            }

            currentCustomerKey = customerKey;

            // try to decrypt the key, if we get an ExcryptionException try setting the cached database identifier
            // and try again. This happens when switching between databases or restoring a database with a different database identifier
            try
            {
                decryptedCustomerKey = encryptionProvider.Decrypt(customerKey);
            }
            catch (EncryptionException ex)
            {
                log.Error("Decryption failed. Trying again", ex);
                databaseIdentifier.Reset();
                decryptedCustomerKey = encryptionProvider.Decrypt(customerKey);
            }

            return decryptedCustomerKey;
        }
    }
}
