using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using System;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Reads the license information from the database
    /// </summary>
    public class CustomerLicenseReader : ICustomerLicenseReader
    {
        /// <summary>
        /// Reads this License
        /// </summary>
        /// <returns></returns>
        public string Read()
        {
            ConfigurationData.CheckForChangesNeeded();

            try
            {
                ConfigurationEntity config = ConfigurationData.Fetch();
                return config.CustomerKey;
            }
            catch (NullReferenceException)
            {
                // No database connection exists so we return an empty string
                return string.Empty;
            }
        }
    }
}
