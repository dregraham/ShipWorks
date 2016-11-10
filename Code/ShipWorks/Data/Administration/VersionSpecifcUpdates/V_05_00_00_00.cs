using System;
using ShipWorks.ApplicationCore.Licensing;

namespace ShipWorks.Data.Administration.VersionSpecifcUpdates
{
    /// <summary>
    /// ShipWorks update that should be applied for a specific version
    /// </summary>
    /// <remarks>
    /// update Configuration table Key column to have an encrypted empty string
    /// using the GetDatabaseGuid stored procedure as the salt.
    /// </remarks>
    public class V_05_00_00_00 : IVersionSpecificUpdate
    {
        private readonly Func<string, ICustomerLicense> getCustomerLicense;
        private readonly IConfigurationData configurationData;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="customerLicense"></param>
        public V_05_00_00_00(Func<string, ICustomerLicense> getCustomerLicense, IConfigurationData configurationData)
        {
            this.getCustomerLicense = getCustomerLicense;
            this.configurationData = configurationData;
        }

        /// <summary>
        /// To which version does this update apply
        /// </summary>
        public Version AppliesTo => new Version(5, 0, 0, 0);

        /// <summary>
        /// Execute the update if there is no customerkey.
        /// </summary>
        public void Update()
        {
            configurationData.CheckForChangesNeeded();

            if (string.IsNullOrEmpty(configurationData.FetchReadOnly().CustomerKey))
            {
                ICustomerLicense customerLicense = getCustomerLicense(string.Empty);

                customerLicense.Save();
            } 
        }
    }
}
