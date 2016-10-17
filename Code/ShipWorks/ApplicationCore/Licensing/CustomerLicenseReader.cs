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
        private readonly IEncryptionProviderFactory encryptionProviderFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public CustomerLicenseReader(IEncryptionProviderFactory encryptionProviderFactory,
            IConfigurationData configurationData)
        {
            this.configurationData = configurationData;
            this.encryptionProviderFactory = encryptionProviderFactory;
        }

        /// <summary>
        /// Reads this License
        /// </summary>
        public string Read()
        {
            IEncryptionProvider encryptionProvider = encryptionProviderFactory.CreateLicenseEncryptionProvider();
            IConfigurationEntity config = configurationData.FetchReadOnly();

                // This exception was rarely thrown by config.CustomerKey, but it did happen. Hopefully this will 
                // keep it from hapenning again... I could also putting some sort of sleep in here (a couple of seconds)
                // This was hapenning when the key and the databaseid didn't match. This would happen when switching 
                // I don't know if it is hapenning anymore...
            return encryptionProvider.Decrypt(config.CustomerKey);
        }
    }
}
