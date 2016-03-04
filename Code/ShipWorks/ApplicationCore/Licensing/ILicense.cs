using System.Collections.Generic;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using System.Windows.Forms;
using ShipWorks.ApplicationCore.Dashboard.Content;
using ShipWorks.ApplicationCore.Licensing.LicenseEnforcement;
using ShipWorks.Editions;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// License describes the capabilities of the customer's license.
    /// </summary>
    public interface ILicense
    {
        /// <summary>
        /// Refresh the License capabilities from Tango
        /// </summary>
        void Refresh();

        /// <summary>
        /// Reason the license is Disabled
        /// </summary>
        string DisabledReason { get; set; }

        /// <summary>
        /// Is the license Disabled
        /// </summary>
        bool IsDisabled { get; }

        /// <summary>
        /// The license key
        /// </summary>
        string Key { get; }

        /// <summary>
        /// Is the license legacy
        /// </summary>
        bool IsLegacy { get; }

        /// <summary>
        /// Activate a new store
        /// </summary>
        EnumResult<LicenseActivationState> Activate(StoreEntity store);

        /// <summary>
        /// Deletes a store
        /// </summary>
        void DeleteStore(StoreEntity store);

        /// <summary>
        /// Enforces the capabilities for this license.
        /// Throws ShipWorksLicenseException when out of compliance
        /// </summary>
        void EnforceCapabilities(EnforcementContext context);

        /// <summary>
        /// Enforces the capabilities for this license.
        /// Prompts user to take action when out of compliance
        /// </summary>
        void EnforceCapabilities(EnforcementContext context, IWin32Window owner);

        /// <summary>
        /// Enforces the capabilities for this license,
        /// related to the given EditionFeature.
        /// </summary>
        IEnumerable<EnumResult<ComplianceLevel>> EnforceCapabilities(EditionFeature feature, EnforcementContext context);

        /// <summary>
        /// Returns a dashboard item to display to the user
        /// </summary>
        /// <returns></returns>
        DashboardLicenseItem CreateDashboardMessage();

        /// <summary>
        /// Associates a free Stamps.com account with a customer license.
        /// </summary>
        /// <param name="uspsAccount">The usps account.</param>
        void AssociateStampsUsername(UspsAccountEntity uspsAccount);
    }
}
