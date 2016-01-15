using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using System;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Stores the license information to the database
    /// </summary>
    public class CustomerLicenseWriter : ICustomerLicenseWriter
    {
        /// <summary>
        /// Writes the customer license to the database
        /// </summary>
        public void Write(ICustomerLicense customerLicense)
        {
            // Initialize Configuration data as it does not exist 
            // when we need to write the key to the database
            ConfigurationData.InitializeForCurrentDatabase();

            ConfigurationEntity config = ConfigurationData.Fetch();
            config.CustomerKey = customerLicense.Key;

            // Save the key to the ConfiguartionEntity 
            ConfigurationData.Save(config);
        }
    }
}