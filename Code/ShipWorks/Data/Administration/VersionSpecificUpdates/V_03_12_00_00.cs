using System;
using ShipWorks.AddressValidation;
using ShipWorks.Data.Connection;

namespace ShipWorks.Data.Administration.VersionSpecificUpdates
{
    /// <summary>
    /// ShipWorks update that should be applied for a specific version
    /// </summary>
    /// <remarks>
    /// If we were upgrading from this version, add AddressValidation filters.
    /// </remarks>
    public class V_03_12_00_00 : IVersionSpecificUpdate
    {
        /// <summary>
        /// This isn't critical. We don't want AV filters showing up after every upgrade
        /// if the user doesn't want them...
        /// </summary>
        public bool AlwaysRun => false;

        /// <summary>
        /// To which version does this update apply
        /// </summary>
        public Version AppliesTo => new Version(3, 12, 0, 0);

        /// <summary>
        /// Execute the update
        /// </summary>
        public void Update()
        {
            AddressValidationDatabaseUpgrade addressValidationDatabaseUpgrade = new AddressValidationDatabaseUpgrade();
            ExistingConnectionScope.ExecuteWithAdapter(addressValidationDatabaseUpgrade.Upgrade);
        }
    }
}
