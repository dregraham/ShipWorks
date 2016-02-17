using System.Windows.Forms;
using Interapptive.Shared.Utility;
using ShipWorks.Editions;

namespace ShipWorks.ApplicationCore.Licensing.LicenseEnforcement
{
    /// <summary>
    /// Interface for enforcing license capabilities
    /// </summary>
    public interface ILicenseEnforcer
    {
        /// <summary>
        /// The priority for this enforcer
        /// </summary>
        int Priority { get; }

        /// <summary>
        /// The edition feature enforced
        /// </summary>
        EditionFeature EditionFeature { get; }

        /// <summary>
        /// Enforces license capabilities
        /// </summary>
        void Enforce(ILicenseCapabilities capabilities, EnforcementContext context, IWin32Window owner);

        /// <summary>
        /// Enforces license capabilities
        /// </summary>
        EnumResult<ComplianceLevel> Enforce(ILicenseCapabilities capabilities, EnforcementContext context);
    }
}