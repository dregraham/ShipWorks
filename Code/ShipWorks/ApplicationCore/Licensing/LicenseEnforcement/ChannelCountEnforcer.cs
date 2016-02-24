﻿using System;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Editions;

namespace ShipWorks.ApplicationCore.Licensing.LicenseEnforcement
{
    /// <summary>
    /// Enforces Channel Counts for the license
    /// </summary>
    public class ChannelCountEnforcer : ILicenseEnforcer
    {
        private readonly IChannelLimitDlgFactory channelLimitDlgFactory;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public ChannelCountEnforcer(IChannelLimitDlgFactory channelLimitDlgFactory, Func<Type, ILog> logFactory)
        {
            this.channelLimitDlgFactory = channelLimitDlgFactory;
            log = logFactory(typeof(ChannelCountEnforcer));
        }

        /// <summary>
        /// Medium Priority
        /// </summary>
        public EnforcementPriority Priority => EnforcementPriority.Medium;

        /// <summary>
        /// Enforces the ChannelCount feature
        /// </summary>
        public EditionFeature EditionFeature => EditionFeature.ChannelCount;

        /// <summary>
        /// Enforces the ChannelCount and displays a dlg on the given owner
        /// </summary>
        public void Enforce(ILicenseCapabilities capabilities, EnforcementContext context, IWin32Window owner)
        {
            if (Enforce(capabilities,context).Value == ComplianceLevel.NotCompliant)
            {
                    IChannelLimitDlg channelLimitDlg = channelLimitDlgFactory.GetChannelLimitDlg(owner, EditionFeature, context);
                    channelLimitDlg.ShowDialog();
            }
        }

        /// <summary>
        /// Enforces the ChannelCount feature and returns an enum result
        /// </summary>
        public EnumResult<ComplianceLevel> Enforce(ILicenseCapabilities capabilities, EnforcementContext context)
        {
            if (capabilities.IsInTrial)
            {
                return new EnumResult<ComplianceLevel>(ComplianceLevel.Compliant, string.Empty);
            }

            // Determine how many channels over the limit we are
            int numberOfChannelsOverLimit = capabilities.ActiveChannels - capabilities.ChannelLimit;

            // We are over the channel limit, display an error stating how many channels need to be removed
            if ((numberOfChannelsOverLimit > 0))
            {
                // Determine if we should use the plural for of channel in our error message
                string plural = numberOfChannelsOverLimit > 1 ? "s" : string.Empty;


                string forbiddenActivity = context == EnforcementContext.OnAddingStore ||
                                           context == EnforcementContext.ExceedingChannelLimit
                    ? "adding a new store."
                    : "downloading orders and creating shipment labels.";

                string error = "You have exceeded your channel limit. Please upgrade your plan or delete " +
                               $"{numberOfChannelsOverLimit} channel{plural} to continue {forbiddenActivity}";
                
                // Return not compliant and an error to display to the user
                return new EnumResult<ComplianceLevel>(ComplianceLevel.NotCompliant, error);
            }

            return new EnumResult<ComplianceLevel>(ComplianceLevel.Compliant, string.Empty);
        }
    }
}