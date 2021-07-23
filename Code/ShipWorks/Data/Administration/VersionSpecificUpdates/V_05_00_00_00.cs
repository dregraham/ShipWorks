using System;
using ShipWorks.ApplicationCore.Licensing;

namespace ShipWorks.Data.Administration.VersionSpecificUpdates
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
        private readonly IConfigurationData configurationData;
        private readonly ICustomerLicenseWriter customerLicenseWriter;

        /// <summary>
        /// Always run just in case it has never been run before.
        /// </summary>
        public bool AlwaysRun => true;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="customerLicense"></param>
        public V_05_00_00_00(IConfigurationData configurationData, ICustomerLicenseWriter customerLicenseWriter)
        {
            this.configurationData = configurationData;
            this.customerLicenseWriter = customerLicenseWriter;
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
                // We're passing in an empty string, which seems pointless, but further down the method chain the 
                // empty string gets overwritten and encrypted.
                customerLicenseWriter.Write(string.Empty, CustomerLicenseKeyType.WebReg);
            } 
        }
    }
}
