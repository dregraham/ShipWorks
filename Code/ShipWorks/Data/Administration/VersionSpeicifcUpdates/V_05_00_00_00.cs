using System;
using ShipWorks.ApplicationCore.Licensing;

namespace ShipWorks.Data.Administration.VersionSpeicifcUpdates
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

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="customerLicense"></param>
        public V_05_00_00_00(Func<string, ICustomerLicense> getCustomerLicense)
        {
            this.getCustomerLicense = getCustomerLicense;
        }

        /// <summary>
        /// To which version does this update apply
        /// </summary>
        public Version AppliesTo => new Version(5, 0, 0, 0);

        /// <summary>
        /// Execute the update
        /// </summary>
        public void Update()
        {
            ICustomerLicense customerLicense = getCustomerLicense(string.Empty);

            customerLicense.Save();
        }
    }
}
