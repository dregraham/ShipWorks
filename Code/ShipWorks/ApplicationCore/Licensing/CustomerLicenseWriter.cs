using Interapptive.Shared.Security;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Stores the license information to the database
    /// </summary>
    public class CustomerLicenseWriter : ICustomerLicenseWriter
    {
        private readonly IEncryptionProvider encryptionProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public CustomerLicenseWriter(IEncryptionProvider encryptionProvider)
        {
            this.encryptionProvider = encryptionProvider;
        }

        /// <summary>
        /// Writes the customer license to the database
        /// </summary>
        public void Write(ICustomerLicense customerLicense)
        {
            // Initialize Configuration data as it does not exist
            // when we need to write the key to the database
            ConfigurationData.InitializeForCurrentDatabase();
            ConfigurationEntity config = ConfigurationData.Fetch();

            config.CustomerKey = encryptionProvider.Encrypt(customerLicense.Key);

            // Save the key to the ConfigurationEntity
            ConfigurationData.Save(config);
        }
    }
}