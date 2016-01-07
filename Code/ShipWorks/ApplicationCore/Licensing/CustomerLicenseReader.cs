using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;

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

            ConfigurationEntity config = ConfigurationData.Fetch();

            return config.CustomerKey;
        }
    }
}
