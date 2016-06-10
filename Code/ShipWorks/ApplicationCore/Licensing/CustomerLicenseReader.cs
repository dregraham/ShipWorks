using Interapptive.Shared.Security;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using System.Security.Cryptography;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Reads the license information from the database
    /// </summary>
    public class CustomerLicenseReader : ICustomerLicenseReader
    {
        private readonly IEncryptionProviderFactory encryptionProviderFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public CustomerLicenseReader(IEncryptionProviderFactory encryptionProviderFactory)
        {
            this.encryptionProviderFactory = encryptionProviderFactory;
        }

        /// <summary>
        /// Reads this License
        /// </summary>
        public string Read()
        {
            IEncryptionProvider encryptionProvider = encryptionProviderFactory.CreateLicenseEncryptionProvider();
            ConfigurationEntity config = ConfigurationData.Fetch();

            try
            {
                string customerKey = encryptionProvider.Decrypt(config.CustomerKey);
                return customerKey;
            }
            catch (EncryptionException ex) when (ex.InnerException is CryptographicException)
            {
                // Crashed because the config data is old, we just have restored
                // databases, refresh the config data and try again.
                ConfigurationData.CheckForChangesNeeded();
                config = ConfigurationData.Fetch();

                return encryptionProvider.Decrypt(config.CustomerKey);
            }
        }
    }
}
