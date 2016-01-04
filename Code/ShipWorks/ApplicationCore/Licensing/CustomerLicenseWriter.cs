using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Stores the license information to the database
    /// </summary>
    public class CustomerLicenseWriter : ICustomerLicenseWriter
    {
        private readonly IConfigurationDataWrapper configurationData;

        public CustomerLicenseWriter(IConfigurationDataWrapper configurationData)
        {
            this.configurationData = configurationData;
        }

        /// <summary>
        /// Writes the customer license to the database
        /// </summary>
        public void Write(ICustomerLicense customerLicense)
        {
            ConfigurationEntity configuration = configurationData.Fetch();

            configuration.CustomerKey = customerLicense.Key;

            configurationData.Save(configuration);
        }
    }
}