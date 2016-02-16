using System.Collections.Generic;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using System.Windows.Forms;
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
        /// <param name="store"></param>
        void DeleteStore(StoreEntity store);

        /// <summary>
        /// Enforces the capabilities for this license.
        /// Throws ShipWorksLicenseException when out of compliance
        /// </summary>
        /// <param name="context">The context.</param>
        void EnforceCapabilities(EnforcementContext context);

        /// <summary>
        /// Enforces the capabilities for this license.
        /// Prompts user to take action when out of compliance
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="owner">The owner.</param>
        void EnforceCapabilities(EnforcementContext context, IWin32Window owner);

        /// <summary>
        /// Enforces the capabilites for this license,
        /// related to the given EditionFeature.
        /// </summary>
        /// <param name="feature">The feature.</param>
        /// <param name="context">The context.</param>
        IEnumerable<EnumResult<ComplianceLevel>> EnforceCapabilites(EditionFeature feature, EnforcementContext context);
    }
}
