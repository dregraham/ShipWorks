using System;
using ShipWorks.AddressValidation;
using ShipWorks.Data.Connection;

namespace ShipWorks.Data.Administration.VersionSpecifcUpdates
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
