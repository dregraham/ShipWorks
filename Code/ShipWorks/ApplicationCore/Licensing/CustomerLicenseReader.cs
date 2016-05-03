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
        private readonly IEncryptionProvider encryptionProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public CustomerLicenseReader(IEncryptionProvider encryptionProvider)
        {
            this.encryptionProvider = encryptionProvider;
        }

        /// <summary>
        /// Reads this License
        /// </summary>
        public string Read()
        {
            ConfigurationEntity config = ConfigurationData.Fetch();

            try
            {
                return encryptionProvider.Decrypt(config.CustomerKey);
            }
            catch (EncryptionException ex) when(ex.InnerException is CryptographicException)
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
