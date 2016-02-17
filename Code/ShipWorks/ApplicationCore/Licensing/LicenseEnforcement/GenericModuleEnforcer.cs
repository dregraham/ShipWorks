using System.Windows.Forms;
using Interapptive.Shared.Utility;
using ShipWorks.Editions;

namespace ShipWorks.ApplicationCore.Licensing.LicenseEnforcement
{
    /// <summary>
    /// Ensure that, if customer has a GenericModule installed, they are allowed to by tango
    /// </summary>
    public class GenericModuleEnforcer : ILicenseEnforcer
    {
        /// <summary>
        /// The priority for this enforcer
        /// </summary>
        public int Priority => 3;

        /// <summary>
        /// The edition feature enforced
        /// </summary>
        public EditionFeature EditionFeature => EditionFeature.GenericModule;

        /// <summary>
        /// Enforces license capabilities
        /// </summary>
        public void Enforce(ILicenseCapabilities capabilities, EnforcementContext context, IWin32Window owner)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Enforces license capabilities
        /// </summary>
        public EnumResult<ComplianceLevel> Enforce(ILicenseCapabilities capabilities, EnforcementContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}