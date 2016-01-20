using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using System;
using Interapptive.Shared.Utility;

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
        /// <exception cref="ShipWorksLicenseException"></exception>
        public string Read()
        {
            ConfigurationData.CheckForChangesNeeded();

            ConfigurationEntity config = ConfigurationData.Fetch();
            return encryptionProvider.Decrypt(config.CustomerKey);
        }
    }
}
