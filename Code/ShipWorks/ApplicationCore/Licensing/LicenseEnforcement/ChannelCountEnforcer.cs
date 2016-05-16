using Interapptive.Shared.Utility;
using ShipWorks.Editions;
using System.Windows.Forms;

namespace ShipWorks.ApplicationCore.Licensing.LicenseEnforcement
{
    /// <summary>
    /// Enforces Channel Counts for the license
    /// </summary>
    public class ChannelCountEnforcer : ILicenseEnforcer
    {
        private readonly IChannelLimitDlgFactory channelLimitDlgFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public ChannelCountEnforcer(IChannelLimitDlgFactory channelLimitDlgFactory)
        {
            this.channelLimitDlgFactory = channelLimitDlgFactory;
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
        /// ChannelCount doesn't apply to trials
        /// </summary>
        public bool AppliesTo(ILicenseCapabilities capabilities) => !capabilities.IsInTrial;

        /// <summary>
        /// Enforces the ChannelCount and displays a dlg on the given owner
        /// </summary>
        public void Enforce(ILicenseCapabilities capabilities, EnforcementContext context, IWin32Window owner)
        {
            if (Enforce(capabilities,context).Value == ComplianceLevel.NotCompliant)
            {
                    IDialog channelLimitDlg = channelLimitDlgFactory.GetChannelLimitDlg(owner, EditionFeature, context);
                    channelLimitDlg.ShowDialog();
            }
        }

        /// <summary>
        /// Enforces the ChannelCount feature and returns an enum result
        /// </summary>
        public EnumResult<ComplianceLevel> Enforce(ILicenseCapabilities capabilities, EnforcementContext context)
        {
            const int unlimitedChannels = -1;

            if (capabilities.ChannelLimit == unlimitedChannels)
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