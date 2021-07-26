using System;
using Interapptive.Shared.Security;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Data;

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
        private string currentLegacyCustomerKey = string.Empty;
        private string decryptedLegacyCustomerKey = string.Empty;
        
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
        public string Read(CustomerLicenseKeyType customerLicenseKeyType = CustomerLicenseKeyType.WebReg)
        {
            return customerLicenseKeyType == CustomerLicenseKeyType.WebReg
                ? ReadKey(CustomerLicenseKeyType.WebReg, ref currentCustomerKey, ref decryptedCustomerKey)
                : ReadKey(CustomerLicenseKeyType.Legacy, ref currentLegacyCustomerKey, ref decryptedLegacyCustomerKey);
        }

        /// <summary>
        /// Read the customer license key based on the given type
        /// </summary>
        private string ReadKey(CustomerLicenseKeyType keyType, ref string currentKey, ref string decryptedKey)
        {
            string key = configurationData.FetchCustomerKey(keyType);

            if (!decryptedKey.IsNullOrWhiteSpace() &&
                key.Equals(currentKey, StringComparison.InvariantCultureIgnoreCase))
            {
                return decryptedKey;
            }

            currentKey = key;

            // try to decrypt the key, if we get an EncryptionException try setting the cached database identifier
            // and try again. This happens when switching between databases or restoring a database with a different database identifier
            try
            {
                log.Debug("CustomerLicenseReader decrypting customer key.");
                decryptedKey = encryptionProvider.Decrypt(key);
            }
            catch (EncryptionException ex)
            {
                log.Error("Decryption failed. Trying again", ex);
                databaseIdentifier.Reset();
                decryptedKey = encryptionProvider.Decrypt(key);
            }

            return decryptedKey;
        }
        
    }
}
