﻿using Interapptive.Shared.Utility;
using ShipWorks.Editions;
using System.Windows.Forms;

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
        EnforcementPriority Priority { get; }

        /// <summary>
        /// The edition feature enforced
        /// </summary>
        EditionFeature EditionFeature { get; }

        /// <summary>
        /// True if Enforcer applies to LicenseCapabilities
        /// </summary>
        bool AppliesTo(ILicenseCapabilities capabilities);

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