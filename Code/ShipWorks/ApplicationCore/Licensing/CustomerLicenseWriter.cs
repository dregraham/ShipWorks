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
            ConfigurationEntity config = null;

            // Fetch the current configuration entity
            // this will crash if this is a brand new database
            try
            {
                config = ConfigurationData.Fetch();
            } catch (NullReferenceException)
            {

            }
            
            // if its null because we are in the database setup wizard
            if (config == null)
            {
                ConfigurationData.InitializeForCurrentDatabase();

                config = ConfigurationData.Fetch();
            }

            config.CustomerKey = customerLicense.Key;

            ConfigurationData.Save(config);
        }
    }
}