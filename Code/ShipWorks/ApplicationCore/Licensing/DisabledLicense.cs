using System.Collections.Generic;
using System.Windows.Forms;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Licensing.LicenseEnforcement;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Editions;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// If an error occurs we can return a disabled license because we can't verify the license.
    /// </summary>
    public class DisabledLicense : ILicense
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DisabledLicense(string disabledReason)
        {
            DisabledReason = disabledReason;
        }

        /// <summary>
        /// Does nothing
        /// </summary>
        public void Refresh()
        {
            // Do nothing
        }

        /// <summary>
        /// Reason license is disabled
        /// </summary>
        public string DisabledReason { get; set; }

        /// <summary>
        /// Returns True
        /// </summary>
        public bool IsDisabled => true;

        /// <summary>
        /// Disabled licenses do not have channel limits
        /// </summary>
        /// <remarks>
        /// Always return 0
        /// </remarks>
        public int NumberOfChannelsOverLimit => 0;

        /// <summary>
        /// Is the user over their shipment limit
        /// </summary>
        public bool IsShipmentLimitReached => true;

        /// <summary>
        /// Throws - We shouldn't try to access the key if the license is disabled.
        /// </summary>
        public string Key
        {
            get
            {
                throw new ShipWorksLicenseException("Key not a valid property for a disabled license.");
            }
        }

        /// <summary>
        /// Not Legacy.
        /// </summary>
        public bool IsLegacy => false;

        /// <summary>
        /// Not over limit, but I did debate throwing for this...
        /// </summary>
        public bool IsOverChannelLimit => true;

        /// <summary>
        /// Throws - We shouldn't try to activate a disabled license.
        /// </summary>
        public EnumResult<LicenseActivationState> Activate(StoreEntity store)
        {
            throw new ShipWorksLicenseException("Activate not valid for a disabled license.");
        }
        
        /// <summary>
        /// Throws we should not try to delete a disabled license
        /// </summary>
        /// <param name="store"></param>
        public void DeleteStore(StoreEntity store)
        {
            throw new ShipWorksLicenseException("Cannot delete a store using a disabled store license");
        }

        /// <summary>
        /// Throws should not enforce disabled license
        /// </summary>
        public void EnforceCapabilities(EnforcementContext context)
        {
            throw new ShipWorksLicenseException("Capabilities are not valid for a disabled license.");
        }

        /// <summary>
        /// Throws should not enforce disabled license
        /// </summary>
        public void EnforceCapabilities(EnforcementContext context, IWin32Window owner)
        {
            throw new ShipWorksLicenseException("Capabilities are not valid for a disabled license.");
        }

        /// <summary>
        /// Throws should not enforce disabled license
        /// </summary>
        public IEnumerable<EnumResult<ComplianceLevel>> EnforceCapabilities(EditionFeature feature, EnforcementContext context)
        {
            throw new ShipWorksLicenseException("Capabilities are not valid for a disabled license.");
        }
    }
}