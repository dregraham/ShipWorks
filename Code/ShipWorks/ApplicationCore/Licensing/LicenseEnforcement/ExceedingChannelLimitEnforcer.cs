using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.Utility;
using ShipWorks.Editions;

namespace ShipWorks.ApplicationCore.Licensing.LicenseEnforcement
{
    public class ExceedingChannelLimitEnforcer : ILicenseEnforcer
    {
        /// <summary>
        /// The priority for this enforcer
        /// </summary>
        public EnforcementPriority Priority => EnforcementPriority.Medium;

        public EditionFeature EditionFeature => EditionFeature.ChannelCount;

        /// <remarks>
        /// Adding a store will get an error from tango. We don't need to call enforce here.
        /// </remarks>
        /// <remarks>
        /// Adding a store will get an error from tango. We don't need to 
        /// enforce anything here.
        /// </remarks>
        public void Enforce(ILicenseCapabilities capabilities, EnforcementContext context, IWin32Window owner)
        {
            
        }

        /// <summary>
        /// Enforces license capabilities
        /// </summary>
        public EnumResult<ComplianceLevel> Enforce(ILicenseCapabilities capabilities, EnforcementContext context)
        {
            // We are not over the limit but we are trying to add a new channel which would put us over the limit
            if (capabilities.ActiveChannels == capabilities.ChannelLimit &&
                context == EnforcementContext.ExceedingChannelLimit)
            {
                return new EnumResult<ComplianceLevel>(ComplianceLevel.NotCompliant,
                    "You will exceed your channel limit. Please upgrade your plan or delete an existing channel to add a new channel");
            }

            return new EnumResult<ComplianceLevel>(ComplianceLevel.Compliant);
        }
    }
}
